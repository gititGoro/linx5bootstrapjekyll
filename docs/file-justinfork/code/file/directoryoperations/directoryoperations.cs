using System;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.File
{
	public class DirectoryOperations : FunctionProvider
	{
		public override string SearchKeywords
		{
			get { return "copy move delete disk folder exists"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new V2.DirectoryOperationsDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			if (String.IsNullOrEmpty(data.Version))
				data = data.UpdateVersion("1");

			switch (data.Version)
			{
				case "1":
					return new V1.DirectoryOperationsDesigner(data, context);
				case "2":
					return new V2.DirectoryOperationsDesigner(data, context);
				default:
					throw new Exception(string.Format("Unknown version [{0}] specified.", data.Version));
			}
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			if (String.IsNullOrEmpty(data.Version))
				data = data.UpdateVersion("1");

			switch (data.Version)
			{
				case "1":
					return new V1.DirectoryOperationsCodeGenerator(data);
				case "2":
					return new V2.DirectoryOperationsCodeGenerator(data);
				default:
					throw new Exception(string.Format("Unknown version [{0}] specified.", data.Version));
			}
		}
	}
}

