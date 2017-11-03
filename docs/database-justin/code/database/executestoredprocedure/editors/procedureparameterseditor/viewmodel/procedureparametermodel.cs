using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.UI;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor.ViewModel
{
	public class ProcedureParameterModel : INotifyPropertyChanged, DragDropController.IDraggableRowItem
	{
		private bool isDragged = false;
		private ProcedureParametersViewModel procedureParametersViewModel;

		public ProcedureParameterModel(ProcedureParametersViewModel procedureParametersViewModel) :
			this(procedureParametersViewModel, new DatabaseModel.ProcedureParameter(), true) {}

		public ProcedureParameterModel(ProcedureParametersViewModel procedureParametersViewModel, DatabaseModel.ProcedureParameter procedureParameter) :
			this(procedureParametersViewModel, (DatabaseModel.ProcedureParameter)procedureParameter.Clone(), false) { }

		private ProcedureParameterModel(ProcedureParametersViewModel procedureParametersViewModel, DatabaseModel.ProcedureParameter procedureParameter, bool isDefault)
		{
			this.procedureParametersViewModel = procedureParametersViewModel;
			ProcedureParameter = procedureParameter;
			IsDefault = isDefault;

			DragDropController = new DragDropController(this);
		}

		public DatabaseModel.ProcedureParameter ProcedureParameter { get; private set; }

		public DragDropController DragDropController { get; private set; }

		public bool IsDefault { get; private set; }

		public string Name 
		{ 
			get { return ProcedureParameter.Name; } 
			set 
			{ 
				ProcedureParameter.Name = value;
				NotifyPropertyChanged();
			}
		}

		public EnumWrapper<DatabaseModel.ParameterDirection> Direction
		{
			get { return ProcedureParameter.Direction; }
			set 
			{ 
				ProcedureParameter.Direction = value;
				NotifyPropertyChanged("Direction");
				NotifyPropertyChanged("RequiresSize");
			}
		}

		public EnumWrapper<DatabaseModel.DataType> DataType
		{
			get { return ProcedureParameter.DataType; }
			set 
			{ 
				ProcedureParameter.DataType = value;
				NotifyPropertyChanged("DataType");
				NotifyPropertyChanged("RequiresSize");
			}
		}

		public int Size
		{
			get 
			{
				if (ProcedureParameter.Size.HasValue)
					return ProcedureParameter.Size.Value;
				DatabaseModel.RequiresSize[] requiresSizeAttributes = TypeHelpers.GetEnumerationValueAttributes<DatabaseModel.RequiresSize>(ProcedureParameter.DataType);
				return requiresSizeAttributes.Length == 0 ? 10 : requiresSizeAttributes[0].DefaultColumnSize;
			}
			set
			{
				ProcedureParameter.Size = value;
				NotifyPropertyChanged();
			}
		}

		public bool RequiresSize
		{
			get { return ProcedureParameter.RequiresSize; }
		}


		public IEnumerable<EnumWrapper<DatabaseModel.ParameterDirection>> Directions
		{
			get
			{
				return ((DatabaseModel.ParameterDirection[])Enum.GetValues(typeof(DatabaseModel.ParameterDirection))).Select(d => new EnumWrapper<DatabaseModel.ParameterDirection>(d));
			}
		}

		public IEnumerable<EnumWrapper<DatabaseModel.DataType>> DataTypes
		{
			get
			{
				return ((DatabaseModel.DataType[])Enum.GetValues(typeof(DatabaseModel.DataType))).Select(t => new EnumWrapper<DatabaseModel.DataType>(t));
			}
		}

		public void Remove()
		{
			procedureParametersViewModel.Remove(this);
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
			get { return procedureParametersViewModel.Parameters.IndexOf(this); }
		}

		public void MoveToIndex(int index)
		{
			Application.Current.Dispatcher.Invoke(() => procedureParametersViewModel.MoveTo(this, index));
		}
		#endregion

		#region INotifyPropertyChanged
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

			if ((IsDefault) && (propertyName == "Name"))
			{
				IsDefault = false;
				procedureParametersViewModel.AddDefaultParameter();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
