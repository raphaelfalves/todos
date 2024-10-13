using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using ToDosProject.Shared.Extendions;
using ToDosProject.Shared.Services;

namespace ToDosProject.App
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
                });

#if DEBUG
            builder.Configuration.AddInMemoryCollection(AspireAppSettings.Settings);
#endif

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            builder.AddAppDefaults();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddServices();

            MauiApp mauiApp = builder.Build();
            mauiApp.InitOpenTelemetryServices();
            return mauiApp;
        }
    }
}
