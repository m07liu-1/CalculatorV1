using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input; 

namespace CalculatorV1.Views;

public partial class PageTwoThreeVM : ObservableObject
{
    [ObservableProperty]
    public partial decimal Average { get; set; }

    public void UpdateData(decimal num1, decimal num2 = 0)
    {
        Average = (num1 + num2) / 2;
    }
}