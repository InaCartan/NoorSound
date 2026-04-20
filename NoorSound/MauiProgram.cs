// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using NoorSound.Models;
using NoorSound.Services;
using NoorSound.ViewModels;
using NoorSound.Views;
using NoorSound.Confiq;
using Supabase;
namespace NoorSound
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();

            // In Shaa Allah
            // configure Supabase
            var url = SupabaseConfiq.SUPABASE_URL;
            var key = SupabaseConfiq.SUPABASE_KEY;
            builder.Services.AddSingleton(provider => new Supabase.Client(url, key));

            // add ViewModels
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<LibraryViewModel>();
            builder.Services.AddTransient<AddAudioViewModel>();

            // add Views
            //builder.Services.AddSingleton<Homepage>();    
            builder.Services.AddSingleton<LibraryPage>();
            builder.Services.AddSingleton<AddAudioPage>();

            // add Data Service
            builder.Services.AddSingleton<IDataService, DataService>();


#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}