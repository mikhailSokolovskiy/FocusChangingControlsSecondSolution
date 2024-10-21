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
    
    private void TextBox_Loaded(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox != null && textBox.Tag.ToString() == "First" && ListBox.ItemCount == 1)
        {
            Console.WriteLine($"{textBox.Tag} loaded.");
            textBox.Focus();
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
        // var nextElement = KeyboardNavigationHandler.GetNext(tb, NavigationDirection.Next);
        // // если tb - последний созданный TextBox, то за ним будет следовать не "наш" элемент
        // if (IsOurTextbox(nextElement)) 
        // {
        //     return false;
        // }
        //
        // return true;
        
        var currentElement = tb?.DataContext as Elements;
        var dc = DataContext as MainWindowViewModel;

        if (currentElement != null && dc != null)
        {
            // Получаем индекс текущего элемента в списке
            var currentIndex = dc.ElementsList.IndexOf(currentElement);
            return currentIndex == dc.ElementsList.Count - 1; // Если это последний элемент
        }

        return false;
    }

    // "нашим" является TextBox, помеченный либо First, либо Second
    private bool IsOurTextbox(IInputElement? element)
    {
        if (element is TextBox nextTb)
        {
            if (nextTb.Tag?.ToString() == "First" || nextTb.Tag?.ToString() == "Second")
            {
                return true;
            }
        }

        return false;
    }

    private async Task SwitchFocusToNextRow(TextBox? tb)
    {
        // await Task.Delay(1);
        // var nextElement = KeyboardNavigationHandler.GetNext(tb!, NavigationDirection.Next);
        // nextElement?.Focus();
        
        var currentElement = tb?.DataContext as Elements;
        var dc = DataContext as MainWindowViewModel;

        if (currentElement != null && dc != null)
        {
            var currentIndex = dc.ElementsList.IndexOf(currentElement);

            // Если текущий индекс меньше последнего, то есть следующий элемент
            if (currentIndex >= 0 && currentIndex < dc.ElementsList.Count - 1)
            {
                var nextIndex = currentIndex + 1;

                // Прокручиваем до следующего элемента
                ListBox.ScrollIntoView(nextIndex);
                Console.WriteLine($"Scrolling to index: {nextIndex}");

                // Ждем, чтобы гарантировать, что контейнер был создан
                await Task.Delay(50);

                // Получаем контейнер для следующего элемента
                var nextContainer = ListBox.ItemContainerGenerator.ContainerFromIndex(nextIndex) as ListBoxItem;

                if (nextContainer != null)
                {
                    Console.WriteLine("Next container found.");

                    // Получаем все TextBox внутри следующего контейнера
                    var nextTextBox = nextContainer.GetVisualDescendants()
                        .OfType<TextBox>()
                        .FirstOrDefault(x => x.Tag?.ToString() == "First");

                    if (nextTextBox != null)
                    {
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            nextTextBox.Focus();
                            Console.WriteLine("Focus set to next TextBox.");
                        });
                    }
                    else
                    {
                        Console.WriteLine("Next TextBox not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Next container not found.");
                }
            }
        }
        
    }
    
    
    private async Task CreateNewRow(TextBox? tb)
    {
        await Task.Delay(1);
        var dc = DataContext as MainWindowViewModel;
        dc.AddNewRow.Execute(tb.DataContext as Elements).Subscribe(); 
        
        // Ждем немного для обновления UI
        await Task.Delay(1);
                
        // Получаем индекс нового элемента
        var newElementIndex = dc.ElementsList.Count - 1;

        // Прокручиваем ListBox
        ListBox.ScrollIntoView(newElementIndex);

        // Ждем, чтобы убедиться, что ListBox обновился
        await Task.Delay(1);

        // Получаем контейнер для нового элемента
        var newContainer = ListBox.ItemContainerGenerator.ContainerFromIndex(newElementIndex) as ListBoxItem;
        if (newContainer != null)
        {
            // Ищем все текстбоксы внутри нового контейнера
            var newTextBox = newContainer.GetVisualDescendants()
                .OfType<TextBox>()
                .FirstOrDefault(x => x.Tag?.ToString() == "First");

            newTextBox?.Focus();
        }
    }
    
    
}