using FocusChangingControlsSecondSolution.ViewModels;
using ReactiveUI;

namespace FocusChangingControlsSecondSolution.Models;

public class Elements: ViewModelBase
{
    private int _id;
    private string _text;
    private string _text2;
    
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
    public string Text2
    {
        get => _text2;
        set => this.RaiseAndSetIfChanged(ref _text2, value);
    }

    
}