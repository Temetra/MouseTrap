using System;
using System.Globalization;
using System.Windows.Data;
using MouseTrap.UserInterface;

namespace MouseTrap.Binding
{
	public class ViewTypeToBoolParameter
	{
		public ViewType ViewType { get; set; }
		public bool Inverted { get; set; } = false;
	}
	
	public class ViewTypeToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var currentView = (ViewType)value;
			if (!(parameter is ViewTypeToBoolParameter pam)) return false;
			return (currentView == pam.ViewType) ^ pam.Inverted;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
