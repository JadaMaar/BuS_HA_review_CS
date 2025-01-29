using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BuSHA_CSEdition.Models;

public class Task : INotifyPropertyChanged
{
    public string TaskName { get; set; } = "";
    private float _mult = 1.0f;

    public float Mult
    {
        get => _mult;
        set
        {
            _mult = value;
            OnPropertyChanged();
        }
    }

    private bool _passed;

    public bool Passed
    {
        get => _passed;
        set
        {
            if (value != _passed)
            {
                _passed = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _star;

    public bool Star
    {
        get => _star;
        set
        {
            if (value != _star)
            {
                _star = value;
                OnPropertyChanged();
                if (_star)
                    Passed = true;
                Console.WriteLine(Passed);
            }
        }
    }

    private ObservableCollection<Comment> _comments = new();

    public ObservableCollection<Comment> Comments
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

    public Task()
    {
        Comments.CollectionChanged += (sender, args) =>
        {
            OnPropertyChanged();
            foreach (var comment in Comments)
            {
                comment.PropertyChanged += (sender, args) => { OnPropertyChanged(); };
            }
        };
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public void RemoveComment(Comment comment)
    {
        Comments.Remove(comment);
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