using NoorSound.ViewModels;

namespace NoorSound.Views;
public partial class SignUpPage : ContentPage
{
	public SignUpPage(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}