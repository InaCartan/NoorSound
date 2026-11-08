// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\
using NoorSound.Services.Interfaces;
using NoorSound.ViewModels;


namespace NoorSound.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _vm;

    public HomePage(HomeViewModel vm, IPlayerService player)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;

        // The hidden MediaElement in HomePage.xaml is the one and only player of the app.
        // PlayerService controls it, so the audio keeps playing on all pages.
        player.Attach(MediaHost);
    }

    // In Shaa Allah, this func is called when this page appears, and loads the audios and playlists,
    // - thus refreshing the page.
    // LoadHome handles its own errors, so this async void can't crash the app.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadHome();
    }
}
