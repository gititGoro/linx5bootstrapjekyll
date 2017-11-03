using System;
using System.Collections.Generic;
using Twenty57.Linx.Components.Database.UI.Editors.ConnectionEditor;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Validation;

namespace Twenty57.Linx.Components.Database.Common
{
	public abstract class DbDesignerBase : FunctionDesigner
	{
		public DbDesignerBase(IDesignerContext context)
			: base(context)
		{
			Properties.Add(new Property(DbShared.ConnectionTypePropertyName, typeof(ConnectionTypeSelection), ValueUseOption.DesignTime, ConnectionTypeSelection.SqlServer));
			Properties.Add(new Property(DbShared.DesignTimeConnectionTypePropertyName, typeof(ConnectionType?), ValueUseOption.DesignTime, null) { IsVisible = false });
			Properties.Add(new Property(DbShared.DesignTimeConnectionStringPropertyName, typeof(string), ValueUseOption.DesignTime, string.Empty) { IsVisible = false });

			SetPropertyAttributes();
			RefreshConnectionString();
		}

		public DbDesignerBase(IFunctionData data, IDesignerContext context) : base(data, context) { }

		public virtual ConnectionType[] SupportedConnectionTypes
		{
			get { return Enum.GetValues(typeof(ConnectionType)) as ConnectionType[]; }
		}

		public bool UseTransaction
		{
			get { return Properties[DbShared.ConnectionTypePropertyName].GetValue<ConnectionTypeSelection>() == ConnectionTypeSelection.UseTransaction; }
		}

		public ConnectionType ConnectionType
		{
			get 
			{ 
				var connectionType = Properties[DbShared.ConnectionTypePropertyName].GetValue<ConnectionTypeSelection>().ToConnectionType();
				return connectionType.HasValue ? connectionType.Value : ConnectionType.SqlServer;
			}
			set { Properties[DbShared.ConnectionTypePropertyName].Value = value.ToConnectionTypeSelection(); }
		}

		public ConnectionType ResolvedConnectionType
		{
			get
			{
				var transactionDesignerData = TransactionDesignerData;
				return transactionDesignerData == null ? ConnectionType : transactionDesignerData.ConnectionType;
			}
		}

		public string ConnectionString
		{
			get { return Properties[DbShared.ConnectionStringPropertyName].GetValue<string>(); }
			set { Properties[DbShared.ConnectionStringPropertyName].Value = value; }
		}

		public string ResolvedConnectionString
		{
			get
			{
				var transactionDesignerData = TransactionDesignerData;
				if (transactionDesignerData != null)
					return transactionDesignerData.ConnectionString;

				object resolvedConnectionString;

				if (!UseTransaction)
				{
					if (Context.TryParsePropertyValue(Properties[DbShared.ConnectionStringPropertyName], out resolvedConnectionString))
						return resolvedConnectionString as string;
				}				
				return null;
			}
		}

		public ConnectionType DesignTimeConnectionType
		{
			get 
			{
				ConnectionType? connectionType = Properties[DbShared.DesignTimeConnectionTypePropertyName].GetValue<ConnectionType?>();
				return connectionType.HasValue ? connectionType.Value : ResolvedConnectionType;
			}
			set { Properties[DbShared.DesignTimeConnectionTypePropertyName].Value = value; }
		}

		public string DesignTimeConnectionString
		{
			get
			{
				string connectionString = Properties[DbShared.DesignTimeConnectionStringPropertyName].GetValue<string>();
				return connectionString.IsEmptyConnectionString() ? (ResolvedConnectionString ?? connectionString) : connectionString;
			}
			set { Properties[DbShared.DesignTimeConnectionStringPropertyName].Value = value; }
		}

		private BeginTransaction.ITransactionDesignerData TransactionDesignerData
		{
			get
			{
				if (UseTransaction)
				{
					BeginTransaction.ITransactionDesignerData data;
					if (Context.TryGetDataFromReferenceItem<BeginTransaction.ITransactionDesignerData>(Properties[DbShared.TransactionPropertyName].Value, out data))
						return data;
				}
				return null;
			}
		}

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		private void SetPropertyAttributes()
		{
			Property connectionTypeProperty = Properties[DbShared.ConnectionTypePropertyName];
			connectionTypeProperty.Description = "The type of database to connect to.";
			connectionTypeProperty.Order = 0;
			connectionTypeProperty.ValueChanged += connectionTypeProperty_ValueChanged;

			ManageConnectionProperties();
		}

		private void connectionTypeProperty_ValueChanged(object sender, EventArgs e)
		{
			ManageConnectionProperties();

			if (!UseTransaction)
			{
				DesignTimeConnectionType = ConnectionType;
				RefreshConnectionString();
			}
		}

		private void RefreshConnectionString()
		{
			if (ConnectionString != null)
				ConnectionString = DatabaseAssistant.GetDatabaseAssistant(ConnectionType).RefreshConnectionString(ConnectionString);
		}

		private void connectionStringProperty_ValueChanged(object sender, EventArgs e)
		{
			if (ConnectionString != null)
				DesignTimeConnectionString = ConnectionString;
			else if (ResolvedConnectionString != null)
				DesignTimeConnectionString = string.Empty;

			string resolvedConnectionString = ResolvedConnectionString;
			if (resolvedConnectionString != null)
			{
				ConnectionType? detectedConnectionType = DatabaseAssistant.DetectConnectionType(resolvedConnectionString);
				if ((detectedConnectionType.HasValue) && (detectedConnectionType.Value != ConnectionType))
					ConnectionType = detectedConnectionType.Value;
			}
		}

		private void ManageConnectionProperties()
		{
			if (UseTransaction)
			{
				if (!Properties.ContainsName(DbShared.TransactionPropertyName))
					Properties.Add(new Property(DbShared.TransactionPropertyName, typeof(Transaction), ValueUseOption.RuntimeRead, null));

				if (Properties.ContainsName(DbShared.ConnectionStringPropertyName))
					Properties.Remove(Properties.GetItemByName(DbShared.ConnectionStringPropertyName));

				Property transactionProperty = Properties[DbShared.TransactionPropertyName];
				transactionProperty.Description = "Select a Transaction object from the BeginTransaction function.";
				transactionProperty.Order = 1;
				transactionProperty.Validations.Add(new RequiredValidator());
				transactionProperty.IsVisible = UseTransaction;
			}
			else
			{
				if (!Properties.ContainsName(DbShared.ConnectionStringPropertyName))
					Properties.Add(new Property(DbShared.ConnectionStringPropertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty));

				if (Properties.ContainsName(DbShared.TransactionPropertyName))
					Properties.Remove(Properties.GetItemByName(DbShared.TransactionPropertyName));

				Property connectionStringProperty = Properties[DbShared.ConnectionStringPropertyName];
				connectionStringProperty.Description = "Configuration values required by the data provider to connect to the data source.";
				connectionStringProperty.Order = 1;
				connectionStringProperty.Validations.Add(new RequiredValidator());
				connectionStringProperty.Editor = typeof(ConnectionEditorItem);
				connectionStringProperty.IsVisible = !UseTransaction;
				connectionStringProperty.ValueChanged += connectionStringProperty_ValueChanged;

				RefreshConnectionString();
			}
		}
	}
}
