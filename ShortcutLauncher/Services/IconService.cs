using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace ShortcutLauncher.Services;

public static class IconService
{
    public static BitmapSource? GetIcon(string path)
    {
        try
        {
            if (!File.Exists(path))
                return null;

            Icon icon = Icon.ExtractAssociatedIcon(path);

            using MemoryStream ms = new MemoryStream();
            icon.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = ms;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            return image;
        }
        catch
        {
            return null;
        }
    }
}