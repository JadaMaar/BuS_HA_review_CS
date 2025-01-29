using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using BuSHA_CSEdition.Models;
using BuSHA_CSEdition.ViewModels;

namespace BuSHA_CSEdition.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void AddTask(object? sender, RoutedEventArgs e)
    {
        var vm = (MainViewModel)DataContext!;
        vm.AddTask();
    }

    private void RemoveComment(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: Comment obj })
        {
            var vm = (MainViewModel)DataContext!;
            vm.RemoveComment(obj);
        }
    }

    private void AddComment(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: Task obj })
        {
            var vm = (MainViewModel)DataContext!;
            vm.AddComment(obj);
        }
    }

    private void RemoveTask(object? sender, RoutedEventArgs e)
    {
        var vm = (MainViewModel)DataContext!;
        vm.RemoveTask();
    }

    private void Generate(object? sender, RoutedEventArgs e)
    {
        var vm = (MainViewModel)DataContext!;
        vm.Generate();
    }

    private void NameBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            GenerateBtn.IsEnabled = textBox.Text != "";
        }
    }

    private void Clear(object? sender, RoutedEventArgs e)
    {
        var vm = (MainViewModel)DataContext!;
        vm.ClearComments();
    }

    private async void Export(object? sender, RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = "Save File",
            DefaultExtension = "bus"
        });
        if (file != null)
        {
            var vm = (MainViewModel)DataContext!;
            vm.Export(file.Path.LocalPath);
        }
    }

    private async void Import(object? sender, RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open File",
            AllowMultiple = false
        });
        if (files != null)
        {
            var vm = (MainViewModel)DataContext!;
            vm.Import(files[0].Path.LocalPath);
        }
    }

    private async void SetPath(object? sender, RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var folder = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "Pick Folder",
            AllowMultiple = false
        });

        var vm = (MainViewModel)DataContext!;
        vm.SavePath = folder[0].Path.LocalPath;
    }

    private void OpenSettings(object? sender, RoutedEventArgs e)
    {
        DialogHost.IsOpen = true;
    }
}