using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File
{
	public class BinaryFileRead : FunctionProvider
	{
		public override string Name
		{
			get { return "BinaryFileRead"; }
		}

		public override string SearchKeywords
		{
			get { return "binary file read"; }
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new BinaryFileReadDesigner(data, context);
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new BinaryFileReadDesigner(context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new BinaryFileReadCodeGenerator(data);
		}

		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return BinaryFileReadFunctionUpdater.Instance.Update(data);
		}
	}
}