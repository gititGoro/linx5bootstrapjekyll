using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;

namespace Twenty57.Linx.Components.Database.BeginTransaction
{
	public class BeginTransactionDesigner : DbDesignerBase
	{
		public BeginTransactionDesigner(IDesignerContext context) : base(context)
		{
			Properties[DbShared.ConnectionTypePropertyName].IsVisible = false;
			Properties.Add(new Property(BeginTransactionShared.ConnectionTypePropertyName, typeof(ConnectionType), ValueUseOption.DesignTime, ConnectionType.SqlServer));

			Properties.Add(new Property(BeginTransactionShared.SqlServerIsolationLevelPropertyName, typeof(SqlServerIsolationLevel), ValueUseOption.DesignTime, SqlServerIsolationLevel.ReadCommitted));
			Properties.Add(new Property(BeginTransactionShared.OracleIsolationLevelPropertyName, typeof(OracleIsolationLevel), ValueUseOption.DesignTime, OracleIsolationLevel.ReadCommitted));
			Properties.Add(new Property(BeginTransactionShared.OleDbIsolationLevelPropertyName, typeof(OleDbIsolationLevel), ValueUseOption.DesignTime, OleDbIsolationLevel.ReadCommitted));
			Properties.Add(new Property(BeginTransactionShared.OdbcIsolationLevelPropertyName, typeof(OdbcIsolationLevel), ValueUseOption.DesignTime, OdbcIsolationLevel.ReadCommitted));
			SetPropertyAttributes();

			var output = TypeReference.CreateResource(typeof(Transaction));
			ExecutionPaths.Add(BeginTransactionShared.ExecutionPathName, BeginTransactionShared.ExecutionPathName, output, IterationHint.Once);
		}
		
		public BeginTransactionDesigner(IFunctionData data, IDesignerContext context) : 
			base(data, context) { }

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		public override bool TryGetDataForTypeReference<T>(ITypeReference typeReference, out T data)
		{
			if (!typeReference.Equals(ExecutionPaths.Single().Output))
				return base.TryGetDataForTypeReference<T>(typeReference, out data);

			if (typeof(T) != typeof(ITransactionDesignerData))
				throw new ArgumentException(string.Format("Invalid data type: {0}.", typeof(T)));

			data = (T)(object)new TransactionDesignerData(this);
			return true;
		}

		private void SetPropertyAttributes()
		{
			Property connectionTypeProperty = Properties[BeginTransactionShared.ConnectionTypePropertyName];
			connectionTypeProperty.Description = "The type of database to connect to.";
			connectionTypeProperty.Order = 0;
			connectionTypeProperty.ValueChanged += ConnectionTypeProperty_ValueChanged;

			foreach (string isolationLevelPropertyName in 
				new string[] { BeginTransactionShared.SqlServerIsolationLevelPropertyName, BeginTransactionShared.OracleIsolationLevelPropertyName, BeginTransactionShared.OleDbIsolationLevelPropertyName, BeginTransactionShared.OdbcIsolationLevelPropertyName })
			{
				Property isolationLevelProperty = Properties[isolationLevelPropertyName];
				isolationLevelProperty.Description = "The isolation level for the database transaction.";
				isolationLevelProperty.Order = 3;
			}
			RefreshIsolationLevelPropertiesVisibility();
		}

		private void ConnectionTypeProperty_ValueChanged(object sender, EventArgs e)
		{
			RefreshIsolationLevelPropertiesVisibility();

			ConnectionType = Properties[BeginTransactionShared.ConnectionTypePropertyName].GetValue<ConnectionType>();
		}

		private void RefreshIsolationLevelPropertiesVisibility()
		{
			Properties[BeginTransactionShared.SqlServerIsolationLevelPropertyName].IsVisible = ConnectionType == ConnectionType.SqlServer;
			Properties[BeginTransactionShared.OracleIsolationLevelPropertyName].IsVisible = ConnectionType == ConnectionType.Oracle;
			Properties[BeginTransactionShared.OleDbIsolationLevelPropertyName].IsVisible = ConnectionType == ConnectionType.OleDb;
			Properties[BeginTransactionShared.OdbcIsolationLevelPropertyName].IsVisible = ConnectionType == ConnectionType.Odbc;
		}


		private class TransactionDesignerData : ITransactionDesignerData
		{
			private BeginTransactionDesigner designer;

			public TransactionDesignerData(BeginTransactionDesigner designer)
			{
				this.designer = designer;
			}

			public ConnectionType ConnectionType
			{
				get { return designer.ConnectionType; }
			}

			public string ConnectionString
			{
				get { return designer.ResolvedConnectionString; }
			}

			public IsolationLevel IsolationLevel
			{
				get
				{
					string isolationLevelName = null;
					switch (designer.ConnectionType)
					{
						case ConnectionType.SqlServer: isolationLevelName = designer.Properties[BeginTransactionShared.SqlServerIsolationLevelPropertyName].Value.ToString(); break;
						case ConnectionType.Oracle: isolationLevelName = designer.Properties[BeginTransactionShared.OracleIsolationLevelPropertyName].Value.ToString(); break;
						case ConnectionType.OleDb: isolationLevelName = designer.Properties[BeginTransactionShared.OleDbIsolationLevelPropertyName].Value.ToString(); break;
						case ConnectionType.Odbc: isolationLevelName = designer.Properties[BeginTransactionShared.OdbcIsolationLevelPropertyName].Value.ToString(); break;
					}
					return (IsolationLevel)Enum.Parse(typeof(IsolationLevel), isolationLevelName);
				}
			}
		}
	}
}
