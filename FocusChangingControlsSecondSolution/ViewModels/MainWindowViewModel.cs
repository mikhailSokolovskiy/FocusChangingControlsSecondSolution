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
    public ReactiveCommand<Elements, Unit> AddNewRow { get; set; }
    private ObservableCollection<Elements>? _elementsList;
    public ReactiveCommand<Elements, Unit> PointerReleased { get; set; }
    public ObservableCollection<Elements>? ElementsList
    {
        get => _elementsList;
        set => this.RaiseAndSetIfChanged(ref _elementsList, value);
    }

    private int _findId;

    public int FindId
    {
        get => _findId;
        set => this.RaiseAndSetIfChanged(ref _findId, value);
    }
    public MainWindowViewModel()
    {
        ElementsList = new ObservableCollection<Elements>(new List<Elements>());
        ElementsList.Add(new ObservableCollection<Elements>
        {
            new()
            {
                Id = ElementsList.Count + 1, Text = "", Text2 = ""
            }
        });


        AddNewRow = ReactiveCommand.Create<Elements>((el) =>
        {
            ElementsList.Add(
                new()
                {
                    Id = ElementsList.Count + 1,
                    Text = "",
                    Text2 = "",
                }
            );
        });
        PointerReleased = ReactiveCommand.Create<Elements>((el) =>
        {
            FindId = el.Id;
        });
    }
}