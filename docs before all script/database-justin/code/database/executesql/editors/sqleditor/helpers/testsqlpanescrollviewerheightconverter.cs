using System;
using System.Globalization;
using System.Windows.Data;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers
{
	public class TestSqlPaneScrollViewerHeightConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var height = (double)value;
			return height < 20 ? height : height - 25;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
