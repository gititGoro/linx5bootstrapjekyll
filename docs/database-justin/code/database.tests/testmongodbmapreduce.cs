using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Components.Database.Mongo.MongoDBMapReduce;
using Twenty57.Linx.Plugin.Common.CodeGeneration;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.MongoDB.Tests
{
	[TestFixture]
	public class TestMongoDBMapReduce
	{
		private MongoDBMapReduceExecutor fixtureComponentExecutor;
		private MongoDBMapReduceExecutor fixtureDbReduceMongoDb;
		private TypeReference restaurantType;
		private string mapFunction;
		private string reduceFunction;

		[SetUp]
		public void SetUp()
		{
			fixtureComponentExecutor = new MongoDBMapReduceExecutor();
			fixtureDbReduceMongoDb = fixtureComponentExecutor;
			fixtureDbReduceMongoDb.ConnectionString = "mongodb://localhost:27017/testRestaurants";
			fixtureDbReduceMongoDb.Collection = "TestCollection";
			fixtureDbReduceMongoDb.ReturnModeType = MongoDBMapReduceShared.ReturnModeType.ListOfRows;

			restaurantType = TypeReference.CreateGeneratedType(
				new TypeProperty("name", typeof(string)),
				new TypeProperty("_id", typeof(string)),
				new TypeProperty("value", typeof(object)),
				new TypeProperty("restaurant_id", typeof(object))
				);
			var connectionString = "mongodb://localhost:27017/testRestaurants";
			var databaseName = MongoUrl.Create(connectionString).DatabaseName;

			var clientSettings = new MongoClient(connectionString).Settings;
			MongoServerSettings serverSettings = MongoServerSettings.FromClientSettings(clientSettings);
			var server = new MongoServer(serverSettings);

			var database = server.GetDatabase(databaseName);
			MongoCollection<BsonDocument> test = database.GetCollection<BsonDocument>("TestCollection");

			test.RemoveAll();
			test.Insert(new BsonDocument {
				{ "borough", "Bronx" },
				{"cuisine", "Bakery"},
				{ "name", "Morris Park Bake Shop"},
				{ "restaurant_id", "30075445"},
				{"grades",  new BsonArray { new BsonDocument { { "date", Convert.ToDateTime("2014-03-03T00:00:00.000Z") }, { "grade", "A" }, { "score", 2 } }
						 ,  new BsonDocument {{ "date" , Convert.ToDateTime("2013-09-11T00:00:00.000Z") }, { "grade","A" }, { "score", 6 } }
						 ,  new BsonDocument {{ "date" , Convert.ToDateTime("2013-01-24T00:00:00.000Z") }, { "grade","A" }, { "score", 10 } }} }
			});

			test.Insert(new BsonDocument {
				{"borough", "Brooklyn" },
				{"cuisine", "Hamburgers"},
				{ "name", "Wendy'S"},
				{ "restaurant_id", "30112340"},
				{"grades",  new BsonArray { new BsonDocument { { "date", Convert.ToDateTime("2014-12-30T00:00:00.000Z") }, { "grade", "A" }, { "score", 8 } }
						 ,  new BsonDocument {{ "date" , Convert.ToDateTime("2014-07-01T00:00:00.000Z") }, { "grade","B" }, { "score", 23 } }
						 ,  new BsonDocument {{ "date" , Convert.ToDateTime("2013-04-30T00:00:00.000Z") }, { "grade","A" }, { "score", 12 } }} }
			});

			mapFunction = @"function Map() {					
						for (var i = 0; i < this.grades.length; i++) 
						{
							emit(this._id, this.grades[i].score);
						}			
				}";

			reduceFunction = @"function Reduce(key, values) 
					{
						total = { count: 0, sum: 0 };
						total.count = values.length;
						for (var i = 0; i < values.length; i++)
						{
							total.sum += values[i];
						}
						return total;
					}";
		}

		[Test]
		public void TestExecuteMapReduceWithListOfRowsReturnMode()
		{
			fixtureDbReduceMongoDb.OutputType = restaurantType;
			fixtureDbReduceMongoDb.LoopResults = false;
			fixtureDbReduceMongoDb.Map = mapFunction;
			fixtureDbReduceMongoDb.Reduce = reduceFunction;
			fixtureDbReduceMongoDb.ReturnModeType = MongoDBMapReduceShared.ReturnModeType.ListOfRows;

			var output = fixtureComponentExecutor.ExecuteCompiled();

			Assert.NotNull(output);
			Assert.AreEqual(2, output.Value.Count);
			Assert.AreEqual(18, output.Value[0].value.sum);
			Assert.AreEqual(3, output.Value[0].value.count);
			Assert.AreEqual(43, output.Value[1].value.sum);
			Assert.AreEqual(3, output.Value[1].value.count);
		}

		[Test]
		public void TestExecuteMapReduceWithFirstRowReturnMode()
		{
			fixtureDbReduceMongoDb.OutputType = restaurantType;
			fixtureDbReduceMongoDb.LoopResults = false;
			fixtureDbReduceMongoDb.Map = mapFunction;
			fixtureDbReduceMongoDb.Reduce = reduceFunction;
			fixtureDbReduceMongoDb.ReturnModeType = MongoDBMapReduceShared.ReturnModeType.FirstRow;

			dynamic output = fixtureComponentExecutor.ExecuteCompiled().Value;

			Assert.NotNull(output);
			Assert.AreEqual(18, output.value.sum);
			Assert.AreEqual(3, output.value.count);
		}

		[Test]
		public void TestExecuteMapReduceWithFirstRowReturnModeWhenNoRows()
		{
			fixtureDbReduceMongoDb.OutputType = restaurantType;
			fixtureDbReduceMongoDb.LoopResults = false;
			fixtureDbReduceMongoDb.Map = mapFunction;
			fixtureDbReduceMongoDb.Reduce = reduceFunction;
			fixtureDbReduceMongoDb.ReturnModeType = MongoDBMapReduceShared.ReturnModeType.FirstRow;
			fixtureDbReduceMongoDb.Query =
				@"
					{ age: { $gt: 100 } }
				";

			Assert.That(() => fixtureComponentExecutor.ExecuteCompiled(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("No rows returned by query.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteMapReduceWithRowByRowReturnMode()
		{
			fixtureDbReduceMongoDb.OutputType = restaurantType;
			fixtureDbReduceMongoDb.LoopResults = false;
			fixtureDbReduceMongoDb.Map = mapFunction;
			fixtureDbReduceMongoDb.Reduce = reduceFunction;
			fixtureDbReduceMongoDb.ReturnModeType = MongoDBMapReduceShared.ReturnModeType.RowByRow;

			var output = fixtureComponentExecutor.ExecuteCompiled();

			Assert.NotNull(output);
			Assert.AreEqual(2, output.ExecutionPathResult.Count());
		}

		[Test]
		public void TestExecuteMapReduceWithRowByRowAsString()
		{
			fixtureDbReduceMongoDb.OutputType = TypeReference.Create(typeof(string));
			fixtureDbReduceMongoDb.LoopResults = false;
			fixtureDbReduceMongoDb.Map = mapFunction;
			fixtureDbReduceMongoDb.Reduce = reduceFunction;
			fixtureDbReduceMongoDb.ReturnModeType = MongoDBMapReduceShared.ReturnModeType.RowByRow;

			var output = fixtureComponentExecutor.ExecuteCompiled();

			Assert.NotNull(output);
			Assert.AreEqual(2, output.ExecutionPathResult.Count());
		}

		[Test]
		public void TestExecuteMapReduceFinalize()
		{
			fixtureDbReduceMongoDb.OutputType = restaurantType;
			fixtureDbReduceMongoDb.Map = mapFunction;
			fixtureDbReduceMongoDb.Reduce = reduceFunction;
			fixtureDbReduceMongoDb.Finalize = @"function Finalize(key, reduced) 
					{
							reduced.average = reduced.sum / reduced.count;
							return reduced;
					}";

			var output = fixtureComponentExecutor.ExecuteCompiled();

			Assert.NotNull(output);
			Assert.AreEqual(2, output.Value.Count);

			Assert.AreEqual(18, output.Value[0].value.sum);
			Assert.AreEqual(3, output.Value[0].value.count);
			Assert.AreEqual(6, output.Value[0].value.average);

			Assert.AreEqual(43, output.Value[1].value.sum);
			Assert.AreEqual(3, output.Value[1].value.count);
			Assert.AreEqual(14, Math.Round(output.Value[1].value.average));
		}

		[Test]
		public void TestQuery()
		{
			fixtureDbReduceMongoDb.OutputType = restaurantType;
			fixtureDbReduceMongoDb.Query = "{\"borough\":\"Bronx\"}";
			fixtureDbReduceMongoDb.LoopResults = false;
			fixtureDbReduceMongoDb.Map = mapFunction;
			fixtureDbReduceMongoDb.Reduce = reduceFunction;

			var output = fixtureComponentExecutor.ExecuteCompiled();

			Assert.NotNull(output);
			Assert.AreEqual(1, output.Value.Count);
			Assert.AreEqual(18, output.Value[0].value.sum);
			Assert.AreEqual(3, output.Value[0].value.count);
		}

		[Test]
		public void TestSort()
		{
			fixtureDbReduceMongoDb.OutputType = restaurantType;
			fixtureDbReduceMongoDb.LoopResults = false;
			fixtureDbReduceMongoDb.Sort = "{\"restaurant_id\":1}";
			fixtureDbReduceMongoDb.Map = mapFunction;
			fixtureDbReduceMongoDb.Reduce = reduceFunction;
			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.AreEqual(2, output.Value.Count);
			Assert.AreEqual(18, output.Value[0].value.sum);
			Assert.AreEqual(3, output.Value[0].value.count);
		}

		[Test]
		public void TestLimit()
		{
			fixtureDbReduceMongoDb.OutputType = restaurantType;
			fixtureDbReduceMongoDb.LoopResults = false;
			fixtureDbReduceMongoDb.Limit = 1;
			fixtureDbReduceMongoDb.Map = mapFunction;
			fixtureDbReduceMongoDb.Reduce = reduceFunction;

			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.AreEqual(1, output.Value.Count);
			Assert.AreEqual(18, output.Value[0].value.sum);
			Assert.AreEqual(3, output.Value[0].value.count);
		}
	}

	internal class MongoDBMapReduceExecutor
	{
		public string ConnectionString;
		public string Collection;
		public string Query;
		public TypeReference OutputType;
		public bool LoopResults;
		public string Sort;
		public int Limit;
		public string Map;
		public string Reduce;
		public string Finalize;
		public MongoDBMapReduceShared.ReturnModeType ReturnModeType;

		internal FunctionResult ExecuteCompiled(params object[] parameters)
		{
			FunctionTester<MongoDBMapReduce> functionTester =
				new FunctionTester<MongoDBMapReduce>();
			functionTester.CustomTypes.Add(OutputType);

			return functionTester.Compile(
					new PropertyValue(MongoDBMapReduceShared.Names.ConnectionString, ConnectionString),
					new PropertyValue(MongoDBMapReduceShared.Names.Collection, Collection),
					new PropertyValue(MongoDBMapReduceShared.Names.Query, Query),
					new PropertyValue(MongoDBMapReduceShared.Names.OutputType, OutputType),
					new PropertyValue(MongoDBMapReduceShared.Names.Sort, Sort),
					new PropertyValue(MongoDBMapReduceShared.Names.Limit, Limit),
					new PropertyValue(MongoDBMapReduceShared.Names.Map, Map),
					new PropertyValue(MongoDBMapReduceShared.Names.Reduce, Reduce),
					new PropertyValue(MongoDBMapReduceShared.Names.Finalize, Finalize),
					new PropertyValue(MongoDBMapReduceShared.Names.ReturnOptionsPropertyName, ReturnModeType)
					).Execute(
						new ParameterValue[]
						{
							new ParameterValue(MongoDBMapReduceShared.Names.ConnectionString,ConnectionString),
							new ParameterValue(MongoDBMapReduceShared.Names.Collection,Collection),
							new ParameterValue(MongoDBMapReduceShared.Names.Query,Query),
							new ParameterValue(MongoDBMapReduceShared.Names.Sort,Sort),
							new ParameterValue(MongoDBMapReduceShared.Names.Limit,Limit),
							new ParameterValue(MongoDBMapReduceShared.Names.Map,Map),
							new ParameterValue(MongoDBMapReduceShared.Names.Reduce,Reduce),
							new ParameterValue(MongoDBMapReduceShared.Names.Finalize,Finalize)
						}.Concat(parameters.Where(v => v is ParameterValue).Select(v => v as ParameterValue)).ToArray()
					);
		}
	}
}