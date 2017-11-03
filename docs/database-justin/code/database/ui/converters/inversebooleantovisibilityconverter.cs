using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Twenty57.Linx.Components.Database.UI.Converters
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class InverseBooleanToVisibilityConverter : IValueConverter
	{
		private static readonly BooleanToVisibilityConverter instance;

		static InverseBooleanToVisibilityConverter()
		{
			instance = new BooleanToVisibilityConverter();
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return instance.Convert(!(bool)value, targetType, parameter, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
