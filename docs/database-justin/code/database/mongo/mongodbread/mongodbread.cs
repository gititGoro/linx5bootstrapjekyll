using Twenty57.Linx.Components.Database.Mongo.MongoDBRead;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.MongoDB
{
	public class MongoDBRead : FunctionProvider
	{
		public override string Name
		{
			get { return "MongoDBRead"; }
		}

		public override string SearchKeywords
		{
			get { return "mongo database read"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new MongoDBReadDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new MongoDBReadDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new MongoDBReadCodeGenerator(data);
		}

		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return MongoDBReadFunctionUpdater.Instance.Update(data);
		}
	}
}