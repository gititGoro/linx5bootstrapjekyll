using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NUnit.Framework;
using Twenty57.Linx.Components.MongoDB;
using Twenty57.Linx.Plugin.TestKit;


namespace Twenty57.Linx.Components.Database.Tests
{
	[TestFixture]
	public class TestMongoDBWrite
	{
		private const string inputName = "input";
		private MongoCollection<BsonDocument> mongoCollection;
		private MongoDBWriteExecutor fixtureMongoWrite;

		[SetUp]
		public void SetUp()
		{
			fixtureMongoWrite = new MongoDBWriteExecutor();
			fixtureMongoWrite.Name = "test";
			fixtureMongoWrite.ConnectionString = "mongodb://localhost:27017/testdb";
			fixtureMongoWrite.Collection = "TestCollection";
			fixtureMongoWrite.UpdateOperation = "{}";

			var connectionString = "mongodb://localhost:27017/testdb";
			var databaseName = MongoUrl.Create(connectionString).DatabaseName;

			var clientSettings = new MongoClient(connectionString).Settings;
			MongoServerSettings serverSettings = MongoServerSettings.FromClientSettings(clientSettings);
			var server = new MongoServer(serverSettings);

			var database = server.GetDatabase(databaseName);
			mongoCollection = database.GetCollection<BsonDocument>("TestCollection");
			mongoCollection.RemoveAll();
			mongoCollection.Insert(new BsonDocument {
				{ "name", "Harry Potter" },
				{"gender", "male"},
				{ "age", 8},
			});
			mongoCollection.Insert(new BsonDocument {
				{ "name", "Nancy Drew" },
				{"gender", "female"},
				{ "age", 14},
			});
			mongoCollection.Insert(new BsonDocument {
				{ "name", "Adrian Mole" },
				{"gender", "male"},
				{ "age", 13.75},
			});
			mongoCollection.Insert(new BsonDocument {
				{ "name", "Cookie Monster" },
				{"gender", "male"},
				{ "age", 40 },
			});
		}

		[Test]
		public void TestDeleteSingle()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Delete;
			fixtureMongoWrite.Criteria = @"{ name: ""Cookie Monster""}";

			fixtureMongoWrite.ExecuteCompiled();
			var results = mongoCollection.Find(Query.EQ("name", "Cookie Monster"));
			Assert.AreEqual(0, results.Count());
			Assert.AreEqual(3, mongoCollection.FindAll().Count());
		}

		[Test]
		public void TestDeleteMultiple()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Delete;
			fixtureMongoWrite.Criteria = @"{ gender: ""male""}";

			fixtureMongoWrite.ExecuteCompiled();
			var results = mongoCollection.Find(Query.EQ("gender", "male"));
			Assert.AreEqual(0, results.Count());
			Assert.AreEqual(1, mongoCollection.FindAll().Count());
		}

		[Test]
		public void TestDeleteMultipleWithOperator()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Delete;
			fixtureMongoWrite.Criteria = @"{ age: {$lt: 20}}";

			fixtureMongoWrite.ExecuteCompiled();
			var results = mongoCollection.FindAll();
			Assert.AreEqual(1, results.Count());
			Assert.AreEqual("Cookie Monster", results.First().GetValue("name").AsString);
		}

		[Test]
		public void TestDeleteWithoutCriteria()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Delete;
			fixtureMongoWrite.Criteria = @"";

			Assert.That(() => fixtureMongoWrite.ExecuteCompiled(), Throws.Exception.TypeOf<ExecuteException>());
		}

		public void TestDeleteAll()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.DeleteAll;
			fixtureMongoWrite.Criteria = @"";

			fixtureMongoWrite.ExecuteCompiled();
			Assert.AreEqual(0, mongoCollection.FindAll().Count());
		}

		[Test]
		public void TestInsertJson()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Insert;
			fixtureMongoWrite.Data = @"{ name: ""Peter Rabbit"", age:2}";
			fixtureMongoWrite.ExecuteCompiled();
			Assert.AreEqual(1, mongoCollection.Find(
					Query.And(
							Query.EQ("name", "Peter Rabbit"),
							Query.EQ("age", 2)
							)).Count()
				 );
		}

		[Test]
		public void TestInsertObject()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Insert;
			fixtureMongoWrite.Data = new Person
			{
				name = "Peter Rabbit",
				age = 4
			};
			fixtureMongoWrite.ExecuteCompiled();
			Assert.AreEqual(1, mongoCollection.Find(
					Query.And(
							Query.EQ("name", "Peter Rabbit"),
							Query.EQ("age", 4)
							)).Count()
				 );
		}

		[Test]
		public void TestUpdate()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Update;
			fixtureMongoWrite.Criteria = @"{ name: ""Harry Potter""}";
			fixtureMongoWrite.UpdateOperation = @"{ $set: {gender: ""wizard""}}";

			fixtureMongoWrite.ExecuteCompiled();
			var results = mongoCollection.Find(Query.EQ("name", "Harry Potter"));
			Assert.AreEqual(1, results.Count());
			Assert.AreEqual("wizard", results.First().GetValue("gender").AsString);
		}

		[Test]
		public void TestUpdateExpression()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Update;
			fixtureMongoWrite.Criteria = @"{ name: ""Harry Potter""}";
			fixtureMongoWrite.UpdateOperation = @"{ $set: {gender: ""@{""evil ""+""wizard""}""}}";
			fixtureMongoWrite.ExecuteCompiled(new ParameterValue(MongoDBWriteShared.Names.UpdateOperationExpressions + "0", "evil wizard"));
			var results = mongoCollection.Find(Query.EQ("name", "Harry Potter"));
			Assert.AreEqual(1, results.Count());
			Assert.AreEqual("evil wizard", results.First().GetValue("gender").AsString);
		}

		[Test]
		public void TestCriteriaExpression()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Update;
			fixtureMongoWrite.Criteria = @"{ name: ""@{""Harry ""+ ""Potter""}""}";
			fixtureMongoWrite.UpdateOperation = @"{ $set: {gender: ""wizard""}}";

			fixtureMongoWrite.ExecuteCompiled(new ParameterValue(MongoDBWriteShared.Names.CriteriaExpressions + "0", "Harry Potter"));
			var results = mongoCollection.Find(Query.EQ("name", "Harry Potter"));
			Assert.AreEqual(1, results.Count());
			Assert.AreEqual("wizard", results.First().GetValue("gender").AsString);
		}

		[Test]
		public void TestUpdateMultiple()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Update;
			fixtureMongoWrite.Criteria = @"{ gender: ""male"" }";
			fixtureMongoWrite.UpdateOperation = @"{ $set: {gender: ""wizard""}}";

			fixtureMongoWrite.ExecuteCompiled();

			Assert.AreEqual(0, mongoCollection.Find(Query.EQ("gender", "male")).Count());
			Assert.AreEqual(3, mongoCollection.Find(Query.EQ("gender", "wizard")).Count());
			Assert.AreEqual(1, mongoCollection.Find(Query.EQ("gender", "female")).Count());
		}

		[Test]
		public void TestUpdateInsertMatch()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Update;
			fixtureMongoWrite.InsertIfNotFound = true;
			fixtureMongoWrite.Criteria = @"{ name: ""Harry Potter""}";
			fixtureMongoWrite.UpdateOperation = @"{ $set: {gender: ""wizard""}}";

			fixtureMongoWrite.ExecuteCompiled();
			var results = mongoCollection.Find(Query.EQ("name", "Harry Potter"));
			Assert.AreEqual(1, results.Count());
			Assert.AreEqual("wizard", results.First().GetValue("gender").AsString);
		}

		[Test]
		public void TestUpdateInsertNoMatch()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Update;
			fixtureMongoWrite.InsertIfNotFound = true;
			fixtureMongoWrite.Criteria = @"{ name: ""Jeremiah Puddleduck""}";
			fixtureMongoWrite.UpdateOperation = @"{ $set: {gender: ""wizard""}}";

			fixtureMongoWrite.ExecuteCompiled();
			var results = mongoCollection.Find(Query.EQ("name", "Jeremiah Puddleduck"));
			Assert.AreEqual(1, results.Count());
			Assert.AreEqual("wizard", results.First().GetValue("gender").AsString);
			Assert.AreEqual(5, mongoCollection.FindAll().Count());
		}

		[Test]
		public void TestReplaceWithMatch()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Replace;
			fixtureMongoWrite.Criteria = @"{ name: ""Harry Potter""}";
			fixtureMongoWrite.Data = new Person
			{
				name = "Hermione Granger",
				age = 15
			};
			fixtureMongoWrite.InsertIfNotFound = false;
			fixtureMongoWrite.ExecuteCompiled();

			Assert.AreEqual(0, mongoCollection.Find(Query.EQ("name", "Harry Potter")).Count());
			Assert.AreEqual(1, mongoCollection.Find(
					Query.And(
					Query.EQ("name", "Hermione Granger"),
					Query.EQ("age", 15)
							)).Count());
		}
		[Test]
		public void TestReplaceNoMatch()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Replace;
			fixtureMongoWrite.Criteria = @"{ name: ""Nobody Matches""}";
			fixtureMongoWrite.Data = new Person
			{
				name = "Hermione Granger",
				age = 15
			};
			fixtureMongoWrite.InsertIfNotFound = false;
			fixtureMongoWrite.ExecuteCompiled();

			Assert.AreEqual(4, mongoCollection.FindAll().Count());
			Assert.AreEqual(0, mongoCollection.Find(
				Query.And(
				Query.EQ("name", "Hermione Granger"),
				Query.EQ("age", 15)
					)).Count());
		}
		[Test]
		public void TestReplaceInsertWithMatch()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Replace;
			fixtureMongoWrite.Criteria = "{ name: \"Harry Potter\" }";

			fixtureMongoWrite.Data =
			new Person
			{
				name = "Hermione Granger",
				age = 15
			};
			fixtureMongoWrite.InsertIfNotFound = true;

			fixtureMongoWrite.ExecuteCompiled();
			Assert.AreEqual(0, mongoCollection.Find(Query.EQ("name", "Harry Potter")).Count());
			Assert.AreEqual(1, mongoCollection.Find(
					Query.And(
					Query.EQ("name", "Hermione Granger"),
					Query.EQ("age", 15)
							)).Count());
		}

		[Test]
		public void TestReplaceInsertWithOutMatch()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Replace;
			fixtureMongoWrite.Criteria = @"{ name: ""Nonexistant name""}";
			fixtureMongoWrite.Data = new Person
			{
				name = "Hermione Granger",
				age = 15
			};
			fixtureMongoWrite.InsertIfNotFound = true;
			fixtureMongoWrite.ExecuteCompiled();
			Assert.AreEqual(1, mongoCollection.Find(Query.EQ("name", "Harry Potter")).Count());
			Assert.AreEqual(1, mongoCollection.Find(
					Query.And(
					Query.EQ("name", "Hermione Granger"),
					Query.EQ("age", 15)
							)).Count());
		}

		[Test]
		public void TestReplaceWithoutCriteria()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Replace;
			fixtureMongoWrite.Criteria = @"";
			var targetId = mongoCollection.Find(Query.EQ("name", "Harry Potter")).First().GetValue("_id").ToString();

			fixtureMongoWrite.Data =
			new Person
			{
				id = targetId,
				name = "Hermione Granger",
				age = 15
			};

			fixtureMongoWrite.ExecuteCompiled();
			Assert.AreEqual(0, mongoCollection.Find(Query.EQ("name", "Harry Potter")).Count());
			Assert.AreEqual(1, mongoCollection.Find(
					Query.And(
					Query.EQ("name", "Hermione Granger"),
					Query.EQ("age", 15)
							)).Count());
		}

		[Test]
		public void TestReplaceWithoutCriteriaNoId()
		{
			fixtureMongoWrite.Operation = MongoDBWriteOperation.Replace;
			fixtureMongoWrite.Criteria = @"";
			fixtureMongoWrite.InsertIfNotFound = true;

			fixtureMongoWrite.Data = new Person
			{
				name = "Hermione Granger",
				age = 15
			};

			Assert.That(() => fixtureMongoWrite.ExecuteCompiled(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Cannot perform mongo replace: no criteria specified and the object's id field is not set\r\nSee Code and Parameter properties for more information."));
		}

		[TestCase(MongoDBWriteOperation.DeleteAll)]
		[TestCase(MongoDBWriteOperation.Delete)]
		[TestCase(MongoDBWriteOperation.Replace)]
		[TestCase(MongoDBWriteOperation.Update)]
		[TestCase(MongoDBWriteOperation.Insert)]
		public void TestNoDatabase(MongoDBWriteOperation operation)
		{
			fixtureMongoWrite.Operation = operation;
			fixtureMongoWrite.Data = "{}";
			fixtureMongoWrite.ConnectionString = "mongodb://localhost:27017/";

			Assert.That(() => fixtureMongoWrite.ExecuteCompiled(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Database name not specified in connection string\r\nSee Code and Parameter properties for more information."));
		}


		[TestCase(MongoDBWriteOperation.Delete)]
		[TestCase(MongoDBWriteOperation.Replace)]
		[TestCase(MongoDBWriteOperation.Update)]
		public void TestInvalidCriteria(MongoDBWriteOperation operation)
		{
			fixtureMongoWrite.Operation = operation;
			fixtureMongoWrite.Data = "{}";
			fixtureMongoWrite.Criteria = "This in invalid JSON.";

			Assert.That(() => fixtureMongoWrite.ExecuteCompiled(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Invalid criteria\r\nSee Code and Parameter properties for more information."));
		}
	}

	public class MongoDBWriteExecutor
	{
		public string Name = "";
		public string UpdateOperation = "";
		public string Collection = "";
		public string ConnectionString = "";
		public MongoDBWriteOperation Operation = MongoDBWriteOperation.Delete;
		public string Criteria = "";
		public object Data = "";
		public bool InsertIfNotFound;

		internal FunctionResult ExecuteCompiled(params object[] parameters)
		{
			return
				new FunctionTester<MongoDBWrite>().Compile(
					new PropertyValue(MongoDBWriteShared.Names.Operation, Operation),
					new PropertyValue(MongoDBWriteShared.Names.InsertIfNotFound, InsertIfNotFound),
					new PropertyValue(MongoDBWriteShared.Names.Criteria, Criteria),
					new PropertyValue(MongoDBWriteShared.Names.UpdateOperation, UpdateOperation),
					new PropertyValue(MongoDBWriteShared.Names.Data, Data))
					.Execute(
						(new ParameterValue[]
						{
							new ParameterValue(MongoDBWriteShared.Names.ConnectionString, ConnectionString),
							new ParameterValue(MongoDBWriteShared.Names.Collection, Collection),
							new ParameterValue(MongoDBWriteShared.Names.Data, Data),
							new ParameterValue(MongoDBWriteShared.Names.Criteria, Criteria),
							new ParameterValue(MongoDBWriteShared.Names.UpdateOperation, UpdateOperation)
						}
							).Concat(parameters.Where(v => v is ParameterValue).Select(v => v as ParameterValue)).ToArray()
					);
		}
	}

	public class Person
	{
		public string name;
		public int age;
		public string gender;
		public string id;
	}

	public class PersonWithoutId
	{
		public string name;
		public int age;
		public string gender;
	}
}