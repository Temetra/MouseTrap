using System;
using System.Drawing;
using System.IO;
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
			else if (CheckPathIsValid(processPath))
			{
				// Get icon and cache before returning
				using (var ico = Icon.ExtractAssociatedIcon(processPath))
				{
					result = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
					AddToCache(processPath, result);
					return result;
				}
			}


			// Not found, return default icon
			return DefaultIcon;
		}

		private static bool CheckPathIsValid(string path)
		{
			return !string.IsNullOrWhiteSpace(path) &&	// Not null or empty
				!path.StartsWith("\\") &&				// Not UNC path
				File.Exists(path);						// File exists
		}

		private static void AddToCache(string processPath, BitmapSource result)
		{
			CacheItemPolicy policy = new CacheItemPolicy
			{
				SlidingExpiration = TimeSpan.FromMinutes(10)
			};

			IconCache.Set(processPath, result, policy);
		}
	}
}
