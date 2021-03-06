using Twenty57.Linx.Components.Database.Mongo.MongoDBMapReduce;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.MongoDB
{
	public class MongoDBMapReduce : FunctionProvider
	{
		public override string Name
		{
			get { return "MongoDBMapReduce"; }
		}

		public override string SearchKeywords
		{
			get { return "mongo database map reduce"; }
		}

		public override FunctionDesigner CreateDesigner(IDesignerContext context)
		{
			return new MongoDBMapReduceDesigner(context);
		}

		public override FunctionDesigner CreateDesigner(IFunctionData data, IDesignerContext context)
		{
			return new MongoDBMapReduceDesigner(data, context);
		}

		public override FunctionCodeGenerator CreateCodeGenerator(IFunctionData data)
		{
			return new MongoDBMapReduceCodeGenerator(data);
		}
		public override IFunctionData UpdateToLatestVersion(IFunctionData data)
		{
			return MongoDBMapReduceFunctionUpdater.Instance.Update(data);
		}
	}
}