using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CalculatorV1.Views;

public partial class PageTwo : ContentPage
{
	public PageTwo(PageTwoVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    private void transferData(Object sender, EventArgs e)
    {
        var shell = Shell.Current;
        var Tabs = shell.Items.FirstOrDefault(i => i is TabBar) as TabBar;
        if (Tabs != null)
        {
            if (decimal.TryParse(Data1.Text, out decimal num1) && decimal.TryParse(Data2.Text, out decimal num2))
            {
                foreach(ShellSection tab in Tabs.Items)
                {
                    if (tab.Title == "PageThree")
                    {
                        Tabs.CurrentItem = tab;
                        // Call PageThree's receive data method but dont know how
                        return;
                    }
                }
                // Have to make next page
                var template = new DataTemplate(typeof(PageThree));
                var newTab = new ShellContent
                { 
                    Title = typeof(PageThree).Name,
                    Route = nameof(PageThree),
                    ContentTemplate = template
                };
                Tabs.Items.Add(newTab);
                Tabs.CurrentItem = newTab;
                // call PageThree's receive data method but doesnt know how
            } else
            {
                message.TextColor = Colors.Red;
                message.Text = "Error! Invalid Number(s) Entered!";
            }
        }
    }

    private void updateTime(object sender, EventArgs e)
    {

    }

    private void takeTime(object sender, EventArgs e)
    {
        var currentTime = DateTime.Now.TimeOfDay;
        timePicker.Time = currentTime;
    }

    private void takeDate(object sender, EventArgs e)
    {
        datePicker.Date = DateTime.Today;
    }

    private void OnCheckBoxCheckedChanged(object sender, EventArgs e)
    {

    }
}

public partial class PageTwoVM : ObservableObject
{
    private readonly PageTwoThreeVM _vm;

    [ObservableProperty]
    public partial string Num1 { get; set; }

    [ObservableProperty]
    public partial string Num2 { get; set; }

    public PageTwoVM(PageTwoThreeVM vm)
    {
        _vm = vm;
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
        var pagethreeVM = new PageThreeVM(_vm);
        var newTab = new Tab
        {
            Title = "PageThree",
            Items = { new ShellContent { Content = new PageThree(pagethreeVM) } }
        };

        Tabs.Items.Add(newTab);
        Tabs.CurrentItem = newTab;
    }
}