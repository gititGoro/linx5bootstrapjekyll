using System.Windows;
using System.Windows.Controls;
using Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser.ViewModel;
using Twenty57.Linx.Components.Database.UI;
using Twenty57.Linx.Components.Database.UI.Controls.ConnectionControl;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser
{
	public partial class TableChooserWindow
	{
		private TableChooserWindow(EditingInfo editingInfo)
		{
			InitializeComponent();
			var viewModel = new TableChooserViewModel(editingInfo, (ConnectionViewModel)ConnectionControl.DataContext);
			DataContext = viewModel;
			viewModel.ShowMessage += viewModel_ShowMessage;
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
			var window = new TableChooserWindow(editingInfo);
			window.Owner = Application.Current.MainWindow;
			return window.ShowDialog() ?? false;
		}

		private void viewModel_ShowMessage(object sender, ShowMessageEventArgs args)
		{
			args.MessageResponse = MessageBox.Show(this, args.Text, args.Caption, args.Options, args.MessageType);
		}


		#region http://stackoverflow.com/questions/13629650/how-to-bind-a-datatrigger-to-the-computedverticalscrollbarvisibility-property-of
		private static readonly DependencyPropertyKey ListBoxScrollViewerPropertyKey = DependencyProperty.RegisterReadOnly("ListBoxScrollViewer", typeof(ScrollViewer), typeof(TableChooserWindow), new PropertyMetadata());
		protected static readonly DependencyProperty ListBoxScrollViewerProperty = ListBoxScrollViewerPropertyKey.DependencyProperty;

		protected ScrollViewer ListBoxScrollViewer
		{
			get { return (ScrollViewer)GetValue(ListBoxScrollViewerProperty); }
			private set { SetValue(ListBoxScrollViewerPropertyKey, value); }
		}

		private void ColumnsListView_Loaded(object sender, RoutedEventArgs e)
		{
			if (ReferenceEquals(e.OriginalSource, ColumnsListView))
			{
				var scrollViewer = ColumnsListView.GetFirstDescendantBreadthFirst<ScrollViewer>();
				if (scrollViewer != null)
				{
					ListBoxScrollViewer = scrollViewer;
				}
			}
		}
		#endregion
	}
}
