using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File
{
	public class FileOperations : FunctionProvider
	{
		public override string Name
		{
			get { return "FileOperations"; }
		}

		public override string SearchKeywords
		{
			get { return "copy move delete disk"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new FileOperationsDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new FileOperationsDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new FileOperationsCodeGenerator(data);
		}
	}
}
