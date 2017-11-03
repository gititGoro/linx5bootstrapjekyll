using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.UI;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ResultSetEditor.ViewModel
{
	public class ResultSetFieldModel : INotifyPropertyChanged, DragDropController.IDraggableRowItem
	{
		private bool isDragged = false;
		private ResultSetViewModel resultSetViewModel;

		public ResultSetFieldModel(ResultSetViewModel resultSetViewModel) :
			this(resultSetViewModel, new DatabaseModel.ResultSetField(), true) { }

		public ResultSetFieldModel(ResultSetViewModel resultSetViewModel, DatabaseModel.ResultSetField resultSetField) :
			this(resultSetViewModel, (DatabaseModel.ResultSetField)resultSetField.Clone(), false) { }

		private ResultSetFieldModel(ResultSetViewModel resultSetViewModel, DatabaseModel.ResultSetField resultSetField, bool isDefault)
		{
			this.resultSetViewModel = resultSetViewModel;
			resultSetViewModel.PropertyChanged += resultSetViewModel_PropertyChanged;
			ResultSetField = resultSetField;
			IsDefault = isDefault;

			DragDropController = new DragDropController(this);
		}

		public DatabaseModel.ResultSetField ResultSetField { get; private set; }

		public DragDropController DragDropController { get; private set; }

		public bool IsDefault { get; private set; }

		public string ColumnName
		{
			get { return ResultSetField.ColumnName; }
			set
			{
				ResultSetField.ColumnName = value;
				NotifyPropertyChanged();
			}
		}

		public EnumWrapper<DatabaseModel.DataType> DataType
		{
			get { return ResultSetField.DataType; }
			set
			{
				ResultSetField.DataType = value;
				NotifyPropertyChanged();
			}
		}

		public IEnumerable<EnumWrapper<DatabaseModel.DataType>> DataTypes
		{
			get
			{
				return ((DatabaseModel.DataType[])Enum.GetValues(typeof(DatabaseModel.DataType))).Select(t => new EnumWrapper<DatabaseModel.DataType>(t));
			}
		}

		public string OutputName
		{
			get { return ResultSetField.OutputName; }
			set
			{
				ResultSetField.OutputName = value;
				NotifyPropertyChanged("OutputName");
				NotifyPropertyChanged("OutputNameIsValid");
			}
		}

		public bool OutputNameIsValid
		{
			get 
			{ 
				return (IsDefault) || (resultSetViewModel.IsMappingCustomType ? resultSetViewModel.CustomTypeProperties.Contains(OutputName) : Names.IsNameValid(OutputName));
			}
		}

		public object ColumnNameIsFocused
		{
			set
			{
				if ((value is bool) && (!(bool)value) && (string.IsNullOrEmpty(OutputName)) && (resultSetViewModel.SelectedCustomType == null) && (!string.IsNullOrEmpty(ColumnName)))
					OutputName = Names.GetValidName(ColumnName);
			}
		}

		public void Remove()
		{
			resultSetViewModel.Remove(this);
		}

		#region DragDropController.IDraggableRowItem
		public bool SupportsDragDrop
		{
			get { return !IsDefault; }
		}

		public bool IsDragged
		{
			get { return isDragged; }
			set
			{
				isDragged = value;
				NotifyPropertyChanged();
			}
		}

		public int RowIndex
		{
			get { return resultSetViewModel.ResultSetFields.IndexOf(this); }
		}

		public void MoveToIndex(int index)
		{
			Application.Current.Dispatcher.Invoke(() => resultSetViewModel.MoveTo(this, index));
		}
		#endregion

		private void resultSetViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsMappingCustomType")
				NotifyPropertyChanged("OutputNameIsValid");
		}

		#region INotifyPropertyChanged
		public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

			if ((IsDefault) && (propertyName == "ColumnName"))
			{
				IsDefault = false;
				resultSetViewModel.AddDefaultField();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
