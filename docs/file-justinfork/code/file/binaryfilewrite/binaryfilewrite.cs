using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File
{
	public class BinaryFileWrite : FunctionProvider
	{
		public override string Name
		{
			get { return "BinaryFileWrite"; }
		}

		public override string SearchKeywords
		{
			get { return "binary file write"; }
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new BinaryFileWriteDesigner(data, context);
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new BinaryFileWriteDesigner(context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new BinaryFileWriteCodeGenerator(data);
		}

		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return BinaryFileWriteFunctionUpdater.Instance.Update(data);
		}
	}
}