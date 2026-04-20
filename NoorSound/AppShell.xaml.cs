using NoorSound.Views;

namespace NoorSound
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("AddAudioPage", typeof(AddAudioPage));
        }
    }
}
