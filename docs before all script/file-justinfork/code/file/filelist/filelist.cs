using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File
{
	public class FileList : FunctionProvider
	{
		public override string Name
		{
			get { return "FileList"; }
		}

		public override string SearchKeywords
		{
			get { return "path directory"; }
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new FileListDesigner(data, context);
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new FileListDesigner(context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new FileListCodeGenerator(data);
		}

		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return FileListFunctionUpdater.Instance.Update(data);
		}
	}
}