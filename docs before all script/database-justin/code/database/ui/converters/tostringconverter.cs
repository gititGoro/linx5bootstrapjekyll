using System;
using System.Windows.Data;

namespace Twenty57.Linx.Components.Database.UI.Converters
{
	[ValueConversion(typeof(object), typeof(string))]
	public class ToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value == null ? string.Empty : value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
