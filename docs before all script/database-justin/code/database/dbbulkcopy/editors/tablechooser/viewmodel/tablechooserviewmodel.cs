using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.UI;
using Twenty57.Linx.Components.Database.UI.Controls.ConnectionControl;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser.ViewModel
{
	public class TableChooserViewModel : INotifyPropertyChanged, IShowMessage
	{
		private string errorText;
		private ConnectionViewModel connectionViewModel;
		private bool isLoadingTables = false;
		private List<string> tables = null;
		private bool ignoreWhenTableIsBlanked = false;
		private string tableName;
		private int tableIndex;
		private List<TableColumnModel> tableColumns = null;

		private bool? dialogResult;
		private DelegateCommand saveCommand = null;

		public TableChooserViewModel(EditingInfo editingInfo, ConnectionViewModel connectionViewModel)
		{
			EditingInfo = editingInfo;
			tableName = editingInfo.TableName;

			this.connectionViewModel = connectionViewModel;
			connectionViewModel.AllowedConnectionTypes = editingInfo.AllowedConnectionTypes;
			connectionViewModel.ConnectionType = editingInfo.ConnectionType;
			connectionViewModel.ConnectionString = editingInfo.ConnectionString;
			connectionViewModel.ConnectionChanged += connectionViewModel_ConnectionChanged;

			Task.Factory.StartNew(() => { var list = Tables; });
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

		public DelegateCommand SaveCommand
		{
			get
			{
				if (saveCommand == null)
					saveCommand = new DelegateCommand(Save, () => !string.IsNullOrEmpty(tableName));
				return saveCommand;
			}
		}

		public EditingInfo EditingInfo { get; private set; }

		public string ErrorText
		{
			get { return errorText; }
			set
			{
				errorText = value;
				NotifyPropertyChanged("ErrorText");
				NotifyPropertyChanged("HasErrorText");
			}
		}

		public bool HasErrorText
		{
			get { return !string.IsNullOrEmpty(errorText); }
		}

		public bool HasConnectionString
		{
			get { return !connectionViewModel.ConnectionString.IsEmptyConnectionString(); }
		}

		public bool IsLoadingTables
		{
			get { return isLoadingTables; }
			set
			{
				isLoadingTables = value;
				NotifyPropertyChanged();
			}
		}

		public List<string> Tables
		{
			get
			{
				if (tables == null)
				{
					ErrorText = null;
					if (HasConnectionString)
					{
						var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionViewModel.ConnectionType);
						try
						{
							ignoreWhenTableIsBlanked = !string.IsNullOrEmpty(TableName);
							IsLoadingTables = true;
							try
							{
								using (var connection = databaseAssistant.CreateConnection(connectionViewModel.ConnectionString))
								{
									connection.Open();
									tables = new List<string>(databaseAssistant.GetTableNames(connection).OrderBy(t => t));
								}
							}
							finally
							{
								IsLoadingTables = false;
							}
							ErrorText = null;
							RefreshTableIndex();
							NotifyPropertyChanged("TableColumns");
						}
						catch (Exception exc)
						{
							if (tables == null)
								ErrorText = exc.Message;
							else
								throw exc;
						}
					}
					else
						tables = new List<string>();
				}

				return tables;
			}
		}

		public string TableName
		{
			get { return tableName; }
			set
			{
				if (ignoreWhenTableIsBlanked)
				{
					ignoreWhenTableIsBlanked = false;
					if (string.IsNullOrEmpty(value as string))
						return;
				}

				tableName = value;
				NotifyPropertyChanged();
				RefreshTableIndex();

				ResetTableColumns();
			}
		}

		public int TableIndex
		{
			get { return tableIndex; }
			private set
			{
				tableIndex = value;
				NotifyPropertyChanged();
			}
		}

		public List<TableColumnModel> TableColumns
		{
			get
			{
				if ((tableColumns == null) && (!IsLoadingTables) && (tables != null) && (tables.Contains(tableName)))
				{
					try
					{
						var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionViewModel.ConnectionType);
						using (var connection = databaseAssistant.CreateConnection(connectionViewModel.ConnectionString))
						{
							connection.Open();
							tableColumns = new List<TableColumnModel>(databaseAssistant.GetTableColumns(connection, tableName).Select(c => new TableColumnModel(c)));
						}
						ErrorText = null;
						NotifyPropertyChanged("HasTableColumns");
					}
					catch (Exception exc)
					{
						ErrorText = string.Format("Error fetching columns: {0}.", exc.Message);
					}
				}
				return tableColumns;
			}
		}

		public bool HasTableColumns { get { return tableColumns != null; } }


		private void connectionViewModel_ConnectionChanged(object sender, EventArgs e)
		{
			NotifyPropertyChanged("HasConnectionString");
			ResetTables();
		}

		private void RefreshTableIndex()
		{
			if ((tables != null) && (tables.Contains(tableName)))
				TableIndex = tables.IndexOf(tableName);
		}

		private void ResetTables()
		{
			tables = null;
			NotifyPropertyChanged("Tables");
			ResetTableColumns();
		}

		private void ResetTableColumns()
		{
			tableColumns = null;
			NotifyPropertyChanged("HasTableColumns");
			NotifyPropertyChanged("TableColumns");
		}

		private void Save()
		{
			if ((connectionViewModel.ConnectionType != EditingInfo.ConnectionType) || (connectionViewModel.ConnectionString != EditingInfo.ConnectionString))
			{
				if (EditingInfo.ConnectionString.IsEmptyConnectionString())
					EditingInfo.ShouldUpdateConnectionString = true;
				else if (ShowMessage != null)
				{
					var args = new ShowMessageEventArgs("Would you like to save the change to the connection string?", "Save", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
					ShowMessage(this, args);
					switch (args.MessageResponse)
					{
						case MessageBoxResult.Cancel: return;
						case MessageBoxResult.Yes: EditingInfo.ShouldUpdateConnectionString = true; break;
					}
				}
				EditingInfo.ConnectionType = connectionViewModel.ConnectionType;
				EditingInfo.ConnectionString = connectionViewModel.ConnectionString;
			}

			EditingInfo.TableName = TableName;
			EditingInfo.TableColumns = new DatabaseModel.Columns(TableColumns.Select(c => c.Column));

			DialogResult = true;
		}

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event ShowMessageEventHandler ShowMessage;
	}
}
