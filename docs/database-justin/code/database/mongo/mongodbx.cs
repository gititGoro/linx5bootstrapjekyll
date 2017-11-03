using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.IO;

namespace Twenty57.Linx.Components.MongoDB
{
	public class MongoDBX : IDisposable
	{
		private const int DatabaseObjectSampleSize = 3;

		private string connectionString;
		private MongoServer server = null;
		private MongoDatabase database = null;

		public MongoDBX(string connectionString)
		{
			this.connectionString = connectionString;

			if (MongoUrl.Create(connectionString).DatabaseName == null)
				throw new MongoException("Database name not specified in connection string");
		}

		private MongoServer Server
		{
			get
			{
				if (server == null)
				{
					var clientSettings = new MongoClient(this.connectionString).Settings;
					MongoServerSettings serverSettings = MongoServerSettings.FromClientSettings(clientSettings);
					Log(string.Format("Connecting to server <{0}>", serverSettings.Server));
					server = new MongoServer(serverSettings);
				}
				return server;
			}
		}

		private MongoDatabase Database
		{
			get
			{
				if (database == null)
				{
					string databaseName = MongoUrl.Create(connectionString).DatabaseName;
					database = Server.GetDatabase(databaseName);
				}
				return database;
			}
		}

		public List<MongoDatabaseObject> GetDatabaseObjects(string collectionName)
		{
			MongoCollection<BsonDocument> collection = Database.GetCollection<BsonDocument>(collectionName);
			MongoCursor<BsonDocument> documents = collection.FindAll();

			int docCount = 0;
			IEnumerable<MongoDatabaseObject> dbObjects = new List<MongoDatabaseObject>();

			foreach (BsonDocument document in documents)
			{
				docCount++;
				if (docCount > DatabaseObjectSampleSize) break;
				dbObjects = dbObjects.Concat(GetDatabaseObjects("", document));
			}
			return dbObjects.Distinct(new MongoDatabaseObject.EqualityComparer()).OrderBy(v => v.Path).ToList();
		}

		public IEnumerable<T> Find<T>(string collectionName, string query, string fields, string sort, int skip, int limit)
		{
			RegisterClassForBSONSerialisation<T>();
			MongoCursor<BsonDocument> cursor = Find(collectionName, query, fields, sort, skip, limit);

			return cursor.Select(DeserializeRetrieved<T>);
		}

		public IEnumerable<string> FindAsJson(string collectionName, string query, string fields, string sort, int skip, int limit)
		{
			return Find(collectionName, query, fields, sort, skip, limit).Select(BsonToJson);
		}

		public IEnumerable<T> Aggregate<T>(string collectionName, string aggregationPipeline)
		{
			RegisterClassForBSONSerialisation<T>();
			return Aggregate(collectionName, aggregationPipeline).Select(DeserializeRetrieved<T>);
		}

		public IEnumerable<string> AggregateAsJson(string collectionName, string aggregationPipeline)
		{
			return Aggregate(collectionName, aggregationPipeline).Select(BsonToJson);
		}

		public void Insert<T>(string collectionName, T data)
		{
			BsonDocument deserialized;
			var stringData = data as string;

			if (stringData != null)
			{
				Exception exception;
				if (!AttemptDeserialize(data as string, out deserialized, out exception))
					throw new Exception("Invalid JSON format in 'Data' property", exception);
				Insert(collectionName, deserialized);
				return;
			}
			RegisterClassForBSONSerialisation<T>();
			IEnumerable<BsonClassMap> maps = BsonClassMap.GetRegisteredClassMaps();
			BsonClassMap map = maps.First(v => v.ClassType == typeof(T));

			MongoCollection<T> collection = Database.GetCollection<T>(collectionName);
			var result = collection.Insert(data);

			Log(string.Format("{0} item(s) inserted.", result.DocumentsAffected));
		}

		public void Update(string collectionName, string criteria, string updateOperation, bool insertIfNotFound)
		{
			var flags = UpdateFlags.Multi;
			if (insertIfNotFound)
				flags |= UpdateFlags.Upsert;

			BsonDocument deserialisedOperation;
			Exception exception;
			if (!AttemptDeserialize(updateOperation, out deserialisedOperation, out exception))
				throw new Exception("Invalid JSON format in 'Update Operation'", exception);
			var result = Database.GetCollection<BsonDocument>(collectionName)
				.Update(new QueryDocument(ParseQuery(criteria)), new UpdateDocument(deserialisedOperation), flags);

			Log(string.Format("{0} item(s) {1}.", result.DocumentsAffected, (result.DocumentsAffected == 0) || (result.UpdatedExisting) ? "updated" : "inserted"));
		}

		public void Replace<T>(string collectionName, string criteria, T data, bool insertIfNotFound)
		{
			var flags = UpdateFlags.None;
			if (insertIfNotFound)
				flags |= UpdateFlags.Upsert;
			BsonDocument deserialized;
			var stringData = data as string;
			if (stringData != null)
			{
				Exception exception;
				if (!AttemptDeserialize(data as string, out deserialized, out exception))
					throw new Exception("Invalid JSON format in 'Data'", exception);
				Replace(collectionName, criteria, deserialized, insertIfNotFound);
				return;
			}
			RegisterClassForBSONSerialisation<T>();
			MongoCollection<T> collection = Database.GetCollection<T>(collectionName);

			WriteConcernResult result;
			if (String.IsNullOrEmpty(criteria) && typeof(T) != typeof(BsonDocument))
			{
				IEnumerable<BsonClassMap> maps = BsonClassMap.GetRegisteredClassMaps();
				BsonMemberMap idMap = maps.First(v => v.ClassType == typeof(T)).IdMemberMap;
				if (idMap == null)
					throw new MongoException(
						"Cannot perform mongo replace: no criteria specified and the object does not have an id field");
				var id = (string)idMap.Getter(data);
				if (String.IsNullOrEmpty(id))
					throw new MongoException("Cannot perform mongo replace: no criteria specified and the object's id field is not set");

				result = collection.Update(Query.EQ("_id", new ObjectId(id)), global::MongoDB.Driver.Builders.Update.Replace(data), flags);
			}
			else
			{
				result = collection.Update(new QueryDocument(ParseQuery(criteria)), global::MongoDB.Driver.Builders.Update.Replace(data),
					flags);
			}

			Log(string.Format("{0} items {1}.", result.DocumentsAffected, (result.DocumentsAffected == 0) || (result.UpdatedExisting) ? "replaced" : "inserted"));
		}

		public void Delete(string collectionName, string criteria)
		{
			MongoCollection collection = Database.GetCollection(collectionName);
			if (String.IsNullOrEmpty(criteria))
				throw new ArgumentException("No criteria specified for mongo delete operation");
			var result = collection.Remove(new QueryDocument(ParseQuery(criteria)));

			Log(string.Format("{0} item(s) removed.", result.DocumentsAffected));
		}

		public void DeleteAll(string collectionName)
		{
			var result = Database.GetCollection(collectionName).RemoveAll();

			Log(string.Format("{0} item(s) removed.", result.DocumentsAffected));
		}

		public IEnumerable<T> MapReduce<T>(string collectionName, string criteria, string mapFunction, string reduceFunction, string mapFinalize, string sort, int limit)
		{
			RegisterClassForBSONSerialisation<T>();
			return MapReduce(collectionName, criteria, mapFunction, reduceFunction, mapFinalize, sort, limit).GetResults().Select(DeserializeRetrieved<T>);
		}

		public IEnumerable<string> MapReduceAsJson(string collectionName, string criteria, string mapFunction, string reduceFunction, string mapFinalize, string sort, int limit)
		{
			return MapReduce(collectionName, criteria, mapFunction, reduceFunction, mapFinalize, sort, limit).GetResults().Select(BsonToJson);
		}

		public void Dispose()
		{
			if (server != null)
			{
				server.Disconnect();
				Log("Disconnected from server.");
			}
		}

		public delegate void LogEventHandler(string message);
		public event LogEventHandler LogEvent;

		private void Log(string message)
		{
			if (LogEvent != null)
				LogEvent(message);
		}

		private IEnumerable<MongoDatabaseObject> GetDatabaseObjects(string path, BsonDocument document)
		{
			foreach (string name in document.Names)
			{
				var mongoObject = new MongoDatabaseObject(name, path);

				BsonValue value = document.GetElement(name).Value;
				if (value.BsonType == BsonType.Array)
				{
					BsonArray arrayItem = value.AsBsonArray;
					if (arrayItem.Count > 0)
						value = arrayItem.First();
				}

				if (value.BsonType == BsonType.Document)
					mongoObject.ComplexType = true;

				yield return mongoObject;

				if (value.BsonType == BsonType.Document)
					foreach (MongoDatabaseObject subobject in GetDatabaseObjects(mongoObject.Path, value.AsBsonDocument))
						yield return subobject;
			}
		}

		private void RegisterClassForBSONSerialisation<T>()
		{
			if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
				BsonClassMap.RegisterClassMap<T>(cm =>
				{
					cm.AutoMap();
					cm.SetIgnoreExtraElements(true);
					if (cm.IdMemberMap != null)
					{
						cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
						cm.IdMemberMap.SetIgnoreIfNull(true);
						cm.IdMemberMap.SetShouldSerializeMethod(v => !String.IsNullOrEmpty(v as string));
					}
				});
		}

		private BsonDocument ParseQuery(string criteria)
		{
			BsonDocument query;
			Exception exception;
			if (!AttemptDeserialize(criteria, out query, out exception))
				throw new ArgumentException("Invalid criteria", exception);

			return query;
		}

		private BsonDocument[] ParseAggregationPipeline(string aggregation)
		{
			BsonArray pipeline;
			Exception exception;
			if (!AttemptDeserialize(aggregation, out pipeline, out exception))
				throw new ArgumentException("Invalid aggregation pipeline", exception);

			return pipeline.Select(v => v.AsBsonDocument).ToArray();
		}

		private T DeserializeRetrieved<T>(BsonDocument itemDocument)
		{
			Log("Retrieved " + itemDocument);
			return BsonSerializer.Deserialize<T>(itemDocument);
		}

		private bool AttemptDeserialize<T>(string json, out T result, out Exception serialisationException)
		{
			serialisationException = null;
			result = default(T);
			if (String.IsNullOrEmpty(json)) return false;
			try
			{
				result = BsonSerializer.Deserialize<T>(json);
				return true;
			}
			catch (Exception exception)
			{
				serialisationException = exception;
				return false;
			}
		}

		private MongoCursor<BsonDocument> Find(string collectionName, string query, string fields, string sort, int skip, int limit)
		{
			MongoCollection<BsonDocument> collection = Database.GetCollection<BsonDocument>(collectionName);
			MongoCursor<BsonDocument> cursor = (string.IsNullOrEmpty(query)) ? collection.FindAll() : collection.Find(new QueryDocument(ParseQuery(query)));

			if (!string.IsNullOrEmpty(fields))
			{
				ParseQuery(fields).ToList().ForEach(item =>
				{
					if (item.Value == 0)
						cursor.SetFields(Fields.Exclude(item.Name));
					else
						cursor.SetFields(Fields.Include(item.Name));
				});
			}

			if (!string.IsNullOrEmpty(sort))
			{
				ParseQuery(sort).ToList().ForEach(itemtoSort =>
				{
					if (itemtoSort.Value > 0)
						cursor.SetSortOrder(SortBy.Ascending(itemtoSort.Name));
					else
						cursor.SetSortOrder(SortBy.Descending(itemtoSort.Name));
				});
			}

			cursor.SetSkip(skip);
			cursor.SetLimit(limit);
			return cursor;
		}

		private IEnumerable<BsonDocument> Aggregate(string collectionName, string aggregationPipeline)
		{
			MongoCollection<BsonDocument> collection = Database.GetCollection<BsonDocument>(collectionName);

			var aggregateArgs = new AggregateArgs();
			if (!string.IsNullOrEmpty(aggregationPipeline))
			{
				aggregateArgs.Pipeline = ParseAggregationPipeline(aggregationPipeline);
			}

			return collection.Aggregate(aggregateArgs);
		}

		private string BsonToJson(BsonDocument bsonDocument)
		{
			Log("Retrieved " + bsonDocument);
			return bsonDocument.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
		}

		private MapReduceResult MapReduce(string collectionName, string criteria, string mapFunction, string reduceFunction, string mapFinalize, string sort, int limit)
		{
			MongoCollection<BsonDocument> collection = Database.GetCollection<BsonDocument>(collectionName);
			MapReduceArgs args = new MapReduceArgs();
			MapReduceResult result = null;

			if (!string.IsNullOrEmpty(mapFunction))
				args.MapFunction = mapFunction;

			if (!string.IsNullOrEmpty(reduceFunction))
				args.ReduceFunction = reduceFunction;

			if (!string.IsNullOrEmpty(mapFinalize))
				args.FinalizeFunction = mapFinalize;

			if (!string.IsNullOrEmpty(criteria))
				args.Query = Query.Create(new QueryDocument(ParseQuery(criteria)));

			if (!string.IsNullOrEmpty(sort))
			{
				ParseQuery(sort).ToList().ForEach(itemtoSort =>
				{
					if (itemtoSort.Value > 0)
						args.SortBy = SortBy.Ascending(itemtoSort.Name);
					else
						args.SortBy = SortBy.Descending(itemtoSort.Name);
				});
			}

			args.Limit = limit;

			if (!string.IsNullOrEmpty(mapFunction) && !string.IsNullOrEmpty(reduceFunction))
				result = collection.MapReduce(args);
			return result;
		}
	}
}