using MouseTrap.Data;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MouseTrap.Binding
{
	public class DimensionsToThicknessConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var dimensions = (Dimensions)value;

			double left = GetMargin(dimensions.Left);
			double top = GetMargin(dimensions.Top);
			double right = GetMargin(dimensions.Right);
			double bottom = GetMargin(dimensions.Bottom);

			return new Thickness(left, top, right, bottom);
		}

		private static double GetMargin(double value)
		{
			if (value > 0) return 16;		// inner margin
			else if (value < 0) return 2;	// outer margin
			else return 8;					// no margin
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
