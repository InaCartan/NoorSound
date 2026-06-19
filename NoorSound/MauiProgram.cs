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

            builder.Services.AddSingleton(provider =>
            {
                var client = new Supabase.Client(url, key);
                client.InitializeAsync().Wait();
                return client;
            });


            // adding ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<LibraryViewModel>();
            builder.Services.AddTransient<AddAudioViewModel>();

            // adding Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddSingleton<HomePage>();    
            builder.Services.AddSingleton<LibraryPage>();
            builder.Services.AddTransient<AddAudioPage>();

            // adding Services
            builder.Services.AddSingleton<IDataService, DataService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();


            // adding Shell
            builder.Services.AddSingleton<AppShell>();

            



#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}