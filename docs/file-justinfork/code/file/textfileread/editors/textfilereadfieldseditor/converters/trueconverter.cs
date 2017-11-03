using System;
using System.Globalization;
using System.Windows.Data;

namespace Twenty57.Linx.Components.File.TextFileRead.Editors.TextFileReadFieldsEditor.Converters
{
	public class TrueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return true;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}
