using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Components.File.Tests.Helpers;
using Twenty57.Linx.Components.File.TextFileRead;
using Twenty57.Linx.Plugin.Common.CodeGeneration;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.File.Tests
{
	[TestFixture]
	public class TestTextFileRead
	{
		private string path;
		private string file;

		[OneTimeSetUp]
		public void SetupFixture()
		{
			path = Path.Combine(Path.GetTempPath(), "TestFileRead");
			file = Path.Combine(path, "Textfile.txt");
		}

		[OneTimeTearDown]
		public void TeardownFixture()
		{
			ForceDeleteDirectory(path);
		}

		[TestCase(0, 0)]
		[TestCase(1, 1)]
		public void TestReadComplete(int skipHeaderLines, int skipFooterLines)
		{
			dynamic output = Execute(WriteResourceToFile("TextFile.txt"), FileReadOptions.Complete, new TextFileReaderFields(), skipHeaderLines, skipFooterLines).Value;
			string expected = new[] { "Header", "Text", "On", "Every", "Line", "Footer" }
				.Aggregate((current, next) => current + Environment.NewLine + next);
			Assert.AreEqual(expected, output);
		}

		[TestCase("\r")]
		[TestCase("\r\n")]
		[TestCase("\n")]
		public void TestLineDelimiter(string delimiter)
		{
			var fileContents = new[] { "Line1", "Line2", "Line3" };

			string fileName = WriteTextToFile(fileContents.Aggregate((sum, next) => sum + delimiter + next));
			dynamic output = Execute(fileName, FileReadOptions.ListOfLines);
			CollectionAssert.AreEqual(fileContents, (IEnumerable<string>)output.Value);
		}

		[TestCase(0, 0)]
		[TestCase(2, 0)]
		[TestCase(0, 2)]
		[TestCase(2, 2)]
		[TestCase(3, 3)]
		[TestCase(100, 100)]
		public void TestLineByLineReadSkippingHeaderAndFooter(int headerLineCount, int footerLineCount)
		{
			var fileContents = new[] { "Header", "Text", "On", "Every", "Line", "Footer" };
			string fileName = WriteTextToFile(fileContents.Aggregate((sum, next) => sum + "\n" + next));

			IEnumerable<string> expectedContent = fileContents.Reverse().Skip(footerLineCount).Reverse().Skip(headerLineCount);
			IEnumerable<int> expectedNumbers =
				Enumerable.Range(1, fileContents.Length).Reverse().Skip(footerLineCount).Reverse().Skip(headerLineCount);

			FunctionResult output = Execute(fileName, FileReadOptions.LineByLine, new TextFileReaderFields(), headerLineCount, footerLineCount);
			AssertFileLocking(fileName, output.ExecutionPathResult);
			CollectionAssert.AreEqual(expectedContent, GetExecutionPathLineContents(output.ExecutionPathResult));
			CollectionAssert.AreEqual(expectedNumbers, GetExecutionPathLineNumbers(output.ExecutionPathResult));
		}


		[Test]
		public void TestExecuteWithNonDefaultCodePage()
		{
			string fileText = String.Format("Line 1{0}Line 2{0}Line 3", Environment.NewLine);
			string fileName = WriteTextToFile(fileText, Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.EBCDICCodePage));
			dynamic output = Execute(fileName, FileReadOptions.Complete, new TextFileReaderFields(), 0, 0, TextCodepage.EBCDIC);
			Assert.AreEqual(fileText, output.Value);
		}

		[Test]
		public void TestExecuteWithReadCompleteAndBlankLines()
		{
			dynamic output = Execute(WriteResourceToFile("BlankLines.txt"), FileReadOptions.Complete);
			Assert.AreEqual(
				String.Format(@"Header{0}      {0}3{0}      {0}5{0}      {0}7{0}      {0}Footer", Environment.NewLine),
				output.Value);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void TestBlankLinesWhenLoopingIs(bool loopResults)
		{
			var expectedLines = new[] { "Header", "3", "5", "7", "Footer" };
			var expectedLineNumbers = new[] { 1, 3, 5, 7, 9 };

			FunctionResult output = Execute(WriteResourceToFile("BlankLines.txt"), loopResults ? FileReadOptions.LineByLine : FileReadOptions.ListOfLines);
			if (loopResults)
			{
				CollectionAssert.AreEqual(expectedLines, GetExecutionPathLineContents(output.ExecutionPathResult));
				CollectionAssert.AreEqual(expectedLineNumbers, GetExecutionPathLineNumbers(output.ExecutionPathResult));
			}
			else
			{
				CollectionAssert.AreEqual(expectedLines, output.Value);
			}
		}


		[Test]
		public void TestFieldsAsStringsFieldsMismatch()
		{
			string fileName = WriteTextToFile(String.Format("a,b,c{0}d,e,f{0}g,h,i", "\r\n"));
			var fields = new TextFileReaderFields();
			fields.Delimiter = DelimiterType.Comma;
			fields.TextQualifier = TextQualifierType.None;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Field1" });
			fieldCollection.Add(new Afield { Name = "Field2" });
			fieldCollection.Add(new Afield { Name = "Field3" });
			fieldCollection.Add(new Afield { Name = "Field4" });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(3, lines.Count);
			Assert.AreEqual("a", lines[0].Value.LineContents.Field1);
			Assert.AreEqual("b", lines[0].Value.LineContents.Field2);
			Assert.AreEqual("c", lines[0].Value.LineContents.Field3);
			Assert.AreEqual("", lines[0].Value.LineContents.Field4);
			Assert.AreEqual("d", lines[1].Value.LineContents.Field1);
			Assert.AreEqual("e", lines[1].Value.LineContents.Field2);
			Assert.AreEqual("f", lines[1].Value.LineContents.Field3);
			Assert.AreEqual("", lines[1].Value.LineContents.Field4);
			Assert.AreEqual("g", lines[2].Value.LineContents.Field1);
			Assert.AreEqual("h", lines[2].Value.LineContents.Field2);
			Assert.AreEqual("i", lines[2].Value.LineContents.Field3);
			Assert.AreEqual("", lines[2].Value.LineContents.Field4);
		}

		[Test]
		public void TestFieldsAsStrings()
		{
			try
			{
				FieldsAsStringsTester("",
					TextQualifierType.None,
					new[]
					{
						Tuple.Create("a", "b", "c"),
						Tuple.Create("d", "e", "f"),
						Tuple.Create("g", "h", string.Empty)
					},
					DelimiterType.Tab);
			}
			catch (Exception e)
			{
				Console.WriteLine((e.Message));
			}
		}

		[Test]
		public void TestFieldsAsStringsAndLoop()
		{
			FieldsAsStringsTester("",
				TextQualifierType.None,
				new[]
				{
					Tuple.Create("a", "b", "c"),
					Tuple.Create("d", "e", "f"),
					Tuple.Create("g", "h", string.Empty)
				},
				DelimiterType.Tab,
				"",
				true);
		}

		[Test]
		public void TestFieldsAsStringsDelimiterTab()
		{
			FieldsAsStringsTester("",
				TextQualifierType.None,
				new[]
				{
					Tuple.Create("a", "b", "c"),
					Tuple.Create("d", "e", "f"),
					Tuple.Create("g", "h", string.Empty)
				},
				DelimiterType.Tab);
		}


		[Test]
		public void TestFieldsAsStringsTextQualifierDoubleQuotesNoTextQualifiers()
		{
			FieldsAsStringsTester("",
				TextQualifierType.DoubleQuotes,
				new[]
				{
					Tuple.Create("a", "b", "c"),
					Tuple.Create("d", "e", "f"),
					Tuple.Create("g", "h", string.Empty)
				},
				DelimiterType.Comma);
		}

		[Test]
		public void TestFieldsAsStringsTextQualifierDoubleQuotes()
		{
			FieldsAsStringsTester("\"",
				TextQualifierType.DoubleQuotes,
				new[]
				{
					Tuple.Create("a", "b", "c"),
					Tuple.Create("d", "e", "f"),
					Tuple.Create("g", "h", string.Empty)
				},
				DelimiterType.Comma);
		}

		[Test]
		public void TestFieldsAsStringsTextQualifierSingleQuote()
		{
			FieldsAsStringsTester("\'",
				TextQualifierType.SingleQuotes,
				new[]
				{
					Tuple.Create("a", "b", "c"),
					Tuple.Create("d", "e", "f"),
					Tuple.Create("g", "h", string.Empty)
				},
				DelimiterType.Comma);
		}

		[Test]
		public void TestFieldsAsStringsTextQualifierDoubleQuotesButSupplySingle()
		{
			FieldsAsStringsTester("\'",
				TextQualifierType.DoubleQuotes,
				new[]
				{
					Tuple.Create("'a'", "b", "c"),
					Tuple.Create("'d'", "e", "f"),
					Tuple.Create("'g'", "h", "''")
				},
				DelimiterType.Comma);
		}

		[Test]
		public void TestFieldsWithTypes()
		{
			string fileName =
				WriteTextToFile(
					String.Format(
						"Client100456,1312,100,145203,2002/10/21,True,0,50{0}Client100566,99.55,1500,145201,2002/10/18,true,0,55{0}Client100890,866.00,1050,145204,2002/10/21,false,0,70",
						"\r\n"));
			var fields = new TextFileReaderFields();
			fields.Delimiter = DelimiterType.Comma;
			fields.TextQualifier = TextQualifierType.None;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Client", Type = new FieldType(typeof(string)) });
			fieldCollection.Add(new Afield { Name = "Price", Type = new FieldType(typeof(decimal)) });
			fieldCollection.Add(new Afield { Name = "Quantity", Type = new FieldType(typeof(int)) });
			fieldCollection.Add(new Afield { Name = "OrderNo", Type = new FieldType(typeof(string)) });
			fieldCollection.Add(new Afield { Name = "OrderDate", Type = new FieldType(typeof(DateTime)) });
			fieldCollection.Add(new Afield { Name = "BoolVal", Type = new FieldType(typeof(bool)) });
			fieldCollection.Add(new Afield { Name = "ByteVal", Type = new FieldType(typeof(byte)) });
			fieldCollection.Add(new Afield { Name = "DoubleVal", Type = new FieldType(typeof(double)) });
			fields.FieldList = fieldCollection;
			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(3, lines.Count);
			Assert.AreEqual("Client100456", lines[0].Value.LineContents.Client);
			Assert.AreEqual(1312, lines[0].Value.LineContents.Price);
			Assert.AreEqual(100, lines[0].Value.LineContents.Quantity);
			Assert.AreEqual("145203", lines[0].Value.LineContents.OrderNo);
			Assert.AreEqual(2002, ((DateTime)lines[0].Value.LineContents.OrderDate).Year);
			Assert.AreEqual(10, ((DateTime)lines[0].Value.LineContents.OrderDate).Month);
			Assert.AreEqual(21, ((DateTime)lines[0].Value.LineContents.OrderDate).Day);
			Assert.IsTrue(lines[0].Value.LineContents.BoolVal);
			Assert.AreEqual(byte.MinValue, lines[0].Value.LineContents.ByteVal);
			Assert.AreEqual(50, lines[0].Value.LineContents.DoubleVal);
		}

		[Test]
		public void TestFixedLengthFields()
		{
			string fileName =
				WriteTextToFile(
					String.Format("100456NBTPurchase131True 050{0}100566BTXPurchase099true 055{0}100890ANLPurchase866false070", "\r\n"));
			var fields = new TextFileReaderFields();
			fields.TextFileType = FileType.FixedLength;
			fields.TextQualifier = TextQualifierType.None;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Client", Type = new FieldType(typeof(string)), Length = 6 });
			fieldCollection.Add(new Afield { Name = "Security", Type = new FieldType(typeof(string)), Length = 3 });
			fieldCollection.Add(new Afield { Name = "Transaction", Type = new FieldType(typeof(string)), Length = 8 });
			fieldCollection.Add(new Afield { Name = "Price", Type = new FieldType(typeof(double)), Length = 3 });
			fieldCollection.Add(new Afield { Name = "BoolVal", Type = new FieldType(typeof(bool)), Length = 5 });
			fieldCollection.Add(new Afield { Name = "IntVal", Type = new FieldType(typeof(int)), Length = 3 });
			fields.FieldList = fieldCollection;
			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(3, lines.Count);
			Assert.AreEqual("100456", lines[0].Value.LineContents.Client);
			Assert.AreEqual("NBT", lines[0].Value.LineContents.Security);
			Assert.AreEqual("Purchase", lines[0].Value.LineContents.Transaction);
			Assert.AreEqual(131, lines[0].Value.LineContents.Price);
			Assert.AreEqual(true, lines[0].Value.LineContents.BoolVal);
			Assert.AreEqual(50, lines[0].Value.LineContents.IntVal);
		}

		[Test]
		public void TestFixedLengthFieldsWithLessChars()
		{
			string fileName = WriteTextToFile(String.Format("100456NBTPurchase{0}100566BTXPurchase{0}100890ANLPurchase", "\r\n"));
			var fields = new TextFileReaderFields();
			fields.TextFileType = FileType.FixedLength;
			fields.TextQualifier = TextQualifierType.None;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Client", Type = new FieldType(typeof(string)), Length = 6 });
			fieldCollection.Add(new Afield { Name = "Security", Type = new FieldType(typeof(string)), Length = 3 });
			fieldCollection.Add(new Afield { Name = "Transaction", Type = new FieldType(typeof(string)), Length = 10 });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(3, lines.Count);
			Assert.AreEqual("100456", lines[0].Value.LineContents.Client);
			Assert.AreEqual("NBT", lines[0].Value.LineContents.Security);
			Assert.AreEqual("Purchase", lines[0].Value.LineContents.Transaction);
		}

		[Test]
		public void TestFixedLengthFieldsTextQualifier()
		{
			string fileName =
				WriteTextToFile(
					String.Format("100456NBT'Purchase'131True 050{0}100566BTX'Purchase'099true 055{0}100890ANL'Purchase'866false070",
						"\r\n"));
			var fields = new TextFileReaderFields();
			fields.TextFileType = FileType.FixedLength;
			fields.TextQualifier = TextQualifierType.SingleQuotes;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Client", Type = new FieldType(typeof(string)), Length = 6 });
			fieldCollection.Add(new Afield { Name = "Security", Type = new FieldType(typeof(string)), Length = 3 });
			fieldCollection.Add(new Afield { Name = "Transaction", Type = new FieldType(typeof(string)), Length = 10 });
			fieldCollection.Add(new Afield { Name = "Price", Type = new FieldType(typeof(double)), Length = 3 });
			fieldCollection.Add(new Afield { Name = "BoolVal", Type = new FieldType(typeof(bool)), Length = 5 });
			fieldCollection.Add(new Afield { Name = "IntVal", Type = new FieldType(typeof(int)), Length = 3 });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(3, lines.Count);
			Assert.AreEqual("100456", lines[0].Value.LineContents.Client);
			Assert.AreEqual("NBT", lines[0].Value.LineContents.Security);
			Assert.AreEqual("Purchase", lines[0].Value.LineContents.Transaction);
			Assert.AreEqual(131, lines[0].Value.LineContents.Price);
			Assert.AreEqual(true, lines[0].Value.LineContents.BoolVal);
			Assert.AreEqual(50, lines[0].Value.LineContents.IntVal);
		}


		[Test]
		public void TestFieldsSkip()
		{
			string fileName = WriteTextToFile(String.Format("a,b,c{0}d,e,f{0}g,h,i", "\r\n"));
			var fields = new TextFileReaderFields();
			fields.Delimiter = DelimiterType.Comma;
			fields.TextQualifier = TextQualifierType.None;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Field1" });
			fieldCollection.Add(new Afield { Name = "Field2", Skip = true });
			fieldCollection.Add(new Afield { Name = "Field3" });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(3, lines.Count);
			Assert.That(() => lines[0].Value.LineContents.Field2,
				Throws.Exception.TypeOf<RuntimeBinderException>());
			Assert.AreEqual("a", lines[0].Value.LineContents.Field1);
			Assert.AreEqual("c", lines[0].Value.LineContents.Field3);
			Assert.AreEqual("d", lines[1].Value.LineContents.Field1);
			Assert.AreEqual("f", lines[1].Value.LineContents.Field3);
			Assert.AreEqual("g", lines[2].Value.LineContents.Field1);
			Assert.AreEqual("i", lines[2].Value.LineContents.Field3);
		}

		[Test]
		public void TestFieldsAsStringsSpecificCase()
		{
			string fileName = WriteTextToFile(String.Format("'abc'|'123'|'01-12-2013'{0}def|' 456'|' 25-12-2013'", "\r\n"));
			var fields = new TextFileReaderFields();
			fields.Delimiter = DelimiterType.Other;
			fields.OtherDelimiter = "|";
			fields.TextQualifier = TextQualifierType.SingleQuotes;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Field1" });
			fieldCollection.Add(new Afield { Name = "Field2" });
			fieldCollection.Add(new Afield { Name = "Field3" });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(2, lines.Count);
			Assert.AreEqual("abc", lines[0].Value.LineContents.Field1);
			Assert.AreEqual("123", lines[0].Value.LineContents.Field2);
			Assert.AreEqual("01-12-2013", lines[0].Value.LineContents.Field3);
			Assert.AreEqual("def", lines[1].Value.LineContents.Field1);
			Assert.AreEqual(" 456", lines[1].Value.LineContents.Field2);
			Assert.AreEqual(" 25-12-2013", lines[1].Value.LineContents.Field3);
		}

		[Test]
		public void TestFieldFormat()
		{
			string fileName =
				WriteTextToFile(
					String.Format(
						"Client100456,1312,100,145203,10 21-2002,True,0,50.0{0}Client100566,99.55,1500,145201,10 18-2002,true,0,55{0}Client100890,866.00,1050,145204,10 21-2002,false,0,70",
						"\r\n"));
			var fields = new TextFileReaderFields();
			fields.Delimiter = DelimiterType.Comma;
			fields.TextQualifier = TextQualifierType.None;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Client", Type = new FieldType(typeof(string)) });
			fieldCollection.Add(new Afield { Name = "Price", Type = new FieldType(typeof(decimal)) });
			fieldCollection.Add(new Afield { Name = "Quantity", Type = new FieldType(typeof(int)) });
			fieldCollection.Add(new Afield { Name = "OrderNo", Type = new FieldType(typeof(string)) });
			fieldCollection.Add(new Afield { Name = "OrderDate", Type = new FieldType(typeof(DateTime)), Format = "MM dd-yyyy" });
			fieldCollection.Add(new Afield { Name = "BoolVal", Type = new FieldType(typeof(bool)) });
			fieldCollection.Add(new Afield { Name = "ByteVal", Type = new FieldType(typeof(byte)) });
			fieldCollection.Add(new Afield { Name = "DoubleVal", Type = new FieldType(typeof(double)) });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			List<NextResult> lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(3, lines.Count);
			Assert.AreEqual("Client100456", lines[0].Value.LineContents.Client);
			Assert.AreEqual(1312, lines[0].Value.LineContents.Price);
			Assert.AreEqual(100, lines[0].Value.LineContents.Quantity);
			Assert.AreEqual("145203", lines[0].Value.LineContents.OrderNo);
			Assert.AreEqual(2002, ((DateTime)lines[0].Value.LineContents.OrderDate).Year);
			Assert.AreEqual(10, ((DateTime)lines[0].Value.LineContents.OrderDate).Month);
			Assert.AreEqual(21, ((DateTime)lines[0].Value.LineContents.OrderDate).Day);
			Assert.IsTrue(lines[0].Value.LineContents.BoolVal);
			Assert.AreEqual(byte.MinValue, lines[0].Value.LineContents.ByteVal);
			Assert.AreEqual(50, lines[0].Value.LineContents.DoubleVal);
		}

		[Test]
		public void TestSnifferDelimiter()
		{
			string fileName = WriteTextToFile(String.Format("a,b,c{0}d,e,f{0}g,h,i", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName);
			Assert.AreEqual(',', sniffer.Delimiter);
		}

		[Test]
		public void TestSnifferDelimiter2()
		{
			string fileName = WriteTextToFile(String.Format(";a,b,|c{0}d,e,f{0}g,|h,i", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName);
			Assert.AreEqual(',', sniffer.Delimiter);
		}

		[Test]
		public void TestSnifferTextQualifier()
		{
			string fileName = WriteTextToFile(String.Format("'a',b,'c'{0}'d',e,'f'{0}'g',h,'i'", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName);
			Assert.AreEqual(',', sniffer.Delimiter);
			Assert.AreEqual('\'', sniffer.TextQualifier);
		}

		[Test]
		public void TestSnifferTextQualifierMixed()
		{
			string fileName = WriteTextToFile(String.Format("'a',\"b,'c'{0}'d',e,'f'{0}'g',\"h\",'i'", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName);
			Assert.AreEqual(',', sniffer.Delimiter);
			Assert.AreEqual('\'', sniffer.TextQualifier);
		}

		[Test]
		public void TestSnifferTextQualifierNone()
		{
			string fileName = WriteTextToFile(String.Format("'a,\"b,'c{0}'d,e,'f{0}'g,\"h,'i", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName);
			Assert.AreEqual(',', sniffer.Delimiter);
			Assert.IsFalse(sniffer.TextQualifier.HasValue);
		}

		[Test]
		public void TestColumnHeadersNoHeader()
		{
			string fileName = WriteTextToFile(String.Format("a,b,c{0}d,e,f{0}g,h,i", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName, 0);
			string[] columnHeaders = sniffer.ColumnHeaders;
			Assert.AreEqual(3, columnHeaders.Length);
			Assert.AreEqual("Column1", columnHeaders[0]);
			Assert.AreEqual("Column2", columnHeaders[1]);
			Assert.AreEqual("Column3", columnHeaders[2]);
		}

		[Test]
		public void TestColumnHeadersWithHeader()
		{
			string fileName = WriteTextToFile(String.Format("OrderNumber, Quantity, Price{0}a,b,c{0}d,e,f{0}g,h,i", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName, 1);
			string[] columnHeaders = sniffer.ColumnHeaders;
			Assert.AreEqual(3, columnHeaders.Length);
			Assert.AreEqual("OrderNumber", columnHeaders[0]);
			Assert.AreEqual("Quantity", columnHeaders[1]);
			Assert.AreEqual("Price", columnHeaders[2]);
		}

		[Test]
		public void TestColumnHeadersDuplicate()
		{
			string fileName = WriteTextToFile(String.Format("OrderNumber, Quantity, Quantity{0}a,b,c{0}d,e,f{0}g,h,i", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName, 1);
			string[] columnHeaders = sniffer.ColumnHeaders;
			Assert.AreEqual(3, columnHeaders.Length);
			Assert.AreEqual("OrderNumber", columnHeaders[0]);
			Assert.AreEqual("Quantity", columnHeaders[1]);
			Assert.AreEqual("Quantity3", columnHeaders[2]);
		}

		[Test]
		public void TestColumnHeadersWithMultiHeader()
		{
			string fileName =
				WriteTextToFile(
					String.Format("-----------------{0}OrderNumber, Quantity, Price{0}--------------------{0}a,b,c{0}d,e,f{0}g,h,i",
						"\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName, 3);
			string[] columnHeaders = sniffer.ColumnHeaders;
			Assert.AreEqual(3, columnHeaders.Length);
			Assert.AreEqual("OrderNumber", columnHeaders[0]);
			Assert.AreEqual("Quantity", columnHeaders[1]);
			Assert.AreEqual("Price", columnHeaders[2]);
		}

		[Test]
		public void TestHeaderLinesWithoutHeader()
		{
			string fileName = WriteTextToFile(String.Format("a,b,c{0}d,e,f{0}g,h,i", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName, 0);
			Assert.AreEqual(0, sniffer.HeaderLines);
		}

		[Test]
		public void TestHeaderLinesWithHeader()
		{
			string fileName =
				WriteTextToFile(String.Format("OrderNumber, Quantity, Price{0}a,50,100{0}d,2,4{0}g,100,200", "\r\n"));
			CSVSniffer sniffer = CSVSniffer.Sniff(fileName, 0);
			Assert.AreEqual(1, sniffer.HeaderLines);
		}

		[Test]
		public void TestExecuteWithEmptyValuesInFile()
		{
			string fileName = WriteTextToFile(String.Format("a,b,c{0}d,,f{0}g,h,i", "\r\n"));
			var fields = new TextFileReaderFields();
			fields.Delimiter = DelimiterType.Comma;
			fields.TextQualifier = TextQualifierType.None;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Field1" });
			fieldCollection.Add(new Afield { Name = "Field2" });
			fieldCollection.Add(new Afield { Name = "Field3" });
			fieldCollection.Add(new Afield { Name = "Field4" });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(3, lines.Count);
			Assert.AreEqual("a", lines[0].Value.LineContents.Field1);
			Assert.AreEqual("b", lines[0].Value.LineContents.Field2);
			Assert.AreEqual("c", lines[0].Value.LineContents.Field3);
			Assert.AreEqual("d", lines[1].Value.LineContents.Field1);
			Assert.AreEqual("", lines[1].Value.LineContents.Field2);
			Assert.AreEqual("f", lines[1].Value.LineContents.Field3);
			Assert.AreEqual("g", lines[2].Value.LineContents.Field1);
			Assert.AreEqual("h", lines[2].Value.LineContents.Field2);
			Assert.AreEqual("i", lines[2].Value.LineContents.Field3);
		}

		[Test]
		public void TestTrimOutput()
		{
			string fileName =
				WriteTextToFile(String.Format("john, smith, ferrari ,sandton{0}debbie, smuts, bugatti ,rosebank", "\r\n"));
			var fields = new TextFileReaderFields();
			fields.Delimiter = DelimiterType.Comma;
			fields.TextQualifier = TextQualifierType.None;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Field1" });
			fieldCollection.Add(new Afield { Name = "Field2" });
			fieldCollection.Add(new Afield { Name = "Field3" });
			fieldCollection.Add(new Afield { Name = "Field4" });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(2, lines.Count);
			Assert.AreEqual("john", lines[0].Value.LineContents.Field1);
			Assert.AreEqual("smith", lines[0].Value.LineContents.Field2);
			Assert.AreEqual("ferrari", lines[0].Value.LineContents.Field3);
			Assert.AreEqual("sandton", lines[0].Value.LineContents.Field4);
			Assert.AreEqual("debbie", lines[1].Value.LineContents.Field1);
			Assert.AreEqual("smuts", lines[1].Value.LineContents.Field2);
			Assert.AreEqual("bugatti", lines[1].Value.LineContents.Field3);
			Assert.AreEqual("rosebank", lines[1].Value.LineContents.Field4);
		}

		[Test]
		public void TestTrimOutputExceptTextQualifier()
		{
			string fileName =
				WriteTextToFile(String.Format("john, smith,\" ferrari \",sandton{0}debbie, smuts,\" bugatti \",rosebank", "\r\n"));
			var fields = new TextFileReaderFields();
			fields.Delimiter = DelimiterType.Comma;
			fields.TextQualifier = TextQualifierType.DoubleQuotes;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Field1" });
			fieldCollection.Add(new Afield { Name = "Field2" });
			fieldCollection.Add(new Afield { Name = "Field3" });
			fieldCollection.Add(new Afield { Name = "Field4" });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			var lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(2, lines.Count);
			Assert.AreEqual("john", lines[0].Value.LineContents.Field1);
			Assert.AreEqual("smith", lines[0].Value.LineContents.Field2);
			Assert.AreEqual(" ferrari ", lines[0].Value.LineContents.Field3);
			Assert.AreEqual("sandton", lines[0].Value.LineContents.Field4);
			Assert.AreEqual("debbie", lines[1].Value.LineContents.Field1);
			Assert.AreEqual("smuts", lines[1].Value.LineContents.Field2);
			Assert.AreEqual(" bugatti ", lines[1].Value.LineContents.Field3);
			Assert.AreEqual("rosebank", lines[1].Value.LineContents.Field4);
		}

		[Test]
		public void TestEmbeddedDelimiters()
		{
			string fileName =
				WriteTextToFile(String.Format(
					"\"Wilbur Smith\",\"Go, go, go\"{0}\"Willem Wikkelspies\",\"Romeo said \"\"jump\"\"\"", "\r\n"));
			var fields = new TextFileReaderFields();
			fields.Delimiter = DelimiterType.Comma;
			fields.TextQualifier = TextQualifierType.DoubleQuotes;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Field1" });
			fieldCollection.Add(new Afield { Name = "Field2" });
			fieldCollection.Add(new Afield { Name = "Field3" });
			fieldCollection.Add(new Afield { Name = "Field4" });
			fields.FieldList = fieldCollection;

			dynamic output = Execute(fileName, FileReadOptions.LineByLine, fields);
			List<NextResult> lines = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
			Assert.AreEqual(2, lines.Count);
			Assert.AreEqual("Wilbur Smith", lines[0].Value.LineContents.Field1);
			Assert.AreEqual("Go, go, go", lines[0].Value.LineContents.Field2);
			Assert.AreEqual("Willem Wikkelspies", lines[1].Value.LineContents.Field1);
			Assert.AreEqual("Romeo said \"jump\"", lines[1].Value.LineContents.Field2);
		}

		[Test]
		public void TestExecuteWithInvalidFile()
		{

			Assert.That(() => Execute(@"G:\No\Such\File.txt"),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("File [G:\\No\\Such\\File.txt] does not exist.\r\nSee Code and Parameter properties for more information."));
		}

		protected static void ForceDeleteDirectory(string path)
		{
			if (!Directory.Exists(path))
				return;

			var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };
			foreach (FileSystemInfo info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
				info.Attributes = FileAttributes.Normal;

			directory.Delete(true);
		}

		private void AssertFileLocking(string filePath, IEnumerable<NextResult> executionPath)
		{
			Assert.IsFalse(FileHelpers.IsFileLocked(filePath));
			foreach (NextResult result in executionPath)
			{
				Assert.IsTrue(FileHelpers.IsFileLocked(filePath));
			}
			Assert.IsFalse(FileHelpers.IsFileLocked(filePath));
		}

		private List<int> GetExecutionPathLineNumbers(IEnumerable<NextResult> executionPath)
		{
			var lineNumbers = new List<int>();
			foreach (NextResult result in executionPath)
			{
				lineNumbers.Add(result.Value.LineNumber);
			}
			return lineNumbers;
		}

		private List<string> GetExecutionPathLineContents(IEnumerable<NextResult> executionPath)
		{
			var lineContents = new List<string>();
			foreach (NextResult result in executionPath)
			{
				lineContents.Add(result.Value.LineContents);
			}
			return lineContents;
		}

		private void FieldsAsStringsTester(
			string textQualifierInFile,
			TextQualifierType textQualifierForComponent,
			Tuple<string, string, string>[] expectedResults,
			DelimiterType delimiterType,
			string otherDelimiter = "",
			bool loopResults = false)
		{
			string delimiter = "";
			switch (delimiterType)
			{
				case DelimiterType.Comma:
					delimiter = ",";
					break;
				case DelimiterType.Tab:
					delimiter = "\t";
					break;
				case DelimiterType.Other:
					delimiter = otherDelimiter;
					break;
				default:
					throw new Exception("Unexpected DelimiterType " + delimiterType);
			}

			string fileName =
				WriteTextToFile(String.Format("{1}a{1}{0}b{0}c\r\n{1}d{1}{0}e{0}f\r\n{1}g{1}{0}h{0}{1}{1}", delimiter,
					textQualifierInFile));
			var fields = new TextFileReaderFields();
			fields.Delimiter = delimiterType;
			if (delimiterType == DelimiterType.Other)
				fields.OtherDelimiter = otherDelimiter;
			fields.TextQualifier = textQualifierForComponent;
			var fieldCollection = new AfieldCollection();
			fieldCollection.Add(new Afield { Name = "Field1" });
			fieldCollection.Add(new Afield { Name = "Field2" });
			fieldCollection.Add(new Afield { Name = "Field3" });
			fields.FieldList = fieldCollection;
			FunctionResult output = Execute(fileName, loopResults ? FileReadOptions.LineByLine : FileReadOptions.ListOfLines, fields);

			if (loopResults)
			{
				List<NextResult> resultList = ((IEnumerable<NextResult>)output.ExecutionPathResult).ToList();
				for (int i = 0; i < expectedResults.Length; i++)
				{
					Assert.AreEqual(expectedResults[i].Item1, resultList[i].Value.LineContents.Field1);
					Assert.AreEqual(expectedResults[i].Item2, resultList[i].Value.LineContents.Field2);
					Assert.AreEqual(expectedResults[i].Item3, resultList[i].Value.LineContents.Field3);
				}
			}
			else
			{
				dynamic results = output.Value;
				for (int i = 0; i < expectedResults.Length; i++)
				{
					Assert.AreEqual(expectedResults[i].Item1, results[i].Field1);
					Assert.AreEqual(expectedResults[i].Item2, results[i].Field2);
					Assert.AreEqual(expectedResults[i].Item3, results[i].Field3);
				}
			}
		}

		private FunctionResult Execute(string fileName, FileReadOptions readType = FileReadOptions.Complete)
		{
			return Execute(fileName, readType, new TextFileReaderFields());
		}

		private FunctionResult Execute(string fileName, FileReadOptions readType, TextFileReaderFields fields, int skipHeaderLines = 0, int skipFooterLines = 0, TextCodepage codepage = TextCodepage.Default)
		{
			FunctionExecutor tester = (new FunctionTester<TextFileRead.TextFileRead>()).Compile(
				new PropertyValue(TextFileReadShared.CodepagePropertyName, codepage),
				new PropertyValue(TextFileReadShared.ReadTypePropertyName, readType),
				new PropertyValue(TextFileReadShared.FieldsPropertyName, fields),
				new PropertyValue(TextFileReadShared.SkipHeaderLinesPropertyName, skipHeaderLines),
				new PropertyValue(TextFileReadShared.SkipFooterLinesPropertyName, skipFooterLines));
			var result = tester.Execute(new ParameterValue(FileShared.FilePathPropertyName, fileName));
			Assert.IsFalse(FileHelpers.IsFileLocked(fileName));
			return result;
		}


		private static string WriteResourceToFile(string resourceName)
		{
			string fileName = Path.Combine(Path.GetTempPath(), resourceName);
			if (System.IO.File.Exists(fileName))
				System.IO.File.Delete(fileName);
			ResourceHelpers.WriteResourceToFile($"Twenty57.Linx.Components.File.Tests.TestFiles.TestTextFileRead.{resourceName}", fileName);
			return fileName;
		}

		private static string WriteTextToFile(string text, Encoding encoding = null)
		{
			string fileName = Path.GetTempFileName();
			System.IO.File.Delete(fileName);
			System.IO.File.WriteAllText(fileName, text, encoding ?? Encoding.Default);
			return fileName;
		}
	}
}