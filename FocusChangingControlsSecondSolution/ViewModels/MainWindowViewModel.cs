using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using DynamicData;
using FocusChangingControlsSecondSolution.Models;
using ReactiveUI;

namespace FocusChangingControlsSecondSolution.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> AddNewRow { get; }

    private ObservableCollection<Elements> _elementsList;
    public ObservableCollection<Elements> ElementsList
    {
        get => _elementsList;
        set => this.RaiseAndSetIfChanged(ref _elementsList, value);
    }

    public MainWindowViewModel()
    {
        ElementsList = new ObservableCollection<Elements>
        {
            new Elements { Id = 1, Text = "First item" }
        };

        AddNewRow = ReactiveCommand.Create(() =>
        {
           
                ElementsList.Add(new Elements
                {
                    Id = ElementsList.Count + 1,
                    Text = $"Item {ElementsList.Count + 1}"
                });
            
        });
    }
}