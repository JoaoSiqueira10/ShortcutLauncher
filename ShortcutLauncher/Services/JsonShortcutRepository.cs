using System.IO;
using System.Text.Json;
using System.Windows;
using ShortcutLauncher.Models;

namespace ShortcutLauncher.Services;

public class JsonShortcutRepository : IShortcutRepository
{
    //private readonly string _filePath = "Data/shortcuts.json";
    private readonly string _filePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "shortcuts.json");

    public List<ShortcutGroup> Load()
    {
        if (!File.Exists(_filePath))
        {
            CreateEmptyFile();
            return new List<ShortcutGroup>();
        }

        var json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<ShortcutGroup>();

        return JsonSerializer.Deserialize<List<ShortcutGroup>>(json)
               ?? new List<ShortcutGroup>();
    }

    public void Save(List<ShortcutGroup> groups)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        var json = JsonSerializer.Serialize(groups,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(_filePath, json);

        //MessageBox.Show(_filePath);
    }

    private void CreateEmptyFile()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        var json = JsonSerializer.Serialize(new List<ShortcutGroup>(),
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(_filePath, json);
    }
}