using NoorSound.Views;

namespace NoorSound
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RegisterForRoute<AddAudioPage>();
            // RegisterForRoute<UpdateAudioPage>(); // In Shaa Allah, add such a page.
        }

        protected void RegisterForRoute<T>()
        {
            Routing.RegisterRoute(typeof(T).Name, typeof(T));
        }
    }
}
