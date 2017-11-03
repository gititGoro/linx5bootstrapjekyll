using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ResultSetEditor.ViewModel;
using Twenty57.Linx.Components.Database.UI;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ResultSetEditor
{
	public partial class ResultSetEditorWindow
	{
		private ResultSetEditorWindow(ResultSetViewModel model, IDesignerContext designerContext)
		{
			InitializeComponent();

			DataContext = model;
			model.ShowMessage += model_ShowMessage;
		}

		protected override bool PersistLayout
		{
			get
			{
				return true;
			}
		}

		public static bool Display(ResultSetViewModel model, IDesignerContext context)
		{
			var window = new ResultSetEditorWindow(model, context);
			window.Owner = Application.Current.MainWindow;
			return window.ShowDialog() ?? false;
		}

		private void ListView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				ResultSetFieldModel selectedItem = ((ListView)sender).SelectedItem as ResultSetFieldModel;
				if ((selectedItem != null) && (!selectedItem.IsDefault))
					selectedItem.Remove();
			}
		}

		private void TextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			textBox.Tag = textBox.IsKeyboardFocused;
		}

		private void model_ShowMessage(object sender, ShowMessageEventArgs args)
		{
			args.MessageResponse = MessageBox.Show(this, args.Text, args.Caption, args.Options, args.MessageType);
		}

		private void ListView_Loaded(object sender, RoutedEventArgs e)
		{
			ListView listView = sender as ListView;
			listView.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}
		private void TextBox_GotKeyboardFocus(object sender, RoutedEventArgs e)
		{
			((TextBox)sender).SelectAll();
		}
	}
}
