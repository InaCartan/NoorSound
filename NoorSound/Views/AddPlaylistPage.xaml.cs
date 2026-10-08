using NoorSound.ViewModels;


namespace NoorSound.Views;

public partial class AddPlaylistPage : ContentPage
{
	public AddPlaylistPage(AddPlaylistViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}