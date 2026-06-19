// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\
using NoorSound.ViewModels;

namespace NoorSound.Views;

public partial class ProfilePage : ContentPage
{
	private readonly ProfileViewModel _vm;

	public ProfilePage(ProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm; 
	}
}