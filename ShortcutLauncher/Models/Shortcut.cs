using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace ShortcutLauncher.Models;

public class Shortcut
{
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";

    [JsonIgnore]
    public BitmapSource? Icon { get; set; }
}