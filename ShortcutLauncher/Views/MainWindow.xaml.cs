using System.Windows;
using ShortcutLauncher.Models;
using System.Windows.Controls;
using ShortcutLauncher.ViewModels;
using System.Windows.Controls.Primitives;

namespace ShortcutLauncher.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel();
    }

    private void Shortcut_Click(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton button &&
            button.DataContext is Shortcut shortcut &&
            DataContext is MainViewModel vm)
        {
            vm.SelectedShortcut = shortcut;
        }
    }
}