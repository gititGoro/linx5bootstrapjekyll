using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.UI;
using Twenty57.Linx.Components.Database.UI.Controls.ConnectionControl;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.StoredProcedureEditor.ViewModel
{
	public class StoredProcedureViewModel : INotifyPropertyChanged
	{
		private ConnectionViewModel connectionViewModel;
		private string storedProcedure;
		private bool ignoreWhenStoredProcedureIsBlanked = false;
		private int storedProcedureIndex;
		private List<string> storedProcedures = null;
		private bool isLoadingStoredProcedures = false;
		private List<ProcedureParameterModel> procedureParameters = null;
		private DatabaseModel.ResultSets resultSets = null;
		private string errorText = null;

		private bool? dialogResult;
		private DelegateCommand saveCommand = null;

		public StoredProcedureViewModel(EditingInfo editingInfo, ConnectionViewModel connectionViewModel)
		{
			EditingInfo = editingInfo;
			storedProcedure = editingInfo.StoredProcedure;

			this.connectionViewModel = connectionViewModel;
			connectionViewModel.ConnectionType = editingInfo.ConnectionType;
			connectionViewModel.ConnectionString = editingInfo.ConnectionString;
			connectionViewModel.ConnectionChanged += connectionViewModel_ConnectionChanged;

			Task.Factory.StartNew(() => { var list = StoredProcedures; });
		}

		public EditingInfo EditingInfo { get; private set; }

		public bool ShouldUpdateConnectionString
		{
			get { return EditingInfo.ShouldUpdateConnectionString; }
			set
			{
				EditingInfo.ShouldUpdateConnectionString = value;
				NotifyPropertyChanged();
			}
		}

		public bool ShouldUpdateStoredProcedure
		{
			get { return EditingInfo.ShouldUpdateStoredProcedure; }
			set
			{
				EditingInfo.ShouldUpdateStoredProcedure = value;
				NotifyPropertyChanged();
			}
		}

		public bool ShouldUpdateParameters
		{
			get { return EditingInfo.ShouldUpdateProcedureParameters; }
			set
			{
				EditingInfo.ShouldUpdateProcedureParameters = value;
				NotifyPropertyChanged();
				NotifyCanSaveChanged();
			}
		}

		public bool ShouldUpdateResultSets
		{
			get { return EditingInfo.ShouldUpdateResultSets; }
			set
			{
				EditingInfo.ShouldUpdateResultSets = value;
				NotifyPropertyChanged();
				NotifyCanSaveChanged();
			}
		}

		public bool HasConnectionString
		{
			get { return !connectionViewModel.ConnectionString.IsEmptyConnectionString(); }
		}

		public string StoredProcedure
		{
			get { return storedProcedure; }
			set
			{
				if (ignoreWhenStoredProcedureIsBlanked)
				{
					ignoreWhenStoredProcedureIsBlanked = false;
					if (string.IsNullOrEmpty(value as string))
						return;
				}

				storedProcedure = value;
				NotifyPropertyChanged("StoredProcedure");
				RefreshStoredProcedureIndex();
				NotifyCanSaveChanged();

				ResetProcedureParameters();
			}
		}

		public int StoredProcedureIndex
		{
			get { return storedProcedureIndex; }
			private set
			{
				storedProcedureIndex = value;
				NotifyPropertyChanged();
			}
		}

		public List<string> StoredProcedures
		{
			get
			{
				if (storedProcedures == null)
				{
					ErrorText = null;
					if (HasConnectionString)
					{
						var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionViewModel.ConnectionType);
						try
						{
							ignoreWhenStoredProcedureIsBlanked = !string.IsNullOrEmpty(StoredProcedure);
							IsLoadingStoredProcedures = true;
							try
							{
								using (var connection = databaseAssistant.CreateConnection(connectionViewModel.ConnectionString))
								{
									connection.Open();
									storedProcedures = new List<string>(databaseAssistant.GetStoredProcedureNames(connection));
								}
							}
							finally
							{
								IsLoadingStoredProcedures = false;
							}
							ErrorText = null;
							RefreshStoredProcedureIndex();
							NotifyPropertyChanged("ProcedureParameters");
							NotifyPropertyChanged("ResultSets");
							NotifyPropertyChanged("ResultSetCount");
							NotifyPropertyChanged("ResultSetCountText");
						}
						catch (Exception exc)
						{
							if (storedProcedures == null)
								ErrorText = exc.Message;
							else
								throw exc;
						}
					}
					else
						storedProcedures = new List<string>();
				}
				return storedProcedures;
			}
		}

		public bool IsLoadingStoredProcedures
		{
			get { return isLoadingStoredProcedures; }
			private set
			{
				isLoadingStoredProcedures = value;
				NotifyPropertyChanged();
			}
		}

		public string ErrorText
		{
			get { return errorText; }
			private set
			{
				errorText = value;
				NotifyPropertyChanged("ErrorText");
				NotifyPropertyChanged("HasErrorText");
			}
		}

		public bool HasErrorText { get { return errorText != null; } }

		public List<ProcedureParameterModel> ProcedureParameters
		{
			get 
			{
				if ((procedureParameters == null) && (!IsLoadingStoredProcedures) && (storedProcedures != null) && (storedProcedures.Contains(storedProcedure)))
				{
					try
					{
						var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionViewModel.ConnectionType);
						using (var connection = databaseAssistant.CreateConnection(connectionViewModel.ConnectionString))
						{
							connection.Open();
							procedureParameters = new List<ProcedureParameterModel>(databaseAssistant.GetStoredProcedureParameters(connection, storedProcedure).Select(p => new ProcedureParameterModel(p)));
						}
						ErrorText = null;
						NotifyPropertyChanged("HasProcedureParameters");
						if (ShouldUpdateParameters)
							NotifyCanSaveChanged();
					}
					catch (Exception exc)
					{
						ErrorText = string.Format("Error fetching parameters: {0}.", exc.Message);
					}
				}

				return procedureParameters;
			}
		}

		public bool HasProcedureParameters { get { return procedureParameters != null; } }

		public DatabaseModel.ResultSets ResultSets
		{
			get
			{
				if ((resultSets == null) && (!IsLoadingStoredProcedures) && (storedProcedures != null) && (storedProcedures.Contains(storedProcedure)))
				{
					try
					{
						var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionViewModel.ConnectionType);
						using (var connection = databaseAssistant.CreateConnection(connectionViewModel.ConnectionString))
						{
							connection.Open();
							resultSets = databaseAssistant.GetResultSets(connection, storedProcedure);
						}
						ErrorText = null;
						NotifyPropertyChanged("HasResultSets");
						NotifyPropertyChanged("ResultSetCount");
						NotifyPropertyChanged("ResultSetCountText");
						if (ShouldUpdateResultSets)
							NotifyCanSaveChanged();
					}
					catch (Exception exc)
					{
						ErrorText = string.Format("Error fetching result sets: {0}.", exc.Message);
					}
				}

				return resultSets;
			}
		}

		public int ResultSetCount { get { return ResultSets == null ? 0 : ResultSets.Count; } }

		public string ResultSetCountText { get { return string.Format("{0} {1}", ResultSetCount, ResultSetCount == 1 ? "result set" : "result sets"); } }

		public bool HasResultSets { get { return resultSets != null; } }

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
					saveCommand = new DelegateCommand(() => Save(), () => CanSave());
				return saveCommand;
			}
		}

		private void connectionViewModel_ConnectionChanged(object sender, EventArgs e)
		{
			NotifyPropertyChanged("HasConnectionString");
			ResetStoredProcedures();
		}

		private void ResetStoredProcedures()
		{
			storedProcedures = null;
			NotifyPropertyChanged("StoredProcedures");
			ResetProcedureParameters();
		}

		private void ResetProcedureParameters()
		{
			procedureParameters = null;
			NotifyPropertyChanged("HasProcedureParameters");
			NotifyPropertyChanged("ProcedureParameters");
			resultSets = null;
			NotifyPropertyChanged("HasResultSets");
			NotifyPropertyChanged("ResultSets");
			NotifyPropertyChanged("ResultSetCount");
			NotifyPropertyChanged("ResultSetCountText");
		}

		private void RefreshStoredProcedureIndex()
		{
			if ((storedProcedures != null) && (storedProcedures.Contains(storedProcedure)))
				StoredProcedureIndex = storedProcedures.IndexOf(storedProcedure);
		}

		private bool CanSave()
		{
			if (string.IsNullOrEmpty(StoredProcedure))
				return false;
			return ((!ShouldUpdateParameters) || (HasProcedureParameters)) && ((!ShouldUpdateResultSets) || (HasResultSets));
		}

		private void Save()
		{
			EditingInfo.ConnectionType = connectionViewModel.ConnectionType;
			EditingInfo.ConnectionString = connectionViewModel.ConnectionString;
			EditingInfo.StoredProcedure = StoredProcedure;
			if (ShouldUpdateParameters)
				EditingInfo.ProcedureParameters = new DatabaseModel.ProcedureParameters(ProcedureParameters.Select(p => p.ProcedureParameter));
			if (ShouldUpdateResultSets)
				EditingInfo.ResultSets = ResultSets;

			DialogResult = true;
		}

		private void NotifyCanSaveChanged()
		{
			Application.Current.Dispatcher.Invoke(SaveCommand.RaiseCanExecuteChanged);
		}

		#region INotifyPropertyChanged
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
