// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\
using NoorSound.ViewModels;


namespace NoorSound.Views;

public partial class LibraryPage : ContentPage
{

	private readonly LibraryViewModel _vm;

	public LibraryPage(LibraryViewModel vm)
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
        await _vm.LoadAudios();
    }

}