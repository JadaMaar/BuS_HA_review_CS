using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using BuSHA_CSEdition.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Task = BuSHA_CSEdition.Models.Task;

namespace BuSHA_CSEdition.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Task> Tasks { get; set; } = new();
    private String _fileName = "";

    public String FileName
    {
        get => _fileName;
        set
        {
            _fileName = value;
            IsSaved = false;
            OnPropertyChanged();
        }
    }

    private Boolean _isSaved;

    public Boolean IsSaved
    {
        get => _isSaved;
        set
        {
            _isSaved = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public MainViewModel()
    {
        Tasks.CollectionChanged += (sender, args) =>
        {
            IsSaved = false;
            if (args.NewItems != null)
                foreach (Task task in args.NewItems)
                {
                    task.PropertyChanged += (o, eventArgs) => { IsSaved = false; };
                }
        };
    }

    public void AddTask()
    {
        Task task = new();
        Tasks.Add(task);
    }

    public void RemoveTask()
    {
        if (Tasks.Count > 0)
            Tasks.RemoveAt(Tasks.Count - 1);
    }

    public async void ClearComments()
    {
        try
        {
            if (IsSaved)
            {
                _ClearComments();
            }
            else
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Warning", "Are you sure you would like to clear without saving?",
                        ButtonEnum.YesNo, Icon.Warning, WindowStartupLocation.CenterOwner);
                if (Avalonia.Application.Current
                        ?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    if (desktop.MainWindow != null)
                    {
                        var result = await box.ShowWindowDialogAsync(desktop.MainWindow);
                        if (result.ToString() == "Yes")
                        {
                            _ClearComments();
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            // TODO handle exception
            Console.WriteLine("Exception caught in ClearComments");
        }
    }

    private void _ClearComments()
    {
        FileName = "";
        foreach (var task in Tasks)
        {
            task.Comments.Clear();
            task.Star = false;
            task.Passed = false;
        }
    }

    public void RemoveComment(Comment comment)
    {
        foreach (var task in Tasks)
        {
            if (task.Comments.Contains(comment))
            {
                task.RemoveComment(comment);
                break;
            }
        }
    }

    public void AddComment(Task task)
    {
        task.AddComment(new());
    }

    public void PrintTasks()
    {
        foreach (var task in Tasks)
        {
            Console.WriteLine(task.TaskName);
            Console.WriteLine(task.Mult);
            foreach (var comment in task.Comments)
            {
                Console.WriteLine(comment.Text);
            }

            Console.WriteLine("-------------------------------------");
        }
    }

    public async void Generate()
    {
        if (ValidateInputs())
        {
            Directory.CreateDirectory("reports");
            using (StreamWriter outputFile = new StreamWriter($"reports/{FileName}.txt"))
            {
                float maxPoints = 0f;
                float pointCounter = 0f;
                float starPointCounter = 0f;
                foreach (var task in Tasks)
                {
                    var star = task.Star ? "*" : "";
                    var passed = task.Passed ? 1 : 0;
                    var weighting = task.Mult == 1f ? "" : $"({task.Mult}x Gewichtung)";
                    var taskLine = $"{task.TaskName}) {passed}{star}/1 {weighting}";
                    pointCounter += passed * task.Mult;
                    starPointCounter += task.Star ? passed * task.Mult : 0;
                    maxPoints += task.Mult;
                    outputFile.WriteLine(taskLine);
                    foreach (var comment in task.Comments)
                    {
                        outputFile.WriteLine($"- {comment.Text}");
                    }
                }

                outputFile.WriteLine($"Total: {pointCounter}/{maxPoints}");
                outputFile.WriteLine($"Total *: {starPointCounter}*/{maxPoints}*");
            }

            IsSaved = true;
        }
        else
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Error", "Invalid multiplier are being used.\n Use . for decimals",
                    ButtonEnum.Ok, Icon.Error, WindowStartupLocation.CenterOwner);
            if (Avalonia.Application.Current
                    ?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow != null)
                {
                    var result = await box.ShowWindowDialogAsync(desktop.MainWindow);
                }
            }
        }
    }

    private bool ValidateInputs()
    {
        foreach (var task in Tasks)
        {
            if (task.Mult is float.NaN)
                return false;
        }

        return true;
    }
}