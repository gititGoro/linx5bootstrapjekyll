using System;
using System.Windows;
using System.Windows.Data;

namespace Twenty57.Linx.Components.Database.UI.Converters
{
	[ValueConversion(typeof(double), typeof(GridLength))]
	public class WidthChangeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (double)value + double.Parse(parameter.ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
