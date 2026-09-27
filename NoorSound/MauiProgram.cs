// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using NoorSound.Services;
using NoorSound.ViewModels;
using NoorSound.Views;
using NoorSound.Confiq;
using NoorSound.Services.Interfaces;
namespace NoorSound
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement(isAndroidForegroundServiceEnabled: true)
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // In Shaa Allah these are the following steps that the program will start with:

            // configure Supabase
            var url = SupabaseConfiq.SUPABASE_URL;
            var key = SupabaseConfiq.SUPABASE_KEY;

            builder.Services.AddSingleton(provider =>
            {
                return new Supabase.Client(url, key);
            });


            // adding ViewModels
            builder.Services.AddTransient<AudioPlayerViewModel>();
            builder.Services.AddTransient<AddAudioViewModel>();
            builder.Services.AddTransient<AddPlaylistViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<LibraryViewModel>();
            builder.Services.AddSingleton<SearchViewModel>();

            // adding Views
            builder.Services.AddTransient<AudioPlayerPage>();
            builder.Services.AddTransient<AddPlaylistPage>();
            builder.Services.AddTransient<AddAudioPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<ProfilePage>();

            builder.Services.AddSingleton<HomePage>();    
            builder.Services.AddSingleton<LibraryPage>();
            builder.Services.AddSingleton<SearchPage>();    


            // adding Services
            builder.Services.AddSingleton<IDataService, DataService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<IStartupService, StartupService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();


            // adding Shell
            builder.Services.AddSingleton<AppShell>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}