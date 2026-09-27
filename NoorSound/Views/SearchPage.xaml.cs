// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\
using NoorSound.ViewModels;


namespace NoorSound.Views;

public partial class SearchPage : ContentPage
{
	private readonly SearchViewModel _vm; 

	public SearchPage(SearchViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm;
	}
}