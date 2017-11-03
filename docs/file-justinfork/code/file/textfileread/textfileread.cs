using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File.TextFileRead
{
	public class TextFileRead : FunctionProvider
	{
		public override string Name
		{
			get { return "TextFileRead"; }
		}

		public override string SearchKeywords
		{
			get { return "read text file"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new TextFileReadDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new TextFileReadDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new TextFileReadCodeGenerator(data);
		}
	}
}