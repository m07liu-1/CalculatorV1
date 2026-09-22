using CommunityToolkit.Mvvm.ComponentModel;

namespace CalculatorV1.Views;

public partial class PageThree : ContentPage
{
    public PageThree(PageThreeVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}

public partial class PageThreeVM : ObservableObject
{
    public PageTwoThreeVM _vm { get; }

    public PageThreeVM(PageTwoThreeVM vm)
    {
        _vm = vm;
    }
}