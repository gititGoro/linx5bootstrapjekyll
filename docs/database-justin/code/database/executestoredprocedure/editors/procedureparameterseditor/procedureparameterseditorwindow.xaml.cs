using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor.ViewModel;
using Twenty57.Linx.Components.Database.UI;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor
{
	public partial class ProcedureParametersEditorWindow
	{
		private ProcedureParametersEditorWindow(ProcedureParametersViewModel model)
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

		public static bool Display(ProcedureParametersViewModel model)
		{
			var window = new ProcedureParametersEditorWindow(model);
			window.Owner = Application.Current.MainWindow;
			return window.ShowDialog() ?? false;
		}

		private void ListView_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				ProcedureParameterModel selectedItem = ((ListView)sender).SelectedItem as ProcedureParameterModel;
				if ((selectedItem != null) && (!selectedItem.IsDefault))
					selectedItem.Remove();
			}
		}

		private void model_ShowMessage(object sender, ShowMessageEventArgs args)
		{
			args.MessageResponse = MessageBox.Show(this, args.Text, args.Caption, args.Options, args.MessageType);
		}

		private void ListView_Loaded(object sender, RoutedEventArgs e)
		{
			MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}
		private void TextBox_GotKeyboardFocus(object sender, RoutedEventArgs e)
		{
			((TextBox)sender).SelectAll();
		}
	}
}
