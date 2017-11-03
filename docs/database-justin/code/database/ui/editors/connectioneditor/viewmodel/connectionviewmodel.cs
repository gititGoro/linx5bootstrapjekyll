using MSDASC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.Database.UI.Editors.ConnectionEditor.ViewModel
{
	public class ConnectionViewModel : INotifyPropertyChanged, IShowMessage
	{
		private ConnectionType[] allowedConnectionTypes;
		private ConnectionType connectionType;
		private ObservableCollection<ConnectionParameter> connectionParameters;
		private Dictionary<string, string> parameterValueCache = new Dictionary<string, string>();
		private bool isBusy = false;
		private IntPtr windowHandle = IntPtr.Zero;

		private bool? dialogResult;
		private ICommand openWizardCommand = null, testConnectionCommand = null, saveCommand = null;
		private bool wizardIsOpen = false;

		public ConnectionViewModel(ConnectionType connectionType, string connectionString)
		{
			this.connectionType = connectionType;
			ConnectionString = connectionString;
		}

		public bool? DialogResult
		{
			get { return dialogResult; }
			set
			{
				dialogResult = value;
				NotifyPropertyChanged();
			}
		}

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
					NotifyPropertyChanged("IsWizardAvailable");
					RefreshConnectionParameters();
				}
			}
		}

		public bool IsWizardAvailable
		{
			get { return connectionType == Common.ConnectionType.OleDb; }
		}

		public ObservableCollection<ConnectionParameter> ConnectionParameters
		{
			get { return connectionParameters; }
			private set
			{
				connectionParameters = value;
				AddDefaultConnectionParameter();
				NotifyPropertyChanged();
			}
		}

		public string ConnectionString
		{
			get
			{
				return ConnectionStringHelpers.GetConnectionString(ConnectionParameters.Where(p => !string.IsNullOrEmpty(p.Name)).ToLookup(p => p.Name, p => p.TextValue));
			}
			private set
			{
				var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionType);
				var connectionParameterMap =
					databaseAssistant.RefreshConnectionParameters(value == null
						? Enumerable.Empty<string>().ToLookup(x => string.Empty)
						: value.GetConnectionParameterMap().ToLookup(p => p.Key, p => p.Value), true);
				ConnectionParameters = new ObservableCollection<ConnectionParameter>(connectionParameterMap.Keys.Select(p => new ConnectionParameter(this, p, connectionParameterMap[p])));
			}
		}

		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				isBusy = value;
				NotifyPropertyChanged();
			}
		}

		public ICommand OpenWizardCommand
		{
			get
			{
				if (openWizardCommand == null)
					openWizardCommand = new DelegateCommand(OpenWizard, () => !this.wizardIsOpen);
				return openWizardCommand;
			}
		}

		public ICommand TestConnectionCommand
		{
			get
			{
				if (testConnectionCommand == null)
					testConnectionCommand = new DelegateCommand(TestConnection);
				return testConnectionCommand;
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				if (saveCommand == null)
					saveCommand = new DelegateCommand(() => DialogResult = true);
				return saveCommand;
			}
		}

		public void RefreshConnectionParameters(bool retainUnknownParameters = false)
		{
			foreach (var nextParameter in ConnectionParameters)
			{
				if (parameterValueCache.ContainsKey(nextParameter.Name.ToLower()))
					parameterValueCache[nextParameter.Name.ToLower()] = nextParameter.TextValue;
				else
					parameterValueCache.Add(nextParameter.Name.ToLower(), nextParameter.TextValue);
			}

			var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionType);
			var convertedParameters = databaseAssistant.RefreshConnectionParameters(ConnectionParameters.Where(p => !p.IsDefault).ToLookup(p => p.Name, p => p.TextValue), retainUnknownParameters);
			ConnectionParameters = new ObservableCollection<ConnectionParameter>(convertedParameters.Select(p => new ConnectionParameter(this, p.Key, p.Value)));

			foreach (var nextParameter in ConnectionParameters)
				if ((parameterValueCache.ContainsKey(nextParameter.Name.ToLower())) && (nextParameter.TextValue == nextParameter.DefaultTextValue))
				{
					string cachedValue = parameterValueCache[nextParameter.Name.ToLower()];
					if (!string.IsNullOrEmpty(cachedValue))
						nextParameter.TextValue = cachedValue;
				}
		}

		public void AddDefaultConnectionParameter()
		{
			if ((ConnectionParameters.Count == 0) || (!ConnectionParameters.Last().IsDefault))
				ConnectionParameters.Add(new ConnectionParameter(this));
		}

		public void Remove(ConnectionParameter parameterToRemove)
		{
			ConnectionParameters.Remove(parameterToRemove);
		}

		public void SetWindowHandle(IntPtr windowHandle)
		{
			this.windowHandle = windowHandle;
		}

		private void OpenWizard()
		{
			try
			{
				this.wizardIsOpen = true;
				var tokenSource = new CancellationTokenSource();
				SetConnectionEditorParent(tokenSource.Token);

				if (IsConnectionParametersEmpty())
				{
					string newConnectionString;
					if (GetNewConnectionStringFromWizard(out newConnectionString))
						ConnectionString = newConnectionString;
				}
				else
				{
					string modifiedConnectionString;
					if (GetModifiedConnectionStringFromWizard(ConnectionString, out modifiedConnectionString))
						ConnectionString = modifiedConnectionString;
				}

				tokenSource.Cancel();
			}
			finally
			{
				this.wizardIsOpen = false;
			}
		}

		private bool IsConnectionParametersEmpty()
		{
			return ConnectionParameters.All(p => (string.IsNullOrEmpty(p.Name)) || (p.ComponentType != DatabaseModel.ConnectionStringComponentType.Text)
				|| (string.IsNullOrEmpty(p.TextValue)) || (p.TextValue == p.DefaultTextValue));
		}

		private void TestConnection()
		{
			IsBusy = true;
			Task.Factory.StartNew(() =>
			{
				try
				{
					using (IDbConnection connection = DatabaseAssistant.GetDatabaseAssistant(connectionType).CreateConnection(ConnectionString))
					{
						connection.Open();
					}
					if (ShowMessage != null)
						Application.Current.Dispatcher.Invoke(() => ShowMessage(this, new ShowMessageEventArgs("Success!", "Connect to database", MessageBoxButton.OK, MessageBoxImage.Information)));
				}
				catch (Exception exc)
				{
					if (ShowMessage != null)
						Application.Current.Dispatcher.Invoke(() => ShowMessage(this, new ShowMessageEventArgs(exc.Message, "Error connecting", MessageBoxButton.OK, MessageBoxImage.Error)));
				}
				finally
				{
					IsBusy = false;
				}
			});
		}

		#region INotifyPropertyChanged
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		private void SetConnectionEditorParent(CancellationToken token)
		{
			if (this.windowHandle == IntPtr.Zero || Application.Current == null)
				return;

			try
			{
				Task.Factory.StartNew(() =>
					{
						int counter = 0;
						while (counter < 10)
						{
							token.ThrowIfCancellationRequested();

							var editorHandle = FindWindow(null, "Data Link Properties");
							if (editorHandle != IntPtr.Zero)
							{
								Application.Current.Dispatcher.InvokeAsync(() =>
								{
									int GWLP_HWNDPARENT = -8;
									SetWindowLongPtr(new HandleRef(null, editorHandle), GWLP_HWNDPARENT, this.windowHandle);
								});
								break;
							}

							System.Threading.Thread.Sleep(500);
							counter++;
						}
					}, token);
			}
			catch { }
		}

		private static bool GetNewConnectionStringFromWizard(out string newConnectionString)
		{
			newConnectionString = null;

			DataLinks dataLinks = new DataLinksClass();
			object result = dataLinks.PromptNew();
			if (result != null)
			{
				newConnectionString = ((ADODB.Connection)result).ConnectionString;
				return true;
			}

			return false;
		}

		private static bool GetModifiedConnectionStringFromWizard(string existingConnectionString, out string modifiedConnectionString)
		{
			modifiedConnectionString = null;

			try
			{
				DataLinks dataLinks = new DataLinksClass();

				ADODB.Connection connection = new ADODB.ConnectionClass { ConnectionString = existingConnectionString };
				object refObject = connection;
				if (dataLinks.PromptEdit(ref refObject))
				{
					modifiedConnectionString = connection.ConnectionString;
					return true;
				}

				return false;
			}
			catch
			{
				string newConnectionString;
				if (GetNewConnectionStringFromWizard(out newConnectionString))
				{
					modifiedConnectionString = newConnectionString;
					return true;
				}

				return false;
			}
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size == 8)
				return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
			else
				return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
		}

		[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
		private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
		private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

		public event ShowMessageEventHandler ShowMessage;
	}
}
