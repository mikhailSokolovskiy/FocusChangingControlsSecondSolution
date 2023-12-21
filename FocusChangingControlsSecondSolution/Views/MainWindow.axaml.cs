using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using FocusChangingControlsSecondSolution.Models;
using FocusChangingControlsSecondSolution.ViewModels;

namespace FocusChangingControlsSecondSolution.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var tb = sender as TextBox;
        // поскольку разом добавляется два TextBox-а, то фокус нужно поставить на первый из них
        if (tb.Tag == "First")
        {
            tb.Focus();
        }
    }

    private void Control_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var tb = sender as TextBox;
            if (IsLastTextBox(tb)) // если Enter нажат в последнем TextBox-е, то нужно создать новую строку
            {
                CreateNewRow(tb);
            }
            else // иначе фокус должен переключиться на уже существующий TextBox
            {
                SwitchFocusToNextRow(tb);
            }
        }
    }

    private bool IsLastTextBox(TextBox tb)
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
    private bool IsOurTextbox(IInputElement element)
    {
        if (element is TextBox nextTb)
        {
            if (nextTb.Tag == "First" || nextTb.Tag == "Second")
            {
                return true;
            }
        }

        return false;
    }

    private void SwitchFocusToNextRow(TextBox tb)
    {
        var nextElement = KeyboardNavigationHandler.GetNext(tb, NavigationDirection.Next);
        nextElement.Focus();
    }

    private void CreateNewRow(TextBox? tb)
    {
        var dc = DataContext as MainWindowViewModel;
        dc.AddNewRow.Execute(tb.DataContext as Elements).Subscribe();
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var dc = DataContext as MainWindowViewModel;
        var tb = sender as TextBox;
        dc.PointerReleased.Execute(tb.DataContext as Elements).Subscribe();
    }
}