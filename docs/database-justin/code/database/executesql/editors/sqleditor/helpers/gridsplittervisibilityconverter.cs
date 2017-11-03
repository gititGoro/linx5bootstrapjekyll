using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers
{
	public class GridSplitterVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool hasQueryExecuted = (bool)values[0];
			int parameterCount = (int)values[1];
			bool beVisible = (hasQueryExecuted) || (parameterCount > 5);
			return beVisible ? Visibility.Visible : Visibility.Hidden;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}
