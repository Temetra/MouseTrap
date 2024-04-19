using Microsoft.Extensions.Caching.Memory;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Drawing.Imaging;
using System.IO;
using Temetra.Windows;

namespace MouseTrap.Services;

internal class IconService
{
    private static readonly Uri defaultImageUri = new("ms-appx:///Assets/StoreLogo.png");
    private static readonly BitmapImage defaultImage = new(defaultImageUri);
    public static BitmapImage DefaultImage => defaultImage;

    private readonly IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

    public BitmapImage GetBitmapImage(string path, string image)
    {
        var filename = ThumbnailProvider.GetPackageThumbnailPath(path, image);

        if (CheckPathIsValid(filename))
        {
            var key = filename.ToLower();
            var isCached = cache.TryGetValue(key, out BitmapImage result);

            if (!isCached)
            {
                Core.Log.Logger.Information("Fetching image for {Path}, {Image}", path, image);
                result = new BitmapImage(new Uri(filename));
                cache.Set(key, result);
            }

            return result;
        }
        else
        {
            Core.Log.Logger.Error("Failed to find image for {Path}, {Image}", path, image);
            return null;
        }
    }

    public BitmapImage GetBitmapImage(string filename, double width, double height)
    {
        if (CheckPathIsValid(filename))
        {
            var key = filename.ToLower();
            var isCached = cache.TryGetValue(key, out BitmapImage result);

            if (!isCached)
            {
                Core.Log.Logger.Information("Fetching image for {Filename}", filename);

                // Get bitmap from IShellItemImageFactory
                var options = ThumbnailOptions.BiggerSizeOk;
                using var bitmap = ThumbnailProvider.GetThumbnail(filename, (int)width, (int)height, options);

                // Save to BitmapImage
                using MemoryStream stream = new();
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                result = new();
                result.SetSource(stream.AsRandomAccessStream());
                cache.Set(key, result);
            }

            return result;
        }
        else
        {
            Core.Log.Logger.Error("Failed to find image for {Path}", filename);
            return null;
        }
    }

    private static bool CheckPathIsValid(string path)
    {
        return !string.IsNullOrWhiteSpace(path) &&  // Not null or empty
            !path.StartsWith('\\') &&               // Not UNC path
            File.Exists(path);                      // File exists
    }
}
