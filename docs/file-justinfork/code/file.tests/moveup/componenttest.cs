using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.File.Tests
{
	public class ComponentTest<TFunction, TIn, TOut>
		where TFunction : _FunctionProvider<TIn,TOut> 
		where TIn : FunctionInput
		where TOut: FunctionOutput
	{
		protected FunctionResult Execute(TIn input)
		{
			var tester = new FunctionTester<TFunction>().Compile(
				GetStaticParameters(input)
				);

			return tester.Execute(
				GetDynamicParameters(input)
				);
		}

		private FunctionParameter[] GetDynamicParameters(TIn input)
		{
			var parameters = new List<FunctionParameter>();
			foreach (var property in typeof(TIn).GetProperties())
			{
				if ( Attribute.IsDefined(property, typeof(LinxPropertyIgnoreAttribute)) ||
					 ! Attribute.IsDefined(property, typeof(AllowDynamicAttribute)) )
					continue;
					parameters.Add(
						new FunctionParameter(property.Name,
							property.GetValue(input))
						);
			}
			return parameters.ToArray();
		}

		private FunctionProperty[] GetStaticParameters(TIn input)
		{
			var parameters = new List<FunctionProperty>();
			foreach (var property in typeof(TIn).GetProperties())
			{
				if ( Attribute.IsDefined(property, typeof(LinxPropertyIgnoreAttribute)) ||
					 Attribute.IsDefined(property, typeof(AllowDynamicAttribute)) )
					continue;
					parameters.Add(
						new FunctionProperty(property.Name,
							property.GetValue(input))
						);
			}
			return parameters.ToArray();
		}
	}
}
