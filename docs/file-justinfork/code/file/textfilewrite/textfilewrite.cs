using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File.TextFileWrite
{
	public class TextFileWrite : FunctionProvider
	{
		internal readonly static FunctionUpdater functionUpdater = new FunctionUpdater();

		public override string Name
		{
			get { return "TextFileWrite"; }
		}

		public override string SearchKeywords
		{
			get { return "path directory"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new TextFileWriteDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new TextFileWriteDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new TextFileWriteCodeGenerator(data);
		}

		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return FunctionUpdater.Instance.Update(data);
		}
	}
}