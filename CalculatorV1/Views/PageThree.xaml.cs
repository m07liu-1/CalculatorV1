using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.Defaults;
namespace CalculatorV1.Views;


public partial class PageThree : ContentPage
{
    public PageThree(PageThreeVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    private async void OpenGraph(object sender, EventArgs e)
    {
        GraphVM vm = AppShell.Current.Handler.MauiContext.Services.GetService<GraphVM>();
        var popup = new Graph(vm);
        await this.ShowPopupAsync(popup);
    }
    private void OnEntryCompleted(object sender, EventArgs e)
    {
        if (BindingContext is PageThreeVM vm)
        {
            vm.SaveCommand.Execute(null);
        }
    }
}

public partial class PageThreeVM : ObservableObject
{
    [ObservableProperty]
    public partial int ToBeSaved { get; set; } = 0;
    private BackUp backup;
    public PageTwoThreeVM _vm { get; }

    public ObservablePoint[] Values { get; set; } = [
        new(0,0),
        new(1,1),
        new(2,2.3),
        new(4, 5)
        ];
    public PageThreeVM(PageTwoThreeVM vm, BackUp backup)
    {
        _vm = vm;
        this.backup = backup;
        ToBeSaved = (int)backup.CurrentData.Page3.Average;
    }

    [RelayCommand]
    private void Save()
    {
        backup.CurrentData.Page3.Average = ToBeSaved;
        backup.Save();
    }

    public void Load()
    {
        ToBeSaved = (int)backup.CurrentData.Page3.Average;
    }

}

public class PageThreeData
{
    public decimal Average { get; set; } = 0;
}