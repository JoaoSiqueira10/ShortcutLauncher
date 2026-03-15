using System.Collections.ObjectModel;

namespace ShortcutLauncher.Models;

public class ShortcutGroup
{
    public string Name { get; set; } = "";

    public ObservableCollection<Shortcut> Shortcuts { get; set; }
        = new ObservableCollection<Shortcut>();
}