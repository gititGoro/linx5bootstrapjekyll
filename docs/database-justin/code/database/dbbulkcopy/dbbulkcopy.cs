using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.DbBulkCopy
{
	public class DbBulkCopy : FunctionProvider
	{
		public override string SearchKeywords
		{
			get { return "database bulk load"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new DbBulkCopyDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new DbBulkCopyDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new DbBulkCopyCodeGenerator(data);
		}

		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return FunctionUpdater.Instance.Update(data);
		}
	}
}
