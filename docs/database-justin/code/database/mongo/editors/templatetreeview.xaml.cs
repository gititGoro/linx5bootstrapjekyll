using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor;

namespace Twenty57.Linx.Components.MongoDB.Editors
{
	public partial class TemplateTreeView : TabItem
	{
		private ITemplateSource source;

		public TemplateTreeViewModel TemplateTreeViewModel { get; protected set; }


		public TemplateTreeView(ITemplateSource source)
		{
			Header = source.Name;
			this.source = source;
			TemplateTreeViewModel = new TemplateTreeViewModel(source);
			TemplateTreeViewModel.Load();
			base.DataContext=TemplateTreeViewModel;

			InitializeComponent();
		}

		public static DependencyObject FindParent(DependencyObject child, Type parentType)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(child);
			if (parent == null) return null;

			if (parent.GetType() == parentType)
			{
				return parent;
			}
			else
			{
				return FindParent(parent, parentType);
			}
		}
		public void OnLostFocus(object sender, RoutedEventArgs e)
		{
			if ((sender as TreeViewItem) == null)
			{
				TemplateTreeItemViewModel item = TreeView.SelectedItem as TemplateTreeItemViewModel;
				if (item != null)
				{
					item.IsSelected = false;
				}
			}
		}

		public bool Loading { get; set; }
	}
}
