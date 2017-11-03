using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.UI;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ResultSetEditor.ViewModel
{
	public class ResultSetViewModel : INotifyPropertyChanged, IShowMessage
	{
		private ITypeReference selectedCustomType;
		private ObservableCollection<ResultSetFieldModel> resultSetFields;

		private bool? dialogResult;
		private ICommand saveCommand = null;

		public ResultSetViewModel(DatabaseModel.ResultSet resultSet)
		{
			ResultSetFields = new ObservableCollection<ResultSetFieldModel>(resultSet.Fields.Select(f => new ResultSetFieldModel(this, f)));
			selectedCustomType = resultSet.CustomType;
		}

		public DatabaseModel.ResultSet SavedResultSet { get; private set; }

		public ObservableCollection<ResultSetFieldModel> ResultSetFields
		{
			get { return resultSetFields; }
			set
			{
				resultSetFields = value;
				AddDefaultField();
				NotifyPropertyChanged();
			}
		}

		public ITypeReference SelectedCustomType
		{
			get { return selectedCustomType; }
			set
			{
				selectedCustomType = value;
				NotifyPropertyChanged("SelectedCustomType");
				NotifyPropertyChanged("IsMappingCustomType");
				if (selectedCustomType != null)
					NotifyPropertyChanged("CustomTypeProperties");

				if ((ResultSetFields.All(f => f.IsDefault)) && (selectedCustomType != null))
				{
					var resultSetFields = new ObservableCollection<ResultSetFieldModel>();
					foreach (var nextProperty in selectedCustomType.GetProperties())
					{
						DatabaseModel.DataType dataType;
						if (DatabaseModel.TryParseDataType(nextProperty.TypeReference, out dataType))
							resultSetFields.Add(new ResultSetFieldModel(this, new DatabaseModel.ResultSetField(nextProperty.Name, dataType)));
					}
					ResultSetFields = resultSetFields;
				}
			}
		}

		public bool IsMappingCustomType
		{
			get { return SelectedCustomType != null; }
		}

		public IEnumerable<string> CustomTypeProperties
		{
			get { return SelectedCustomType == null ? null : SelectedCustomType.GetProperties().Select(p => p.Name); }
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

		public ICommand SaveCommand
		{
			get
			{
				if (saveCommand == null)
					saveCommand = new DelegateCommand(() => Save());
				return saveCommand;
			}
		}

		public void AddDefaultField()
		{
			if ((ResultSetFields.Count == 0) || (!ResultSetFields.Last().IsDefault))
				ResultSetFields.Add(new ResultSetFieldModel(this));
		}

		public void Remove(ResultSetFieldModel resultSetField)
		{
			ResultSetFields.Remove(resultSetField);
		}

		public void MoveTo(ResultSetFieldModel toMove, int destinationIdx)
		{
			ResultSetFields.Move(ResultSetFields.IndexOf(toMove), destinationIdx);
		}

		private void Save()
		{
			DatabaseModel.ResultSet savedResultSet = new DatabaseModel.ResultSet { CustomType = SelectedCustomType };
			foreach (ResultSetFieldModel nextField in ResultSetFields)
			{
				if (string.IsNullOrEmpty(nextField.ColumnName))
					continue;
				if (IsMappingCustomType)
				{
					if ((!string.IsNullOrEmpty(nextField.OutputName)) && (!CustomTypeProperties.Contains(nextField.OutputName)))
					{
						if (ShowMessage != null)
							ShowMessage(this, new ShowMessageEventArgs(string.Format("Invalid field name: {0}.", nextField.OutputName), "Save result set", MessageBoxButton.OK, MessageBoxImage.Error));
						return;
					}
				}
				else
				{
					if (string.IsNullOrEmpty(nextField.OutputName))
					{
						if (ShowMessage != null)
							ShowMessage(this, new ShowMessageEventArgs("Please enter an output name for each column.", "Save result set", MessageBoxButton.OK, MessageBoxImage.Error));
						return;
					}
					if (!Names.IsNameValid(nextField.OutputName))
					{
						if (ShowMessage != null)
							ShowMessage(this, new ShowMessageEventArgs(string.Format("Output name {0} is not valid.", nextField.OutputName), "Save result set", MessageBoxButton.OK, MessageBoxImage.Error));
						return;
					}
				}
				if ((!string.IsNullOrEmpty(nextField.OutputName)) && (savedResultSet.Fields.Any(f => f.OutputName == nextField.OutputName)))
				{
					if (ShowMessage != null)
						ShowMessage(this, new ShowMessageEventArgs(string.Format("Duplicate output name: {0}.", nextField.OutputName), "Save result set", MessageBoxButton.OK, MessageBoxImage.Error));
					return;
				}

				savedResultSet.Fields.Add(nextField.ResultSetField);
			}

			SavedResultSet = savedResultSet;
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
