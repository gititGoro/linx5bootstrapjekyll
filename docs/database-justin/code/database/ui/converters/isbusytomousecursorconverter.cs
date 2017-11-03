using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace Twenty57.Linx.Components.Database.UI.Converters
{
	[ValueConversion(typeof(bool), typeof(Cursor))]
	public class IsBusyToMouseCursorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? Cursors.Wait : Cursors.Arrow;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}
