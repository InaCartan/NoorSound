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

   
}