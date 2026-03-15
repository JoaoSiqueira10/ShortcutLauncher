using System.Collections.ObjectModel;
using ShortcutLauncher.Models;
using ShortcutLauncher.Services;
using System.Diagnostics;
using System.Windows.Input;
using ShortcutLauncher.Commands;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace ShortcutLauncher.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IShortcutRepository _repository;

    public ICommand OpenShortcutCommand { get; }
    public ICommand CreateGroupCommand { get; }
    public ICommand CreateShortcutCommand { get; }
    public ICommand EditShortcutCommand { get; }
    public ICommand DeleteShortcutCommand { get; }

    public ObservableCollection<ShortcutGroup> Groups { get; set; }   

    public MainViewModel()
    {
        _repository = new JsonShortcutRepository();

        var groups = _repository.Load();

        Groups = new ObservableCollection<ShortcutGroup>(groups);
        foreach (var group in Groups)
        {
            group.Shortcuts = new ObservableCollection<Shortcut>(group.Shortcuts);

            foreach (var shortcut in group.Shortcuts)
            {
                shortcut.Icon = IconService.GetIcon(shortcut.Path);
            }
        }

        OpenShortcutCommand = new RelayCommand(OpenShortcut);
        CreateShortcutCommand = new RelayCommand(CreateShortcut);
        CreateGroupCommand = new RelayCommand(CreateGroup);
        EditShortcutCommand = new RelayCommand(EditShortcut);
        DeleteShortcutCommand = new RelayCommand(DeleteShortcut);
    }

    public void Save()
    {
        _repository.Save(Groups.ToList());
    }

    private void OpenShortcut(object? parameter)
    {
        if (parameter is not Shortcut shortcut)
            return;

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = shortcut.Path,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao abrir o atalho: {ex.Message}");
        }
    }

    private void CreateShortcut(object? parameter)
    {
        if (SelectedGroup == null)
            return;

        var window = new ShortcutLauncher.Views.ShortcutEditorWindow();

        if (window.ShowDialog() == true)
        {
            if (string.IsNullOrWhiteSpace(window.Shortcut.Path))
                return;

            window.Shortcut.Icon = IconService.GetIcon(window.Shortcut.Path);

            SelectedGroup.Shortcuts.Add(window.Shortcut);


            Save();
        }
    }

    private void CreateGroup(object? parameter)
    {
        var input = Microsoft.VisualBasic.Interaction.InputBox(
            "Digite o nome do grupo:",
            "Novo Grupo",
            "");

        if (string.IsNullOrWhiteSpace(input))
            return;

        Groups.Add(new ShortcutGroup
        {
            Name = input
        });

        //foreach (var group in Groups)
        //{
        //    foreach (var shortcut in group.Shortcuts)
        //    {
        //        shortcut.Icon = IconService.GetIcon(shortcut.Path);
        //    }
        //}

        Save();
    }

    private ShortcutGroup? _selectedGroup;
    public ShortcutGroup? SelectedGroup
    {
        get => _selectedGroup;
        set
        {
            _selectedGroup = value;
            OnPropertyChanged();
        }
    }

    private Shortcut? _selectedShortcut;
    public Shortcut? SelectedShortcut
    {
        get => _selectedShortcut;
        set
        {
            _selectedShortcut = value;
            OnPropertyChanged();
        }
    }

    private string _searchText = "";
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            FilterShortcuts();
        }
    }

    private void EditShortcut(object? parameter)
    {
        if (parameter is not Shortcut shortcut)
            return;

        var window = new ShortcutLauncher.Views.ShortcutEditorWindow();

        window.Shortcut.Name = shortcut.Name;
        window.Shortcut.Path = shortcut.Path;

        if (window.ShowDialog() == true)
        {
            shortcut.Name = window.Shortcut.Name;
            shortcut.Path = window.Shortcut.Path;

            shortcut.Icon = IconService.GetIcon(shortcut.Path);

            Save();
            OnPropertyChanged(nameof(Groups));
        }
    }

    private void DeleteShortcut(object? parameter)
    {
        if (parameter is not Shortcut shortcut || SelectedGroup == null)
            return;

        var result = MessageBox.Show(
            "Deseja excluir este atalho?",
            "Confirmação",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        SelectedGroup.Shortcuts.Remove(shortcut);

        SelectedShortcut = null;

        if (SelectedGroup.Shortcuts.Count == 0)
        {
            Groups.Remove(SelectedGroup);
            SelectedGroup = null;
        }

        Save();
    }

    private void FilterShortcuts()
    {
        foreach (var group in Groups)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(group.Shortcuts);

            view.Filter = obj =>
            {
                if (obj is not Shortcut shortcut)
                    return false;

                if (string.IsNullOrWhiteSpace(SearchText))
                    return true;

                return shortcut.Name.Contains(SearchText,
                    StringComparison.OrdinalIgnoreCase);
            };

            view.Refresh();
        }
    }
}