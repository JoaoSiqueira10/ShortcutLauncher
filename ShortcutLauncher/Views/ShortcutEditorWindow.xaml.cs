using System.Windows;
using ShortcutLauncher.Models;
using Microsoft.Win32;
using System.IO;

namespace ShortcutLauncher.Views;

public partial class ShortcutEditorWindow : Window
{
    public Shortcut Shortcut { get; private set; }

    public ShortcutEditorWindow()
    {
        InitializeComponent();

        Shortcut = new Shortcut();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        Shortcut.Name = txtName.Text;
        Shortcut.Path = txtPath.Text;

        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void Browse_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();

        dialog.Filter =
            "Todos os arquivos (*.*)|*.*|" +
            "Executáveis (*.exe)|*.exe";

        if (dialog.ShowDialog() == true)
        {
            txtPath.Text = dialog.FileName;

            // preencher nome automaticamente se estiver vazio
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtName.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }
    }
}