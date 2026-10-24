using NoorSound.ViewModels;


namespace NoorSound.Views;

public partial class AddPlaylistPage : ContentPage
{
    private readonly AddPlaylistViewModel _vm;

    public AddPlaylistPage(AddPlaylistViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm;	
	}

    // In Shaa Allah, this func is called when this page appears, and calls the GetAudio func to get the audios,
    // - thus refreshing the page
    // In Shaa Allah, the func is automatically called.
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // null because the command doesn't require a parameter
        // (the comment is an auto generated suggestion - don't know if it's correct)
        await _vm.LoadMyAudiosCommand.ExecuteAsync(null); 
    }
}