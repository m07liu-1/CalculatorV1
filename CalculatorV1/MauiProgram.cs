using CalculatorV1.Views;
using Microsoft.Extensions.Logging;

namespace CalculatorV1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<BackUp>();
            builder.Services.AddSingleton<PageTwoThreeVM>();
            builder.Services.AddSingleton<PageTwoVM>();
            builder.Services.AddTransient<PageTwo>();
            builder.Services.AddSingleton<PageThreeVM>();
            builder.Services.AddTransient<PageThree>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
