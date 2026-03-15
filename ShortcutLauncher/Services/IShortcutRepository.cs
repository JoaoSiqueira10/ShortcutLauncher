using ShortcutLauncher.Models;

namespace ShortcutLauncher.Services;

public interface IShortcutRepository
{
    List<ShortcutGroup> Load();

    void Save(List<ShortcutGroup> groups);
}