using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.UI.Editors.ConnectionEditor.ViewModel
{
	public class ConnectionParameter : INotifyPropertyChanged
	{
		private ConnectionViewModel connectionViewModel;
		private string name, textValue;

		public ConnectionParameter(ConnectionViewModel connectionViewModel)
		{
			this.connectionViewModel = connectionViewModel;
			name = textValue = string.Empty;
			IsDefault = true;
		}

		public ConnectionParameter(ConnectionViewModel connectionViewModel, string name, string textValue)
		{
			this.connectionViewModel = connectionViewModel;
			this.name = name;
			this.textValue = textValue;
			IsDefault = false;
		}

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				NotifyPropertyChanged("Name");
				NotifyPropertyChanged("Description");
				NotifyPropertyChanged("SuccinctDescription");
				NotifyPropertyChanged("HasSuccinctDescription");
				NotifyPropertyChanged("ComponentType");
			}
		}

		public string TextValue
		{
			get { return textValue; }
			set
			{
				textValue = value;
				NotifyPropertyChanged();
			}
		}

		public bool FlagValue
		{
			get 
			{
				var connectionStringComponent = ConnectionStringComponent;
				return connectionStringComponent == null ? false : TextValue.Equals(connectionStringComponent.TrueValue, StringComparison.CurrentCultureIgnoreCase); 
			}
			set
			{
				var connectionStringComponent = ConnectionStringComponent;
				if (connectionStringComponent != null)
				{
					TextValue = value ? connectionStringComponent.TrueValue : connectionStringComponent.FalseValue;
					connectionViewModel.RefreshConnectionParameters(true);
				}
			}
		}

		public string Description
		{
			get
			{
				var connectionStringComponent = ConnectionStringComponent;
				return connectionStringComponent == null ? null : connectionStringComponent.Description;
			}
		}

		public string SuccinctDescription
		{
			get
			{
				var connectionStringComponent = ConnectionStringComponent;
				return connectionStringComponent == null ? null : connectionStringComponent.SuccinctDescription;
			}
		}

		public bool HasSuccinctDescription { get { return SuccinctDescription != null; } }

		public DatabaseModel.ConnectionStringComponentType ComponentType
		{
			get
			{
				var connectionStringComponent = ConnectionStringComponent;
				return connectionStringComponent == null ? DatabaseModel.ConnectionStringComponentType.Text : connectionStringComponent.Type;
			}
		}

		public string DefaultTextValue
		{
			get
			{
				var connectionStringComponent = ConnectionStringComponent;
				return connectionStringComponent == null ? string.Empty : connectionStringComponent.DefaultTextValue;
			}
		}

		public bool IsDefault { get; set; }

		public void Remove()
		{
			connectionViewModel.Remove(this);
		}

		private DatabaseModel.ConnectionStringComponent ConnectionStringComponent
		{
			get
			{
				return DatabaseAssistant.GetDatabaseAssistant(connectionViewModel.ConnectionType).BasicConnectionStringComponents.SingleOrDefault(c => c.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase));
			}
		}

		#region INotifyPropertyChanged
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

			if ((propertyName == "Name") && (IsDefault))
			{
				IsDefault = false;
				connectionViewModel.AddDefaultConnectionParameter();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
