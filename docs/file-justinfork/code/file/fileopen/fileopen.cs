using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File.FileOpen
{
	public class FileOpen : FunctionProvider
	{
		public override string SearchKeywords
		{
			get { return "open file"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new FileOpenDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new FileOpenDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new FileOpenCodeGenerator(data);
		}

		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return FunctionUpdater.Instance.Update(data);
		}
	}
}
