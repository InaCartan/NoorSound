using NoorSound.ViewModels;

namespace NoorSound.Views;

public partial class AudioPlayerPage : ContentPage
{
	public AudioPlayerPage(AudioPlayerViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}