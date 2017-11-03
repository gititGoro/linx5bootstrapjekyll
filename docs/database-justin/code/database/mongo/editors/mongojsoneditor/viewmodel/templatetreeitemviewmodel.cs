using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.ViewModel
{
	public class TemplateTreeItemViewModel : INotifyPropertyChanged
	{
		private bool isSelected;
		private bool isExpanded;

		public TemplateTreeItemViewModel(TemplateTreeItemViewModel parent = null)
		{
			Parent = parent;

			Text = String.Empty;
			Children = new ObservableCollection<TemplateTreeItemViewModel>();

			isSelected = false;
			IsExpanded = false;
		}

		public TemplateTreeItemViewModel(string name, TemplateTreeItemViewModel parent = null) : this(parent)
		{
			Text = name;
			IsExpanded = Text.Contains("Samples");
		}

		public TemplateTreeItemViewModel(Template template, TemplateTreeItemViewModel parent) : this(parent)
		{
			Template = template;
			Text = template.Name;
			ToolTipText = template.Description;
		}

		public bool IsExpanded
		{
			get { return isExpanded; }
			set
			{
				if (value != isExpanded)
				{
					isExpanded = value;
					OnPropertyChanged("IsExpanded");
				}

				if (isExpanded && null != Parent)
					Parent.IsExpanded = true;
			}
		}


		public TemplateTreeItemViewModel Parent { get; private set; }
		public string Text { get; protected set; }
		public ObservableCollection<TemplateTreeItemViewModel> Children { get; private set; }

		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				if (value != isSelected)
				{
					isSelected = value;
					OnPropertyChanged("IsSelected");
				}
			}
		}

		public string ToolTipText { get; protected set; }

		public bool AllowDrag
		{
			get { return Template != null; }
		}

		public Template Template { get; set; }

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}