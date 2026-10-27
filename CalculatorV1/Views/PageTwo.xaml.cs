    using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace CalculatorV1.Views;

public partial class PageTwo : ContentPage
{
    public PageTwo(PageTwoVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void Entry_Unfocused(object sender, EventArgs e)
    {
        if (BindingContext is PageTwoVM vm)
        {
            vm.SaveCommand.Execute(null);
        }
    }

    private void OnCheckBoxCheckedChanged(object sender, EventArgs e)
    {
        ((CheckBox)sender).Color = ((CheckBox)sender).IsChecked ? Colors.Green : Colors.Red;

    }

    private async void openPopup(object sender, EventArgs e)
    {
        var popup = new Views.DataPopup();
        IPopupResult<string> result = await this.ShowPopupAsync<string>(popup);
        Determinant.Text = result.Result;
    }
}
public class TableData
{
    public string n { get; set; } = string.Empty;
    public string isTrue { get; set; } = string.Empty;
    public string hour { get; set; } = string.Empty;

    public string minutes { get; set; } = string.Empty;
}


public partial class PageTwoVM : ObservableObject
{
    private BackUp backup;
    private readonly PageTwoThreeVM _vm;

    [ObservableProperty]
    public partial string Num1 { get; set; } = "";

    [ObservableProperty]
    public partial string Num2 { get; set; } = "";
    public ObservableCollection<TableData> Data { get; set; } = new();

    public ICommand AddRowCommand { get; }
    public ICommand RemoveCommand { get; }

    public PageTwoVM(PageTwoThreeVM vm, BackUp backup)
    {
        _vm = vm;
        this.backup = backup;
        Num1 = backup.CurrentData.Page2.Num1;
        Num2 = backup.CurrentData.Page2.Num2;
        AddRowCommand = new Command(OnAddRow);
        RemoveCommand = new Command<TableData>(OnRemoveRow);
        Data.Add(new TableData());
    }

    public void OnAddRow()
    {
        Data.Add(new TableData());
    }
    public void OnRemoveRow(TableData row)
    {
        System.Diagnostics.Debug.WriteLine("Value of n is" + row.n);
        Data.Remove(row);
    }

    [RelayCommand]
    private void Transfer() {
        _vm.UpdateData(decimal.Parse(Num1), decimal.Parse(Num2));
        var Tabs = Shell.Current.Items.FirstOrDefault(i => i is TabBar) as TabBar;
        foreach (ShellSection tab in Tabs.Items)
        {
            if (tab.Title == "PageThree")
            {
                Tabs.CurrentItem = tab;
                return;
            }
        }
        var pagethreeVM = AppShell.Current.Handler.MauiContext.Services.GetService<PageThreeVM>();
        var newTab = new Tab
        {
            Title = "PageThree",
            Items = { new ShellContent { Content = new PageThree(pagethreeVM) } }
        };

        Tabs.Items.Add(newTab);
        Tabs.CurrentItem = newTab;
    }

    [RelayCommand]
    private void Save()
    {
        backup.CurrentData.Page2.Num1 = Num1;
        backup.CurrentData.Page2.Num2 = Num2;
        backup.Save();
    }

    public void Load()
    {
        Num1 = backup.CurrentData.Page2.Num1;
        Num2 = backup.CurrentData.Page2.Num2;
    }
}

public class PageTwoData
{
    public string Num1 { get; set; } = "";
    public string Num2 { get; set; } = "";
}