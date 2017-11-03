using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.UI.Editors.ConnectionEditor.ViewModel;

namespace Twenty57.Linx.Components.Database.UI.Editors.ConnectionEditor
{
	public partial class ConnectionEditorWindow
	{
		private int nameSelectionStart, nameSelectionLength;

		private ConnectionEditorWindow(ConnectionViewModel viewModel)
		{
			InitializeComponent();
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

		public static bool EditConnectionString(ref ConnectionType connectionType, ref string connectionString, ConnectionType[] allowedConnectionTypes = null)
		{
			ConnectionViewModel viewModel = new ConnectionViewModel(connectionType, connectionString);
			if (allowedConnectionTypes != null)
				viewModel.AllowedConnectionTypes = allowedConnectionTypes;
			if (Display(viewModel))
			{
				connectionType = viewModel.ConnectionType;
				connectionString = viewModel.ConnectionString;
				return true;
			}
			return false;
		}

		private static bool Display(ConnectionViewModel viewModel)
		{
			var window = new ConnectionEditorWindow(viewModel);
			window.Owner = Application.Current.MainWindow;
			window.Loaded += (sender, args) => viewModel.SetWindowHandle(new WindowInteropHelper(sender as Window).Handle); ;
			return window.ShowDialog() ?? false;
		}

		private void ListView_KeyUp(object sender, KeyEventArgs e)
		{
			ListView listView = sender as ListView;
			if ((listView != null) && (listView.SelectedItem != null) && (FocusManager.GetFocusedElement(this) is ListViewItem) && (e.Key == Key.Delete))
			{
				var item = listView.SelectedItem as ConnectionParameter;
				if (item != null)
					item.Remove();
			}
		}

		private void LabeledNamePanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			foreach (UIElement nextElement in ((StackPanel)sender).Children)
				if (nextElement is TextBox)
				{
					TextBox nameTextBox = (TextBox)nextElement;
					nameSelectionStart = nameTextBox.SelectionStart;
					nameSelectionLength = nameTextBox.SelectionLength;
					return;
				}
		}

		private void NameTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			TextBox nameTextBox = (TextBox)sender;
			if (nameTextBox.Visibility == Visibility.Visible)
			{
				nameTextBox.Focus();
				nameTextBox.SelectionStart = nameSelectionStart;
				nameTextBox.SelectionLength = nameSelectionLength;
			}
		}

		private void viewModel_ShowMessage(object sender, ShowMessageEventArgs args)
		{
			args.MessageResponse = MessageBox.Show(this, args.Text, args.Caption, args.Options, args.MessageType);
		}

		private void ListView_Loaded(object sender, RoutedEventArgs e)
		{
			ListView listView = (sender as ListView);
			listView.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}

		private void ConnectionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ParameterListView.SelectedIndex = 0;
			ParameterListView.ScrollIntoView(ParameterListView.SelectedItem);
			ParameterListView.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}

		private void TextBox_GotKeyboardFocus(object sender, RoutedEventArgs e)
		{
			((TextBox)sender).SelectAll();
		}
	}
}
