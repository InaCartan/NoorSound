using NoorSound.Views;


namespace NoorSound.Services
{
    public static class AppRoutes
    {
        // This is registered separately in AppShell.xaml.cs (note -> .cs)
        public const string AddAudio = nameof(AddAudioPage);

        // These are defined directly inside AppShell.xaml in a ShellContent.
        public const string Login = "//" + nameof(LoginPage);
        public const string Home = "//" + nameof(HomePage);
        public const string Library = "//" + nameof(LibraryPage);
        public const string Profile = "//" + nameof(ProfilePage);
        public const string SignUp = "//" + nameof(SignUpPage);
    }
}
