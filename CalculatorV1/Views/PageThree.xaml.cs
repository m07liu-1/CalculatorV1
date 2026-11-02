using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        var popup = new Graph(AppShell.Current.Handler.MauiContext.Services.GetService<GraphVM>());
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