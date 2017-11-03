using System.Windows;
using System.Windows.Input;

namespace Twenty57.Linx.Components.Database.UI.Controls.ConnectionControl
{
	public partial class ConnectionControl
	{
		private static readonly DependencyPropertyKey LabelColumnWidthPropertyKey = DependencyProperty.RegisterReadOnly("LabelColumnWidth", typeof(double), typeof(ConnectionControl), new PropertyMetadata());
		protected static readonly DependencyProperty LabelColumnWidthProperty = LabelColumnWidthPropertyKey.DependencyProperty;

		public ConnectionControl()
		{
			InitializeComponent();
			DataContext = new ConnectionViewModel();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			Keyboard.Focus(ConnectionStringComboBox);
			SetValue(LabelColumnWidthPropertyKey, parameterGrid.ColumnDefinitions[0].ActualWidth);
		}
	}
}
