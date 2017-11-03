using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel
{
	public abstract class NodeViewModel : INotifyPropertyChanged
	{
		private bool isExpanded;
		private bool isSelected;

		public NodeViewModel(NodeViewModel parent = null)
		{
			Parent = parent;

			Text = String.Empty;
			Children = new ObservableCollection<NodeViewModel>();
			AllowDrag = false;
			SimpleDragText = String.Empty;
			ExtendedDragText = String.Empty;
			this.isExpanded = false;
			this.isSelected = false;
		}

		public NodeViewModel Parent { get; private set; }
		public string Text { get; protected set; }
		public ObservableCollection<NodeViewModel> Children { get; private set; }

		public bool AllowDrag { get; protected set; }

		protected string SimpleDragText { private get; set; }
		protected string ExtendedDragText { private get; set; }

		public virtual bool IsExpanded
		{
			get { return this.isExpanded; }
			set
			{
				if (value != this.isExpanded)
				{
					this.isExpanded = value;
					OnPropertyChanged("IsExpanded");
				}

				if (this.isExpanded && null != Parent)
					Parent.IsExpanded = true;
			}
		}

		public bool IsSelected
		{
			get { return this.isSelected; }
			set
			{
				if (value != this.isSelected)
				{
					this.isSelected = value;
					OnPropertyChanged("IsSelected");
				}
			}
		}

		public string GetSimpleDragText()
		{
			if (!AllowDrag)
				throw new Exception("Drag is not allowed.");

			return SimpleDragText;
		}

		public string GetExtendedDragText()
		{
			if (!AllowDrag)
				throw new Exception("Drag is not allowed.");

			return ExtendedDragText;
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
