using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File
{
	public class DirectoryWatch : ServiceProvider
	{
		public override string Name { get { return "DirectoryWatch"; } }

		public override ServiceDesigner CreateDesigner(IServiceData data, IDesignerContext context)
		{
			return new DirectoryWatchDesigner(data, context);
		}

		public override ServiceDesigner CreateDesigner(IDesignerContext context)
		{
			return new DirectoryWatchDesigner(context);
		}

		public override ServiceCodeGenerator CreateCodeGenerator()
		{
			return new DirectoryWatchCodeGenerator();
		}

		public override IServiceData UpdateToLatestVersion(IServiceData data)
		{
			return ServiceUpdater.Instance.Update(data);
		}
	}
}
