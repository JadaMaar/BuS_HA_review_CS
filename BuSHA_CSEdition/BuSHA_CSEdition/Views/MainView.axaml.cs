using System;
using Avalonia.Controls;
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

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var vm = (MainViewModel)DataContext!;
        vm.AddTask();
    }

    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        var vm = (MainViewModel)DataContext!;
        vm.PrintTasks();
    }

    private void TextBox_OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            // Validate text to allow only numbers, periods, and optional negative sign
            string newText = textBox.Text;
            Console.WriteLine(newText);
            if (!float.TryParse(newText, out _) && !string.IsNullOrEmpty(newText))
            {
                // Revert the change if it's invalid
                textBox.Text = newText.Remove(newText.Length - 1);
                e.Handled = true;
            }
        }
    }

    private void RemoveComment(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Comment obj)
        {
            var vm = (MainViewModel)DataContext!;
            vm.RemoveComment(obj);
        }
    }

    private void AddComment(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Task obj)
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
        vm.Generate(NameBox.Text);
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