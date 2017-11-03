using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.ExecuteSQL.Editors.ResultTypeEditor.ViewModel;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.ResultTypeEditor
{
	public partial class ResultTypeEditorWindow
	{
		public static bool Display(ResultTypeViewModel viewModel)
		{
			var window = new ResultTypeEditorWindow(viewModel);
			viewModel.PostSave += window.Context_PostSave;

			window.Owner = Application.Current.MainWindow;

			return window.ShowDialog() ?? false;
		}


		private ResultTypeEditorWindow(ResultTypeViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}

		private void Context_PostSave()
		{
			DialogResult = true;
			Close();
		}

		private void VariableListKeyPress(object sender, KeyEventArgs e)
		{
			var listView = sender as ListView;

			if (listView != null && listView.SelectedItem != null && e.Key == Key.Delete)
			{
				var focusedElement = FocusManager.GetFocusedElement(this);

				if (!(focusedElement is ListViewItem))
					return;

				var item = listView.SelectedItem as ResultFieldModel;
				if (item == null || item.IsDefault)
					return;

				var viewModel = DataContext as ResultTypeViewModel;
				viewModel.ResultFields.Remove(item);
			}
		}

		private void TextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			textBox.Tag = textBox.IsKeyboardFocused;
		}

		private void DisplayTree_Loaded(object sender, RoutedEventArgs e)
		{
			DisplayTree.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}
		private void TextBox_GotKeyboardFocus(object sender, RoutedEventArgs e)
		{
			((TextBox)sender).SelectAll();
		}
	}
}
