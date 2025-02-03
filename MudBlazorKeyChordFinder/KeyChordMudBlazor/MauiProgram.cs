using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using KeyChordFinder.Services;
using KeyChordFinder.Data;
using System.Reflection;

namespace KeyChordMudBlazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddMudServices();
            builder.Services.AddScoped<PlayAudio>();

            //Copy the database file from the embedded resources to the local storage
            var assembly = Assembly.GetExecutingAssembly();
            DbHelper.CopyIfDoesntExist("KeyChordFinder.db", assembly);

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
