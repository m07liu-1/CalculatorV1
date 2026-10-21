using CalculatorV1.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace CalculatorV1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddHttpClient<AQPopup>(client =>
            {
                client.BaseAddress = new Uri("https://wscdev.aquaticinformatics.net/AQUARIUS/Publish/v2/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddSingleton<BackUp>();
            builder.Services.AddSingleton<PageTwoThreeVM>();
            builder.Services.AddSingleton<PageTwoVM>();
            builder.Services.AddTransient<PageTwo>();
            builder.Services.AddSingleton<PageThreeVM>();
            builder.Services.AddTransient<PageThree>();
            builder.Services.AddTransient<AppShell>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
