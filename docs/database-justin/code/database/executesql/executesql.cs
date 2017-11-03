﻿using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.ExecuteSQL
{
	public class ExecuteSQL : FunctionProvider
	{
		public override string Name
		{
			get { return "ExecuteSQL"; }
		}

		public override string SearchKeywords
		{
			get { return "sql statement database stored procedure"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new ExecuteSQLDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new ExecuteSQLDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new ExecuteSQLCodeGenerator(data);
		}

		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return FunctionUpdater.Instance.Update(data);
		}
	}
}
