using NoorSound.ViewModels;

namespace NoorSound.Views;

public partial class PlaylistPage : ContentPage
{
    private readonly PlaylistViewModel _vm;

    public PlaylistPage(PlaylistViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    // Called when the page appears, loads the audios of the playlist that was tapped
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadPlaylistAudiosCommand.ExecuteAsync(null);
    }
}