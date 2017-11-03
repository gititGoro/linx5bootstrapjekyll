using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.UI;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.ResultTypeEditor.ViewModel
{
	public class ResultFieldModel : INotifyPropertyChanged, DragDropController.IDraggableRowItem
	{
		private ResultTypeViewModel resultTypeViewModel;
		private ResultTypeField originalField;
		private bool columnNameIsValid;
		private bool nameIsValid;
		private string columnName;
		private string name;
		private bool isDefault;
		private bool isDragged = false;

		private ITypeReference selectedType, selectedElementType;

		public ResultFieldModel(ResultTypeViewModel resultTypeViewModel)
		{
			this.resultTypeViewModel = resultTypeViewModel;
			originalField = new ResultTypeField();

			IsDefault = true;
			columnNameIsValid = true;
			nameIsValid = true;
			this.selectedType = TypeHelpers.MapType(typeof(string));
			this.selectedElementType = null;

			DragDropController = new DragDropController(this);
		}

		public ResultFieldModel(ResultTypeViewModel resultTypeViewModel, ResultTypeField resultTypeField) :
			this(resultTypeViewModel)
		{
			originalField = resultTypeField;

			ColumnName = resultTypeField.ColumnName;
			Name = resultTypeField.Name;

			if (resultTypeField.TypeReference != null)
			{
				selectedType = resultTypeField.TypeReference;
				selectedElementType = (selectedType.IsList) ? resultTypeField.TypeReference.GetEnumerableContentType() : null;
			}

			IsDefault = false;
			nameIsValid = true;
			columnNameIsValid = true;
		}

		public DragDropController DragDropController { get; private set; }

		public ITypeReference SelectedType
		{
			get { return selectedType; }
			set
			{
				selectedType = value;
				OnPropertyChanged("SelectedType");
				OnPropertyChanged("DisplayElementTypeSelector");
			}
		}

		public ITypeReference SelectedElementType
		{
			get { return selectedElementType; }
			set
			{
				selectedElementType = value;
				OnPropertyChanged();
			}
		}

		public bool DisplayElementTypeSelector
		{
			get { return (selectedType != null) && (selectedType.IsList); }
		}

		public string ColumnName
		{
			get { return columnName; }
			set
			{
				var oldName = columnName;
				columnName = value;
				OnPropertyChanged();
				if (ColumnNameChanged != null)
					ColumnNameChanged(this, new NameChangedEventArgs(oldName, columnName));
			}
		}

		public string Name
		{
			get { return name; }
			set
			{
				var oldName = name;
				name = value;
				OnPropertyChanged();
				if (NameChanged != null)
					NameChanged(this, new NameChangedEventArgs(oldName, name));
			}
		}

		public object ColumnNameIsFocused
		{
			set
			{
				if ((value is bool) && (!(bool)value) && (string.IsNullOrEmpty(Name)) && (resultTypeViewModel.SelectedCustomType == null) && (!string.IsNullOrEmpty(ColumnName)))
                    Name = Names.GetValidName(ColumnName);
			}
		}

		public bool IsDefault
		{
			get { return isDefault; }
			set
			{
				isDefault = value;
				OnPropertyChanged();
			}
		}

		public bool ColumnNameIsValid
		{
			get { return IsDefault || columnNameIsValid; }
			set
			{
				columnNameIsValid = value;
				OnPropertyChanged();
			}
		}

		public bool NameIsValid
		{
			get { return IsDefault || nameIsValid; }
			set
			{
				nameIsValid = value;
				OnPropertyChanged();
			}
		}

		public event EventHandler<NameChangedEventArgs> ColumnNameChanged;
		public event EventHandler<NameChangedEventArgs> NameChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		public ResultTypeField Save()
		{
			originalField.ColumnName = columnName;
			originalField.Name = name;
			originalField.TypeReference = selectedType.IsList ? TypeReference.CreateList(selectedElementType) : selectedType;
			return originalField;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
				OnPropertyChanged();
			}
		}

		public int RowIndex
		{
			get { return resultTypeViewModel.ResultFields.IndexOf(this); }
		}

		public void MoveToIndex(int index)
		{
			Application.Current.Dispatcher.Invoke(() => resultTypeViewModel.MoveTo(this, index));
		}
		#endregion

		internal void SetTypeReferenceValues(Type type)
		{
			var typeReference = TypeHelpers.MapType(type);

			if (typeReference != null)
			{
				SelectedType = typeReference;
				SelectedElementType = (SelectedType.IsList) ? typeReference.GetEnumerableContentType() : null;
			}
		}
	}

	public class NameChangedEventArgs : EventArgs
	{
		private string oldName;
		private string newName;
		private bool accept = true;

		public string OldName
		{
			get { return oldName; }
		}

		public string NewName
		{
			get { return newName; }
		}

		public bool Accept
		{
			get { return accept; }
			set { accept = value; }
		}

		public NameChangedEventArgs(string oldName, string newName)
		{
			this.oldName = oldName;
			this.newName = newName;
		}
	}
}
