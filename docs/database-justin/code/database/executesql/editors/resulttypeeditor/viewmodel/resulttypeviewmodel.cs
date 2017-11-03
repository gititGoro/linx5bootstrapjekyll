using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.ResultTypeEditor.ViewModel
{
	public class ResultTypeViewModel : INotifyPropertyChanged
	{
		public event Action PostSave;

		private ExecuteSQLDesigner executeSQLDesigner;
		private ITypeReference selectedCustomType;
		private ObservableCollection<ResultFieldModel> resultFields = new ObservableCollection<ResultFieldModel>();

		public ResultTypeViewModel(ExecuteSQLDesigner executeSQLDesigner)
		{
			this.executeSQLDesigner = executeSQLDesigner;
			CreateDataFromSql = new DelegateCommand(CreateFromSQL, CanCreateFromSQL);
			Save = new DelegateCommand(ExecuteSave);

			selectedCustomType = executeSQLDesigner.ResultTypeValue.CustomType;

			foreach (var resultType in executeSQLDesigner.ResultTypeValue.Fields)
			{
				var model = new ResultFieldModel(this, resultType);
				model.ColumnNameChanged += ModelColumnNameChanged;
				model.NameChanged += ModelNameChanged;
				ResultFields.Add(model);
			}
			AddDefaultChild();
		}

		public ICommand CreateDataFromSql { get; private set; }
		public ICommand Save { get; private set; }

		public ObservableCollection<ResultFieldModel> ResultFields
		{
			get { return resultFields; }
			set
			{
				resultFields = value;
				if (!resultFields.Any(f => f.IsDefault))
					AddDefaultChild();
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

				if (ResultFields.All(f => f.IsDefault))
				{
					if (selectedCustomType != null)
					{
						var resultSetFields = new ObservableCollection<ResultFieldModel>();
						foreach (var nextProperty in selectedCustomType.GetProperties())
						{
							var field = new ResultFieldModel(this,
								new ResultTypeField
								{
									Name = nextProperty.Name,
									ColumnName = nextProperty.Name,
									TypeReference = nextProperty.TypeReference
								});
							field.ColumnNameChanged += ModelColumnNameChanged;
							field.NameChanged += ModelNameChanged;
							resultSetFields.Add(field);
						}
						ResultFields = resultSetFields;
					}
				}
				else
				{
					foreach (var nextField in ResultFields.Where(f => !f.IsDefault))
						ValidateName(nextField);
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

		internal void MoveTo(ResultFieldModel toMove, int destinationIndex)
		{
			ResultFields.Move(ResultFields.IndexOf(toMove), destinationIndex);
		}

		private void AddDefaultChild()
		{
			var defaultModel = new ResultFieldModel(this);
			defaultModel.ColumnNameChanged += DefaultModelChanged;
			defaultModel.NameChanged += DefaultModelChanged;
			ResultFields.Add(defaultModel);
		}

		private void ModelColumnNameChanged(object sender, NameChangedEventArgs e)
		{
			foreach (var nextField in ResultFields.Where(x => (((x.ColumnName ?? string.Empty).Equals(e.NewName ?? string.Empty, StringComparison.InvariantCultureIgnoreCase)) || ((x.ColumnName ?? string.Empty).Equals(e.OldName ?? string.Empty, StringComparison.InvariantCultureIgnoreCase))) && !x.IsDefault))
				ValidateColumnName(nextField);
		}

		private void ValidateColumnName(ResultFieldModel field)
		{
			var sameNameFields = ResultFields.Where(x => (x.ColumnName ?? string.Empty).Equals(field.ColumnName ?? string.Empty, StringComparison.InvariantCultureIgnoreCase) && !x.IsDefault);
			field.ColumnNameIsValid = string.IsNullOrWhiteSpace(field.ColumnName) || sameNameFields.Count() == 1;
		}

		private void ModelNameChanged(object sender, NameChangedEventArgs e)
		{
			foreach (var nextField in ResultFields.Where(f => (((f.Name ?? string.Empty) == (e.NewName ?? string.Empty)) || ((f.Name ?? string.Empty) == (e.OldName ?? string.Empty))) && !f.IsDefault))
				ValidateName(nextField);
		}

		private void ValidateName(ResultFieldModel field)
		{
			var sameNameFields = ResultFields.Where(f => (f.Name ?? string.Empty) == (field.Name ?? string.Empty) && !f.IsDefault);
			field.NameIsValid = string.IsNullOrWhiteSpace(field.Name) || (sameNameFields.Count() == 1 && Names.IsNameValid(field.Name) && (!IsMappingCustomType || CustomTypeProperties.Contains(field.Name)));
		}

		private void DefaultModelChanged(object sender, NameChangedEventArgs nameChangedEventArgs)
		{
			var model = (sender as ResultFieldModel);
			if (model == null)
				return;

			model.ColumnNameChanged -= DefaultModelChanged;
			model.NameChanged -= DefaultModelChanged;

			if (model.IsDefault)
			{
				model.IsDefault = false;

				if (!string.IsNullOrWhiteSpace(model.ColumnName))
					ModelColumnNameChanged(sender, nameChangedEventArgs);
				else
					ModelNameChanged(sender, nameChangedEventArgs);

				AddDefaultChild();
			}

			model.ColumnNameChanged += ModelColumnNameChanged;
			model.NameChanged += ModelNameChanged;
		}

		private void CreateFromSQL()
		{
			try
			{
				var connectionType = executeSQLDesigner.ResolvedConnectionType;
				string connectionString = executeSQLDesigner.ResolvedConnectionString;

				DataTable schemaTable = DatabaseHelpers.RetrieveSchema(connectionType, connectionString,
					SqlStringHandler.GetSqlStringHandler(this.executeSQLDesigner.ResolvedSqlStatementValue ?? string.Empty).GetExecutableDesignTimeSql());

				var newResults = new List<ResultFieldModel>();
				foreach (DataColumn dataColumn in schemaTable.Columns)
				{
					var oldModel = ResultFields.FirstOrDefault(x => x.ColumnName == dataColumn.ColumnName);
					if (oldModel != null)
					{
						newResults.Add(oldModel);
						continue;
					}

					ResultFieldModel newModel = new ResultFieldModel(this);
					newModel.IsDefault = false;
					newModel.ColumnName = dataColumn.ColumnName;
					newModel.Name = DatabaseHelpers.GetValidName(dataColumn.ColumnName);
					newModel.SetTypeReferenceValues(dataColumn.DataType);

					newModel.NameChanged += ModelNameChanged;
					newModel.ColumnNameChanged += ModelColumnNameChanged;
					newResults.Add(newModel);
				}

				ResultFields.Where(x => !newResults.Contains(x)).ToList().ForEach(x =>
					{
						x.NameChanged -= ModelNameChanged;
						x.ColumnNameChanged -= ModelColumnNameChanged;

					});
				ResultFields.Clear();

				newResults.ForEach(x => ResultFields.Add(x));

				AddDefaultChild();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool CanCreateFromSQL()
		{
			bool hasSql = !string.IsNullOrEmpty(executeSQLDesigner.ResolvedSqlStatementValue);
			bool hasConnectionString = !string.IsNullOrEmpty(executeSQLDesigner.ResolvedConnectionString);

			return (hasSql) && (hasConnectionString);
		}

		private void ExecuteSave()
		{
			string validationMessage;
			if (!ValidateFields(out validationMessage))
			{
				MessageBox.Show(validationMessage, "Save result set", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			var models = ResultFields.Where(f => (!f.IsDefault) && (!string.IsNullOrWhiteSpace(f.ColumnName))).ToList();

			var newResultType = new ResultType();
			models.ForEach(m => newResultType.Fields.Add(m.Save()));
			newResultType.CustomType = selectedCustomType;

			this.executeSQLDesigner.Context.TransactionManager.StartTransaction("Change result type");
			try
			{
				this.executeSQLDesigner.ResultTypeValue = newResultType;
				this.executeSQLDesigner.Context.TransactionManager.StopTransaction();
			}
			catch
			{
				this.executeSQLDesigner.Context.TransactionManager.RollbackTransaction();
				throw;
			}

			if (PostSave != null)
				PostSave();
		}

		private bool ValidateFields(out string message)
		{
			foreach (ResultFieldModel nextField in ResultFields.Where(f => (!f.IsDefault) && (!string.IsNullOrWhiteSpace(f.ColumnName))))
			{
				if (nextField.SelectedType == null)
				{
					message = "Please select a type for each field.";
					return false;
				}
				if ((!string.IsNullOrWhiteSpace(nextField.Name)) && (ResultFields.Any(f => (f != nextField) && (f.Name == nextField.Name))))
				{
					message = string.Format("Duplicate output name: {0}.", nextField.Name);
					return false;
				}

				if (IsMappingCustomType)
				{
					if (!nextField.NameIsValid)
					{
						message = string.Format("Invalid field name: {0}.", nextField.Name);
						return false;
					}
				}
				else
				{
					if (string.IsNullOrWhiteSpace(nextField.Name))
					{
						message = "Please enter an output name for each column.";
						return false;
					}
					if (!nextField.NameIsValid)
					{
						message = string.Format("Invalid output name {0}.", nextField.Name);
						return false;
					}
				}
			}

			message = null;
			return true;
		}

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
