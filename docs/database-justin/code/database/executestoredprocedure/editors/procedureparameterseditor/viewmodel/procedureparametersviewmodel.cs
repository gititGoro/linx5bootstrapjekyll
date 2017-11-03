using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.StoredProcedureEditor;
using Twenty57.Linx.Components.Database.UI;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor.ViewModel
{
	public class ProcedureParametersViewModel : INotifyPropertyChanged, IShowMessage
	{
		private ObservableCollection<ProcedureParameterModel> parameters;

		private bool? dialogResult;
		private ICommand getParametersCommand = null;
		private ICommand saveCommand = null;

		public ProcedureParametersViewModel(EditingInfo editingInfo)
		{
			EditingInfo = editingInfo;
			Parameters = new ObservableCollection<ProcedureParameterModel>(editingInfo.ProcedureParameters.Select(p => new ProcedureParameterModel(this, p)));
		}

		public EditingInfo EditingInfo { get; private set; }

		public ObservableCollection<ProcedureParameterModel> Parameters
		{
			get { return parameters; }
			private set
			{
				parameters = value;
				AddDefaultParameter();
				NotifyPropertyChanged();
			}
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

		public ICommand GetParametersCommand
		{
			get
			{
				if (getParametersCommand == null)
					getParametersCommand = new DelegateCommand(() => GetParameters());
				return getParametersCommand;
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				if (saveCommand == null)
					saveCommand = new DelegateCommand(() => Save());
				return saveCommand;
			}
		}

		internal void AddDefaultParameter()
		{
			if ((!Parameters.Any()) || (!Parameters.Last().IsDefault))
				Parameters.Add(new ProcedureParameterModel(this));
		}

		public void Remove(ProcedureParameterModel parameter)
		{
			Parameters.Remove(parameter);
		}

		public void MoveTo(ProcedureParameterModel toMove, int destinationIdx)
		{
			Parameters.Move(Parameters.IndexOf(toMove), destinationIdx);
		}

		private void GetParameters()
		{
			if (StoredProcedureEditorItem.EditStoredProcedure(EditingInfo))
				Parameters = new ObservableCollection<ProcedureParameterModel>(EditingInfo.ProcedureParameters.Select(p => new ProcedureParameterModel(this, p)));
		}

		private void Save()
		{
			DatabaseModel.ProcedureParameters savedParameters = new DatabaseModel.ProcedureParameters();
			int parameterPosition = 0;
			foreach (var nextParameterModel in Parameters)
			{
				if (string.IsNullOrEmpty(nextParameterModel.Name))
					continue;
				if (savedParameters.Any(p => p.Name.Equals(nextParameterModel.Name, StringComparison.CurrentCultureIgnoreCase)))
				{
					if (ShowMessage != null)
						ShowMessage(this, new ShowMessageEventArgs("Duplicate parameter name: " + nextParameterModel.Name, "Save parameters", MessageBoxButton.OK, MessageBoxImage.Asterisk));
					return;
				}
				if ((nextParameterModel.Direction == DatabaseModel.ParameterDirection.ReturnValue) && (savedParameters.Any(p => p.Direction == DatabaseModel.ParameterDirection.ReturnValue)))
				{
					if (ShowMessage != null)
						ShowMessage(this, new ShowMessageEventArgs("At most one Return Value parameter is allowed.", "Save parameters", MessageBoxButton.OK, MessageBoxImage.Asterisk));
					return;
				}
				nextParameterModel.ProcedureParameter.Position = parameterPosition++;
				savedParameters.Add(nextParameterModel.ProcedureParameter);
			}

			EditingInfo.ShouldUpdateProcedureParameters = true;
			EditingInfo.ProcedureParameters = savedParameters;
			DialogResult = true;
		}

		#region INotifyPropertyChanged
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		public event ShowMessageEventHandler ShowMessage;
	}
}
