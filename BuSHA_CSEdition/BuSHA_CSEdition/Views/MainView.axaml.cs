using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
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
}