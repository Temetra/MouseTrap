using System;
using System.Globalization;
using System.Windows.Data;

namespace MouseTrap.Binding
{
	public class MergeTitleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var subtitle = (string)value;
			var title = (string)parameter;
			if (string.IsNullOrWhiteSpace(subtitle)) return title;
			return $"{title} - {subtitle}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
