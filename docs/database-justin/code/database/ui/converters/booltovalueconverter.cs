using System;
using System.Globalization;
using System.Windows.Data;

namespace Twenty57.Linx.Components.Database.UI.Converters
{
	public class BoolToValueConverter : IValueConverter
	{
		public object FalseValue { get; set; }
		public object TrueValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? TrueValue : FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}
