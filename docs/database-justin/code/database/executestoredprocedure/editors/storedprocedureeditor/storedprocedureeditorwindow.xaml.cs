using System;
using System.Windows;
using System.Windows.Controls;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.StoredProcedureEditor.ViewModel;
using Twenty57.Linx.Components.Database.UI;
using Twenty57.Linx.Components.Database.UI.Controls.ConnectionControl;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.StoredProcedureEditor
{
	public partial class StoredProcedureEditorWindow
	{
		private StoredProcedureEditorWindow(EditingInfo editingInfo)
		{
			InitializeComponent();

			DataContext = new StoredProcedureViewModel(editingInfo, (ConnectionViewModel)ConnectionControl.DataContext);
		}

		protected override bool PersistLayout
		{
			get
			{
				return true;
			}
		}

		public static bool Display(EditingInfo editingInfo)
		{
			var window = new StoredProcedureEditorWindow(editingInfo);
			window.Owner = Application.Current.MainWindow;
			return window.ShowDialog() ?? false;
		}

		private void StoredProceduresComboBox_DropDownOpened(object sender, EventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			int selectedIndex = comboBox.SelectedIndex;
			comboBox.SelectedIndex = 0;
			comboBox.SelectedIndex = selectedIndex;
		}


		#region http://stackoverflow.com/questions/13629650/how-to-bind-a-datatrigger-to-the-computedverticalscrollbarvisibility-property-of
		private static readonly DependencyPropertyKey ListBoxScrollViewerPropertyKey = DependencyProperty.RegisterReadOnly("ListBoxScrollViewer", typeof(ScrollViewer), typeof(StoredProcedureEditorWindow), new PropertyMetadata());
		protected static readonly DependencyProperty ListBoxScrollViewerProperty = ListBoxScrollViewerPropertyKey.DependencyProperty;

		protected ScrollViewer ListBoxScrollViewer
		{
			get { return (ScrollViewer)GetValue(ListBoxScrollViewerProperty); }
			private set { SetValue(ListBoxScrollViewerPropertyKey, value); }
		}

		private void ParametersListView_Loaded(object sender, RoutedEventArgs e)
		{
			if (ReferenceEquals(e.OriginalSource, ParametersListView))
			{
				var scrollViewer = ParametersListView.GetFirstDescendantBreadthFirst<ScrollViewer>();
				if (scrollViewer != null)
				{
					ListBoxScrollViewer = scrollViewer;
				}
			}
		}
		#endregion
	}
}
