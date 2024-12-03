using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BuSHA_CSEdition.Models;

public class Task : INotifyPropertyChanged
{
    public string task { get; set; } = "";
    public float mult { get; set; } = 1f;
    private bool _passed;
    public bool passed {
        get => _passed;
        set
        {
            if (value != _passed)
            {
                _passed = value;
                OnPropertyChanged(nameof(passed));
            }
        }
    }

    private bool _star;

    public bool star
    {
        get => _star;
        set
        {
            if (value != _star)
            {
                _star = value;
                OnPropertyChanged();
                if (_star)
                    passed = true;
                Console.WriteLine(passed);
            }
        }
    }

    private ObservableCollection<Comment> _comments = new();

    public ObservableCollection<Comment> comments
    {
        get => _comments;
        set
        {
            if (_comments != value)
            {
                _comments = value;
                OnPropertyChanged();
            }
        }
    }

    public void SetTask(string task)
    {
        this.task = task;
    }

    public void SetMult(float mult)
    {
        this.mult = mult;
    }

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }
    
    public void RemoveComment(Comment comment)
    {
        comments.Remove(comment);
    }

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
}