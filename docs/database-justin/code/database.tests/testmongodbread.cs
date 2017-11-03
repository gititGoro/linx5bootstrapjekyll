using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using System.Linq;
using Twenty57.Linx.Components.Database.Mongo.MongoDBRead;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.TestKit;
using Twenty57.Linx.Plugin.Common.CodeGeneration;
using System.Collections.Generic;

namespace Twenty57.Linx.Components.MongoDB.Tests
{
	[TestFixture]
	public class TestMongoDBRead
	{
		private MongoDbReadExecutor fixtureComponentExecutor;
		private MongoDbReadExecutor fixtureDbReadMongoDb;
		private TypeReference personType;
		private TypeReference eldestPersonType;

		[SetUp]
		public void SetUp()
		{
			fixtureComponentExecutor = new MongoDbReadExecutor();
			fixtureDbReadMongoDb = fixtureComponentExecutor;
			fixtureDbReadMongoDb.ConnectionString = "mongodb://localhost:27017/testdb";
			fixtureDbReadMongoDb.Collection = "TestCollection";
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.ListOfRows;

			personType = TypeReference.CreateGeneratedType(
				new TypeProperty("name", typeof(string)),
				new TypeProperty("gender", typeof(string)),
				new TypeProperty("age", typeof(float)),
				new TypeProperty("id", typeof(string))
				);
			eldestPersonType = TypeReference.CreateGeneratedType(new TypeProperty("Eldest", typeof(string)), new TypeProperty("total", typeof(int)));
			var connectionString = "mongodb://localhost:27017/testdb";
			var databaseName = MongoUrl.Create(connectionString).DatabaseName;

			var clientSettings = new MongoClient(connectionString).Settings;
			MongoServerSettings serverSettings = MongoServerSettings.FromClientSettings(clientSettings);
			var server = new MongoServer(serverSettings);

			var database = server.GetDatabase(databaseName);
			MongoCollection<BsonDocument> test = database.GetCollection<BsonDocument>("TestCollection");
			test.RemoveAll();
			test.Insert(new BsonDocument {
				{ "name", "Harry Potter" },
				{"gender", "male"},
				{ "age", 8},
			});
			test.Insert(new BsonDocument {
				{ "name", "Nancy Drew" },
				{"gender", "female"},
				{ "age", 14},
			});
			test.Insert(new BsonDocument {
				{ "name", "Adrian Mole" },
				{"gender", "male"},
				{ "age", 13.75},
			});
			test.Insert(new BsonDocument {
				{ "name", "Cookie Monster" },
				{"gender", "male"},
				{ "age", 40 },
			});
		}

		[Test]
		public void TestReadWithListOfRowsReturnMode()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.ListOfRows;
			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.AreEqual(4, output.Value.Count);
		}

		[Test]
		public void TestReadWithRowByRowReturnMode()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.RowByRow;
			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.AreEqual(4, (output.ExecutionPathResult.Count()));
		}

		[Test]
		public void TestReadWithFirstRowReturnMode()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.FirstRow;
			dynamic output = fixtureComponentExecutor.ExecuteCompiled().Value;
			Assert.NotNull(output);
			Assert.AreEqual(8, output.age);
			Assert.AreEqual("Harry Potter", output.name);
		}

		[Test]
		public void TestReadWithFirstRowReturnModeNoRows()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.Query =
				@"
					{ age: { $gt: 100 } }
				";
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.FirstRow;
			Assert.That(() => fixtureComponentExecutor.ExecuteCompiled(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("No rows returned by query.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestReadWithFirstRowElseEmptyRowReturnModeWhenEmptyFirstRow()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.Query =
				@"
					{ age: { $gt: 100 } }
				";
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.FirstRowElseEmptyRow;
			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.IsNull(output.Value);
		}

		[Test]
		public void TestReadWithFirstRowElseEmptyRowReturnModeWhenNonEmptyFirstRow()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.FirstRowElseEmptyRow;
			dynamic output = fixtureComponentExecutor.ExecuteCompiled().Value;
			Assert.NotNull(output);
			Assert.AreEqual(8, output.age);
			Assert.AreEqual("Harry Potter", output.name);
		}

		[Test]
		public void TestReadWithCriteriaListOfRowsReturnMode()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.ListOfRows;
			fixtureDbReadMongoDb.Query =
				@"
					{ age: { $gt: 30 } }
				";
			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.AreEqual(1, output.Value.Count);
			Assert.AreEqual("Cookie Monster", output.Value[0].name);
		}

		[Test]
		public void TestExecuteWithExpressionInQuery()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.Query =
				@"
					{ age: { $gt: @{SomeIntValue} } }
				";
			var output = fixtureComponentExecutor.ExecuteCompiled(new ParameterValue(MongoDBReadShared.Names.QueryExpressions + 0, 30));
			Assert.NotNull(output);
			Assert.AreEqual(1, output.Value.Count);
			Assert.AreEqual("Cookie Monster", output.Value[0].name);
		}

		[Test]
		public void TestAggregationArray()
		{
			fixtureDbReadMongoDb.OutputType = eldestPersonType;
			fixtureDbReadMongoDb.AggregationPipeline =
				@"
					[
					{ $sort: {gender:1, age:-1} },
					{ $group: {
						_id: ""$gender"", 
						total:{$sum:1}, 
						Eldest: {$first:""$name""} 
						}},
					{ $sort: { _id: 1 } }
					]
				";
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Aggregate;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.ListOfRows;

			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.AreEqual(2, output.Value.Count);
			Assert.AreEqual("Nancy Drew", output.Value[0].Eldest);
			Assert.AreEqual(1, output.Value[0].total);
			Assert.AreEqual("Cookie Monster", output.Value[1].Eldest);
			Assert.AreEqual(3, output.Value[1].total);
		}

		[Test]
		public void TestAggregationArrayWithFirstRowReturnMode()
		{
			fixtureDbReadMongoDb.OutputType = eldestPersonType;
			fixtureDbReadMongoDb.AggregationPipeline =
				@"
					[
					{ $sort: {gender:1, age:-1} },
					{ $group: {
						_id: ""$gender"", 
						total:{$sum:1}, 
						Eldest: {$first:""$name""} 
						}},
					{ $sort: { _id: 1 } }
					]
				";
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Aggregate;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.FirstRow;

			dynamic output = fixtureComponentExecutor.ExecuteCompiled().Value;
			Assert.NotNull(output);
			Assert.AreEqual("Nancy Drew", output.Eldest);
			Assert.AreEqual(1, output.total);
		}

		[Test]
		public void TestAggregationArrayWithFirstRowReturnModeNoRows()
		{
			fixtureDbReadMongoDb.OutputType = eldestPersonType;
			fixtureDbReadMongoDb.AggregationPipeline =
				@"
					[
					{ $sort: {gender:1, age:-1} },
					{ $group: {
						_id: ""$gender"", 
						total:{$sum:1}, 
						Eldest: {$first:""$name""} 
						}},
					{ $sort: { _id: 1 } },
					{ $skip : 4 }					
					]
				";
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Aggregate;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.FirstRow;

			Assert.That(() => fixtureComponentExecutor.ExecuteCompiled(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("No rows returned by query.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestAggregationArrayWithFirstRowElseEmptyRowReturnMode()
		{
			fixtureDbReadMongoDb.OutputType = eldestPersonType;
			fixtureDbReadMongoDb.AggregationPipeline =
				@"
					[
					{ $sort: {gender:1, age:-1} },
					{ $group: {
						_id: ""$gender"", 
						total:{$sum:1}, 
						Eldest: {$first:""$name""} 
						}},
					{ $sort: { _id: 1 } }
					]
				";
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Aggregate;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.FirstRow;

			dynamic output = fixtureComponentExecutor.ExecuteCompiled().Value;
			Assert.NotNull(output);
			Assert.AreEqual("Nancy Drew", output.Eldest);
			Assert.AreEqual(1, output.total);
		}

		[Test]
		public void TestAggregationArrayWithFirstRowElseEmptyRowReturnModeEmptyRow()
		{
			fixtureDbReadMongoDb.OutputType = eldestPersonType;
			fixtureDbReadMongoDb.AggregationPipeline =
				@"
					[
					{ $sort: {gender:1, age:-1} },
					{ $group: {
						_id: ""$gender"", 
						total:{$sum:1}, 
						Eldest: {$first:""$name""} 
						}},
					{ $skip : 4 }
					]
				";
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Aggregate;
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.FirstRowElseEmptyRow;

			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.IsNull(output.Value);
		}

		[Test]
		public void TestResultsTable()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Aggregate;
			fixtureDbReadMongoDb.AggregationPipeline =
							@"
					[{ $sort: {age:-1} }]
				";
			dynamic dataOut = fixtureComponentExecutor.ExecuteCompiled();
			Assert.AreEqual(4, dataOut.Value.Count);

			dynamic row = dataOut.Value[0];
			Assert.AreEqual(40, row.age);
			Assert.AreEqual("Cookie Monster", row.name);
		}

		[Test]
		public void TestResultsTableWithSkipLimit()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Find;

			fixtureDbReadMongoDb.Skip = 1;
			fixtureDbReadMongoDb.Limit = 1;
			fixtureDbReadMongoDb.Fields = "";
			fixtureDbReadMongoDb.Sort = "{age:1}";

			dynamic dataOut = fixtureComponentExecutor.ExecuteCompiled();
			Assert.AreEqual(1, dataOut.Value.Count);

			dynamic row = dataOut.Value[0];
			Assert.AreEqual(13.75, row.age);
			Assert.AreEqual("Adrian Mole", row.name);
		}

		[Test]
		public void TestAggregationWithCriteria()
		{
			fixtureDbReadMongoDb.OutputType = eldestPersonType;
			fixtureDbReadMongoDb.AggregationPipeline =
				@"
					[
					{ $match:{ age: { $lt : 20 }}},
					{ $sort: {gender:1, age:-1} },
					{ $group: {
						_id: ""$gender"", 
						total:{$sum:1}, 
						Eldest: {$first:""$name""} 
						}},
					{ $sort: { _id: 1 } }					
					]
				";
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Aggregate;
			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.AreEqual(2, output.Value.Count);
			Assert.AreEqual("Nancy Drew", output.Value[0].Eldest);
			Assert.AreEqual(1, output.Value[0].total);
			Assert.AreEqual("Adrian Mole", output.Value[1].Eldest);
			Assert.AreEqual(2, output.Value[1].total);
		}

		[Test]
		public void TestNoDatabase()
		{
			fixtureDbReadMongoDb.OutputType = eldestPersonType;
			fixtureDbReadMongoDb.ConnectionString = "mongodb://localhost:27017/";

			Assert.That(() => fixtureComponentExecutor.ExecuteCompiled(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Database name not specified in connection string\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestInvalidCriteria()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.Query = "This in invalid JSON.";

			Assert.That(() => fixtureComponentExecutor.ExecuteCompiled(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Invalid criteria\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestInvalidPipeline()
		{
			fixtureDbReadMongoDb.OutputType = personType;
			fixtureDbReadMongoDb.AggregationPipeline = "This in invalid JSON.";
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Aggregate;

			Assert.That(() => fixtureComponentExecutor.ExecuteCompiled(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Invalid aggregation pipeline\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestFindCollectionAsString()
		{
			fixtureDbReadMongoDb.OutputType = TypeReference.Create(typeof(string));
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.ListOfRows;
			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.AreEqual(4, output.Value.Count);
		}

		[Test]
		public void TestAggregateCollectionAsString()
		{
			fixtureDbReadMongoDb.OutputType = TypeReference.Create(typeof(string));
			fixtureDbReadMongoDb.ReturnModeType = MongoDBReadShared.ReturnModeType.ListOfRows;
			fixtureDbReadMongoDb.Operation = MongoDBReadShared.OperationType.Aggregate;
			fixtureDbReadMongoDb.AggregationPipeline =
				@"
					[
					{ $sort: {gender:1, age:-1} },
					{ $group: {
						_id: ""$gender"", 
						total:{$sum:1}, 
						Eldest: {$first:""$name""} 
						}},
					{ $sort: { _id: 1 } }
					]
				";
			var output = fixtureComponentExecutor.ExecuteCompiled();
			Assert.NotNull(output);
			Assert.AreEqual(2, output.Value.Count);
		}
	}

	internal class MongoDbReadExecutor
	{
		public string ConnectionString;
		public string Collection;
		public string Query;
		public TypeReference OutputType;

		public string AggregationPipeline;
		public MongoDBReadShared.OperationType Operation;
		public int Skip;
		public string Fields;
		public string Sort;
		public int Limit;
		public MongoDBReadShared.ReturnModeType ReturnModeType;

		internal FunctionResult ExecuteCompiled(params object[] parameters)
		{
			FunctionTester<MongoDBRead> functionTester =
				new FunctionTester<MongoDBRead>();
			functionTester.CustomTypes.Add(OutputType);

			return functionTester.Compile(
					new PropertyValue(MongoDBReadShared.Names.ConnectionString, ConnectionString),
					new PropertyValue(MongoDBReadShared.Names.Collection, Collection),
					new PropertyValue(MongoDBReadShared.Names.Query, Query),
					new PropertyValue(MongoDBReadShared.Names.OutputType, OutputType),
					new PropertyValue(MongoDBReadShared.Names.AggregationPipeline, AggregationPipeline),
					new PropertyValue(MongoDBReadShared.Names.Fields, Fields),
					new PropertyValue(MongoDBReadShared.Names.Sort, Sort),
					new PropertyValue(MongoDBReadShared.Names.Skip, Skip),
					new PropertyValue(MongoDBReadShared.Names.Limit, Limit),
					new PropertyValue(MongoDBReadShared.Names.Operation, Operation),
					new PropertyValue(MongoDBReadShared.Names.ReturnOptionsPropertyName, ReturnModeType)
					).Execute(
						new ParameterValue[]
						{
							new ParameterValue(MongoDBReadShared.Names.ConnectionString,ConnectionString),
							new ParameterValue(MongoDBReadShared.Names.Collection,Collection),
							new ParameterValue(MongoDBReadShared.Names.Query,Query),
							new ParameterValue(MongoDBReadShared.Names.AggregationPipeline,AggregationPipeline),
							new ParameterValue(MongoDBReadShared.Names.Fields,Fields),
							new ParameterValue(MongoDBReadShared.Names.Sort,Sort),
							new ParameterValue(MongoDBReadShared.Names.Skip,Skip),
							new ParameterValue(MongoDBReadShared.Names.Limit,Limit)
						}.Concat(parameters.Where(v => v is ParameterValue).Select(v => v as ParameterValue)).ToArray()
					);
		}
	}
}