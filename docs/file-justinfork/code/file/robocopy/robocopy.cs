using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File
{
	public class Robocopy : FunctionProvider
	{
		public override string SearchKeywords
		{
			get { return "Copy mirror move files"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new RobocopyDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new RobocopyDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new RobocopyCodeGenerator(data);
		}
	}
}
