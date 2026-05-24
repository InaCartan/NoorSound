using NoorSound.ViewModels;

namespace NoorSound.Views;

public partial class AddAudioPage : ContentPage
{
	public AddAudioPage(AddAudioViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}