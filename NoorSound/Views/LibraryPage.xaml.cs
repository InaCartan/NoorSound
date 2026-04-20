using NoorSound.ViewModels;
namespace NoorSound.Views;

public partial class LibraryPage : ContentPage
{
	public LibraryPage(LibraryViewModel libraryViewModel)
	{
        InitializeComponent();
		BindingContext = libraryViewModel;
	}
}