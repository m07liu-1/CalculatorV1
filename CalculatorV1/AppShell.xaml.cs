namespace CalculatorV1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register the routes
            Routing.RegisterRoute(nameof(Views.PageTwo), typeof(Views.PageTwo));
            Routing.RegisterRoute(nameof(Views.PageThree), typeof(Views.PageThree));
        }

        private bool openOrCreate(string name)
        {
            foreach (ShellSection tab in Tabs.Items) // Look for tab T in created tabs
            {
                if (tab.Title == name)
                {
                    Tabs.CurrentItem = tab; // If tab is already created switch to that tab
                    return true;
                }
            }
            return false;
        }

        private void openTab(object sender, EventArgs e)
        {
            var request = (MenuFlyoutItem)sender;
            if (request?.Text == "Calculator")
            {
                openOrCreate("Calculator");
                return;
            } else if (request?.Text == "Page 2")
            {
                if (openOrCreate("PageTwo")) return;
                var sharedVM = AppShell.Current.Handler.MauiContext.Services.GetService<Views.PageTwoThreeVM>();
                var pagetwoVM = new Views.PageTwoVM(sharedVM);
                var newPage = new Views.PageTwo(pagetwoVM);
                var newTab = new Tab
                {
                    Title = "PageTwo",
                    Items = {new ShellContent {  Content = newPage } }
                };
                Tabs.Items.Add(newTab);
                Tabs.CurrentItem = newTab;

            } else if (request?.Text == "Page 3")
            {
                if (openOrCreate("PageThree")) return;
                var sharedVM = AppShell.Current.Handler.MauiContext.Services.GetService<Views.PageTwoThreeVM>();
                var pagethreeVM = new Views.PageThreeVM(sharedVM);
                var newPage = new Views.PageThree(pagethreeVM);
                var newTab = new Tab
                {
                    Title = "PageThree",
                    Items = {new ShellContent {  Content = newPage } }
                };
                Tabs.Items.Add(newTab);
                Tabs.CurrentItem = newTab;
            }
        }
        
    }
}
