using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using FocusChangingControlsSecondSolution.Models;
using FocusChangingControlsSecondSolution.ViewModels;

namespace FocusChangingControlsSecondSolution.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    //пока не нужно тк из-за виртуализации сразу будет фокус на появляющийся элемент
    private async void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        await Task.Delay(1);

        var tb = sender as TextBox;
        // поскольку разом добавляется два TextBox-а, то фокус нужно поставить на первый из них
        if (tb.Tag == "First")
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                tb.Focus();
            });
        }
    }

    private async void Control_KeyDown(object? sender, KeyEventArgs e)
    {
        await Task.Delay(1);
        if (e.Key == Key.Enter && e.KeyModifiers == KeyModifiers.None)
        {
            var tb = sender as TextBox;
            if (IsLastTextBox(tb)) // если Enter нажат в последнем TextBox-е, то нужно создать новую строку
            {
                await CreateNewRow(tb);
                
            }
            else // иначе фокус должен переключиться на уже существующий TextBox
            {
                await SwitchFocusToNextRow(tb);
            }
        }
    }

    private bool IsLastTextBox(TextBox? tb)
    {
        var nextElement = KeyboardNavigationHandler.GetNext(tb, NavigationDirection.Next);
        if (IsOurTextbox(
                nextElement)) // если tb - последний созданный TextBox, то за ним будет следовать не "наш" элемент
        {
            return false;
        }

        return true;
    }

    // "нашим" является TextBox, помеченный либо First, либо Second
    private bool IsOurTextbox(IInputElement? element)
    {
        if (element is TextBox nextTb)
        {
            if (nextTb.Tag?.ToString() == "First")
            {
                return true;
            }
        }

        return false;
    }

    private async Task SwitchFocusToNextRow(TextBox? tb)
    {
        await Task.Delay(1);
        var nextElement = KeyboardNavigationHandler.GetNext(tb!, NavigationDirection.Next);
        nextElement?.Focus();
    }

    private async Task CreateNewRow(TextBox? tb)
    {
        await Task.Delay(1);
        var dc = DataContext as MainWindowViewModel;
        dc.AddNewRow.Execute(tb.DataContext as Elements).Subscribe();
        

    }
}