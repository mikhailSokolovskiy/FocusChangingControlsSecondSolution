using FocusChangingControlsSecondSolution.ViewModels;
using ReactiveUI;

namespace FocusChangingControlsSecondSolution.Models;

public class Elements : ReactiveObject
{
    private int _id;
    private string _text;

    public int Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }
}