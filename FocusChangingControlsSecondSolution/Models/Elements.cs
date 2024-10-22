using FocusChangingControlsSecondSolution.ViewModels;
using ReactiveUI;

namespace FocusChangingControlsSecondSolution.Models;

public class Elements : ReactiveObject
{
    private int _id;
    private string _text1;
    private string _text2;

    public int Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string Text1
    {
        get => _text1;
        set => this.RaiseAndSetIfChanged(ref _text1, value);
    }
    
    public string Text2
    {
        get => _text2;
        set => this.RaiseAndSetIfChanged(ref _text2, value);
    }
    
    public bool Visible { get; set; } = true;
}