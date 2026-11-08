using NoorSound.ViewModels;

namespace NoorSound.Views;

public partial class AudioPlayerPage : ContentPage
{
    private readonly AudioPlayerViewModel _vm;

    public AudioPlayerPage(AudioPlayerViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    // The user starts dragging the slider -> stop the slider from following the audio
    private void OnDragStarted(object? sender, EventArgs e)
    {
        _vm.BeginSeek();
    }

    // The user lets go -> jump to that position.
    // EndSeekAsync handles its own errors, so this async void can't crash the app.
    private async void OnDragCompleted(object? sender, EventArgs e)
    {
        await _vm.EndSeekAsync(ProgressSlider.Value);
    }
}
