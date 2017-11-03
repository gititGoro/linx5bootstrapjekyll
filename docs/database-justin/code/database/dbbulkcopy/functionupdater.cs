using System;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.Database.DbBulkCopy
{
	public class FunctionUpdater : IFunctionUpdater
	{
		private static FunctionUpdater instance;

		public string CurrentVersion { get { return "2"; } }

		public static IFunctionUpdater Instance
		{
			get
			{
				if (instance == null)
					instance = new FunctionUpdater();
				return instance;
			}
		}

		public IFunctionData Update(IFunctionData data)
		{
			if (data.Version == CurrentVersion)
				return data;

			if (string.IsNullOrEmpty(data.Version))
				data = UpdateToVersion1(data);

			if (data.Version == "1")
				data = UpdateToVersion2(data);

			if (data.Version == CurrentVersion)
				return data;

			throw new Exception(string.Format("Unknown version [{0}] specified.", data.Version));
		}

		private IFunctionData UpdateToVersion1(IFunctionData data)
		{
			data = AddTransactionProperty(data);
			data = TransformConnectionTypeProperty(data);

			return data.UpdateVersion("1");
		}

		private IFunctionData AddTransactionProperty(IFunctionData data)
		{
			if (data.FindPropertyById(DbShared.TransactionPropertyName) == null)
				data = data.AddProperty(new Property(DbShared.TransactionPropertyName, typeof(Transaction), ValueUseOption.DesignTime, null));

			return data;
		}

		private IFunctionData TransformConnectionTypeProperty(IFunctionData data)
		{
			IPropertyData connectionTypeProperty;
			if (data.TryFindPropertyById(DbShared.ConnectionTypePropertyName, out connectionTypeProperty))
			{
				if (connectionTypeProperty.TypeReference.IsCompiled && connectionTypeProperty.TypeReference.GetUnderlyingType() == typeof(ConnectionTypeSelection))
					return data;

				var newConnectionTypeProperty = new Property(DbShared.ConnectionTypePropertyName, typeof(ConnectionTypeSelection), ValueUseOption.DesignTime, ConnectionTypeSelection.SqlServer)
				{
					Value = connectionTypeProperty.GetValue<Common.ConnectionType>().ToConnectionTypeSelection(),
					IsVisible = connectionTypeProperty.IsVisible
				};

				data = data.ReplaceProperty(connectionTypeProperty, newConnectionTypeProperty);
			}

			return data;
		}

		private IFunctionData UpdateToVersion2(IFunctionData data)
		{
			return data
				.AddProperty(new Property(DbBulkCopyShared.TimeoutPropertyName, typeof(int), ValueUseOption.DesignTime, 30))
				.UpdateVersion("2");
		}
	}
}
