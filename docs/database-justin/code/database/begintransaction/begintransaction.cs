using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.BeginTransaction
{
	public class BeginTransaction : FunctionProvider
	{
		public override string Name
		{
			get { return "BeginTransaction"; }
		}

		public override string SearchKeywords
		{
			get { return "sql statement database transaction"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new BeginTransactionDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new BeginTransactionDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new BeginTransactionCodeGenerator(data);
		}
	}
}
