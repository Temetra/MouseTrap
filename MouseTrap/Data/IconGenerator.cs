using System;
using System.Windows.Media.Imaging;

namespace MouseTrap.Data
{
	public static class IconGenerator
	{
		public static Func<string, BitmapSource> GetIcon { get; set; } = LiveIconGenerator.GetIcon;
	}
}
