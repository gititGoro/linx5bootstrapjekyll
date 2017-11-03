using System.IO;
using NUnit.Framework;
using Twenty57.Linx.Plugin.TestKit;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File.Tests
{
	[TestFixture]
	public class TestBinaryFileRead
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void TestExecuteCompiled()
		{
			byte[] data = { 1, 2, 3, 4, 5 };
			string fileName = WriteBytesToFile(data);

			dynamic output = Execute(fileName);
			Assert.AreEqual(data, output.Value);
		}


		[Test]
		public void TestExecuteWithInvalidFile()
		{
			string fileName = @"G:\No\Such\File.txt";
			Assert.That(() => Execute(fileName),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("File [G:\\No\\Such\\File.txt] does not exist.\r\nSee Code and Parameter properties for more information."));
		}

		private dynamic Execute(string fileName)
		{
			FunctionExecutor tester = new FunctionTester<BinaryFileRead>().Compile(
				);

			var result = tester.Execute(
				new ParameterValue(FileShared.FilePathPropertyName, fileName)
				);
			Assert.IsFalse(FileHelpers.IsFileLocked(fileName));
			return result;
		}

		private static string WriteBytesToFile(byte[] data)
		{
			string fileName = Path.GetTempFileName();
			System.IO.File.Delete(fileName);
			System.IO.File.WriteAllBytes(fileName, data);
			return fileName;
		}
	}
}