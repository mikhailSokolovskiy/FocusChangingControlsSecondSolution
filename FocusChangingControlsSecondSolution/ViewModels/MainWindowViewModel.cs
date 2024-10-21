using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using DynamicData;
using FocusChangingControlsSecondSolution.Models;
using ReactiveUI;

namespace FocusChangingControlsSecondSolution.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ReactiveCommand<Elements, Unit> AddNewRow { get; }

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
            new() { Id = 1, Text1 = "First item", Text2 = "second item"  }
        };

        AddNewRow = ReactiveCommand.CreateFromTask<Elements>(async _ =>
        {
            await CreateRow();
        });
    }
    
    private Task CreateRow()
    {
        ElementsList.Add(new Elements
        {
            Id = ElementsList.Count + 1,
            Text1 = $"Item {ElementsList.Count + 1}",
            Text2 = $"Item2 {ElementsList.Count + 1}",
        });
        return Task.CompletedTask;
    }
}