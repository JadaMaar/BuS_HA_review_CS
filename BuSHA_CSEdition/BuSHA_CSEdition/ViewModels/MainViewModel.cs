using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BuSHA_CSEdition.Models;

namespace BuSHA_CSEdition.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Task> Tasks { get; set; } = new();
    public string Greeting => "Welcome to Avalonia!";
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
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

    public void ClearComments()
    {
        foreach (var task in Tasks)
        {
            task.comments.Clear();
            task.star = false;
            task.passed = false;
        }
    }

    public void RemoveComment(Comment comment)
    {
        foreach (var task in Tasks)
        {
            if (task.comments.Contains(comment))
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
            Console.WriteLine(task.task);
            Console.WriteLine(task.mult);
            foreach (var comment in task.comments)
            {
                Console.WriteLine(comment.text);
            }

            Console.WriteLine("-------------------------------------");
        }
    }

    public void Generate(string? name)
    {
        Directory.CreateDirectory("reports");
        using (StreamWriter outputFile = new StreamWriter($"reports/{name}.txt"))
        {
            float maxPoints = 0f;
            float pointCounter = 0f;
            float starPointCounter = 0f;
            foreach (var task in Tasks)
            {
                var star = task.star ? "*" : "";
                var passed = task.passed ? 1 : 0;
                var weighting = (int)task.mult == 1 ? "" : "({task.mult}x Gewichtung)";
                var taskLine = $"{task.task}) {passed}{star}/1 {weighting}";
                pointCounter += passed * task.mult;
                starPointCounter += task.star ? passed * task.mult : 0;
                maxPoints += task.mult;
                outputFile.WriteLine(taskLine);
                foreach (var comment in task.comments)
                {
                    outputFile.WriteLine($"- {comment.text}");
                }
            }
            outputFile.WriteLine($"Total: {pointCounter}/{maxPoints}");
            outputFile.WriteLine($"Total *: {starPointCounter}*/{maxPoints}*");
        }
    }
}