using CalculatorV1.Views;
using System.Diagnostics;
using System.Xml.Serialization;
using Octokit;

namespace CalculatorV1
{
    public partial class AppShell : Shell
    {
        public AppShell(BackUp backup)
        {
            InitializeComponent();
            BindingContext = backup;
            // Register the routes
            Routing.RegisterRoute(nameof(Views.PageTwo), typeof(Views.PageTwo));
            Routing.RegisterRoute(nameof(Views.PageThree), typeof(Views.PageThree));
            Routing.RegisterRoute(nameof(Views.RandomNum), typeof(Views.RandomNum));

            // Subscribe to Loaded event to ensure UI is fully initialized before showing dialogs
            Loaded += async (s, e) =>
            {
                await LaunchUpdate();
                await DownloadUpdate();
            };
        }
        private async Task DownloadUpdate()
        {
            var updatePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "update.exe");
            if (File.Exists(updatePath))
            {
                return;
            }
            var github = new GitHubClient(new ProductHeaderValue("CalculatorV1"));
            var latestRelease = await github.Repository.Release.GetLatest("m07liu-1", "CalculatorV1");
            var latestVersion = latestRelease.TagName;
            var CurrentVersion = AppInfo.VersionString;
            if (latestVersion != CurrentVersion)
            {
                bool download = await Shell.Current.DisplayAlertAsync("Update Available", "Download in the background now?", "Yes", "No");
                if (download)
                {
                    try
                    {
                        var asset = latestRelease.Assets.FirstOrDefault(a => a.Name.EndsWith(".exe"));
                        if (asset != null)
                        {
                            string tempFilePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "update.exe");
                            using (var client = new HttpClient())
                            {
                                client.DefaultRequestHeaders.UserAgent.ParseAdd("CalculatorV1");
                                var response = await client.GetAsync(asset.BrowserDownloadUrl);

                                var data = await response.Content.ReadAsByteArrayAsync();
                                await File.WriteAllBytesAsync(tempFilePath, data);
                                await Shell.Current.DisplayAlertAsync("Download Complete", "Reopen the exe to launch the update", "Ok");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error downloading update: {ex.Message}");
                    }
                }
            }
        }

        private async Task LaunchUpdate()
        {
            var updatePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "update.exe");
            System.Diagnostics.Debug.WriteLine("Update path: " + updatePath);
            if (File.Exists(updatePath))
            {
                bool confirm = await Shell.Current.DisplayAlertAsync("Update Ready", "An update has been downloaded. Restart to apply the update?", "Yes", "No");
                if (!confirm) return;
                try
                {
                    string executableName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "CalculatorV1.exe";
                    string TargetExePath = Environment.ProcessPath;
                    string batchContent = $@"@echo off
timeout /t 2 /nobreak
taskkill /f /im ""{executableName}""
timeout /t 2 /nobreak
copy /y ""{updatePath}"" ""{TargetExePath}""
start """" ""{TargetExePath}""
del ""{updatePath}""
del ""%~f0""";
                    string batchFilePath = Path.Combine(Path.GetTempPath(), "update.bat");
                    await File.WriteAllTextAsync(batchFilePath, batchContent);
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = batchFilePath,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    Process.Start(processInfo);
                    await Task.Delay(1000);
                    App.Current?.Quit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error launching update: {ex.Message}");
                }
            }
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
                var pagetwoVM = AppShell.Current.Handler.MauiContext.Services.GetService<PageTwoVM>();
                var newPage = new PageTwo(pagetwoVM);
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
                var pagethreeVM = AppShell.Current.Handler.MauiContext.Services.GetService<PageThreeVM>();
                var newPage = new PageThree(pagethreeVM);
                var newTab = new Tab
                {
                    Title = "PageThree",
                    Items = {new ShellContent {  Content = newPage } }
                };
                Tabs.Items.Add(newTab);
                Tabs.CurrentItem = newTab;
            } else
            {
                if (openOrCreate("Random")) return;
                var newTab = new ShellContent
                {
                    Title = "Random",
                    Route = nameof(Views.RandomNum),
                    ContentTemplate = new DataTemplate(typeof(Views.RandomNum))
                };
                Tabs.Items.Add(newTab);
                Tabs.CurrentItem = newTab;
            }
        }
        
        private void Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Load clicked!");
            LoadHelper();
        }

        private async Task LoadHelper()
        {
            try
            {
                FileResult result = await FilePicker.Default.PickAsync(new PickOptions
                { PickerTitle = "Select a xml file"
                });
                if (result != null) {
                    // Ensure we load into the DI singleton BackUp that VMs use
                    var diBackup = AppShell.Current.Handler.MauiContext.Services.GetService<BackUp>();
                    diBackup?.Load(result.FullPath);

                    PageTwoVM vm2 = AppShell.Current.Handler.MauiContext.Services.GetService<Views.PageTwoVM>();
                    PageThreeVM vm3 = AppShell.Current.Handler.MauiContext.Services.GetService<Views.PageThreeVM>();
                    vm2.Load();
                    vm3.Load();
                }
            } catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{ex.Message}");
            }
        }
    }

    [XmlRoot("AppData")]
    public class AppData
    {
        public PageTwoData Page2 { get; set; } = new();
        public PageThreeData Page3 { get; set; } = new();
    }

    public class BackUp
    {
        private readonly string filePath;
        private readonly XmlSerializer serializer;

        public AppData CurrentData { get; private set; } = new();

        public BackUp()
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                "eHSN-Backups");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            filePath = Path.Combine(dir, DateTime.Today.ToString("yyyy-MM-dd") + ".xml");
            serializer = new XmlSerializer(typeof(AppData));
        }

        public void Save()
        {
            try
            {
                using (var stream = new FileStream(filePath, System.IO.FileMode.Create, FileAccess.Write, FileShare.None))
                using (var writer = new StreamWriter(stream))
                {
                    serializer.Serialize(writer, CurrentData);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error saving xml: {e.Message}");
            }
        }

        public void Load(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                CurrentData = (serializer.Deserialize(sr) as AppData) ?? new AppData();
            }
            
        }
    }
}
