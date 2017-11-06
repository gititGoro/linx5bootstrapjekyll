using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Twenty57.Linx.Components.File.Tests.MoveUp
{
	public class TestInput : FunctionInput
	{
		public TestInput()
		{
			DynamicProperties=new DynamicProperties(new [] { "dynamic1", "dynamic2"});
		}
		
	}
	[TestFixture]
	class TestComponentIO
	{

		[Test]
		public void Test()
		{
			var input = new TestInput();
			dynamic values = input.DynamicProperties;
			values.dynamic1 = "tree";
		}
	

	}
}
