﻿using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure
{
	public class ExecuteStoredProcedure : FunctionProvider
	{
		public override string SearchKeywords
		{
			get { return "database stored procedure"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new ExecuteStoredProcedureDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new ExecuteStoredProcedureDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new ExecuteStoredProcedureCodeGenerator(data);
		}

		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return FunctionUpdater.Instance.Update(data);
		}
	}
}
