using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.UI.Editors.ConnectionEditor;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.Database.UI.Controls.ConnectionControl
{
	public class ConnectionViewModel : INotifyPropertyChanged
	{
		private ConnectionType[] allowedConnectionTypes;
		private ConnectionType connectionType;
		private string connectionString;

		private ICommand editConnectionStringCommand = null;

		public ConnectionType[] AllowedConnectionTypes
		{
			get
			{
				if (allowedConnectionTypes == null)
					allowedConnectionTypes = Enum.GetValues(typeof(ConnectionType)) as ConnectionType[];
				return allowedConnectionTypes;
			}
			set { allowedConnectionTypes = value; }
		}

		public IEnumerable<EnumWrapper<ConnectionType>> ConnectionTypes
		{
			get { return AllowedConnectionTypes.Select(t => new EnumWrapper<ConnectionType>(t)); }
		}

		public EnumWrapper<ConnectionType> ConnectionType
		{
			get { return connectionType; }
			set
			{
				if (connectionType != value)
				{
					connectionType = value;
					NotifyPropertyChanged("ConnectionType");

					bool connectionStringChanged = false;
					if (ConnectionString is string)
					{
						string previousConnectionString = ConnectionString as string;
						ConnectionString = DatabaseAssistant.GetDatabaseAssistant(connectionType).RefreshConnectionString(previousConnectionString);
						connectionStringChanged = previousConnectionString != ConnectionString;
					}
					if ((ConnectionChanged != null) && (!connectionStringChanged) && (!connectionString.IsEmptyConnectionString()))
						ConnectionChanged(this, new EventArgs());
				}
			}
		}

		public string ConnectionString
		{
			get { return connectionString; }
			set
			{
				if (connectionString != value)
				{
					connectionString = value;

					ConnectionType? detectedConnectionType = DatabaseAssistant.DetectConnectionType(ConnectionString);
					if ((detectedConnectionType.HasValue) && (detectedConnectionType.Value != connectionType))
					{
						if (ConnectionString.IsEmptyConnectionString())
							connectionString = DatabaseAssistant.GetDatabaseAssistant(connectionType).RefreshConnectionString(connectionString);
						else
							ConnectionType = detectedConnectionType.Value;
					}

					NotifyPropertyChanged("ConnectionString");
					NotifyPropertyChanged("HasConnectionString");

					if (ConnectionChanged != null)
						ConnectionChanged(this, new EventArgs());
				}
			}
		}

		public ICommand EditConnectionStringCommand
		{
			get
			{
				if (editConnectionStringCommand == null)
					editConnectionStringCommand = new DelegateCommand(() => EditConnectionString());
				return editConnectionStringCommand;
			}
		}

		private void EditConnectionString()
		{
			ConnectionType editedConnectionType = connectionType;
			string editedConnectionString = ConnectionString;
			if (ConnectionEditorWindow.EditConnectionString(ref editedConnectionType, ref editedConnectionString))
			{
				ConnectionType = editedConnectionType;
				ConnectionString = editedConnectionString;
			}
		}


		private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler ConnectionChanged;
	}
}
