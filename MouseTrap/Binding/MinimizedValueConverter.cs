using System;
using System.Globalization;
using System.Windows.Data;

namespace MouseTrap.Binding
{
	public class MinimizedValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var isMinimized = (bool)value;
			if (isMinimized) return "Minimized windows";
			return "Active windows";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
