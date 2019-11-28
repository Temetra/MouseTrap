using System;
using System.Drawing;
using System.Runtime.Caching;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MouseTrap.Data
{
	public static class LiveIconGenerator
	{
		private static readonly BitmapImage DefaultIcon = new BitmapImage(new Uri("/MouseTrap;component/Resources/DefaultListIcon.png", UriKind.Relative));
		private static readonly MemoryCache IconCache = MemoryCache.Default;

		public static BitmapSource GetIcon(string processPath)
		{
			if (IconCache[processPath] is BitmapSource result)
			{
				// Return cached value
				return result;
			}
			else if (processPath != null && processPath.Length > 0 && System.IO.File.Exists(processPath))
			{
				// Get icon and cache before returning
				using (var ico = Icon.ExtractAssociatedIcon(processPath))
				{
					result = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

					CacheItemPolicy policy = new CacheItemPolicy
					{
						SlidingExpiration = TimeSpan.FromMinutes(10)
					};

					IconCache.Set(processPath, result, policy);
					
					return result;
				}
			}
			else
			{
				// Not found, return default icon
				return DefaultIcon;
			}
		}
	}
}
