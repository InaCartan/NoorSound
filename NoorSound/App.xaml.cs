using NoorSound.Views;

namespace NoorSound
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly Supabase.Client _supabase;


        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            // _supabase = supabase;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            //Page startPage;

            //// In Shaa Allah, check if user already logged in
            //if (_supabase.Auth.CurrentSession != null)
            //{
            //    startPage = _serviceProvider
            //        .GetRequiredService<AppShell>();
            //}
            //else
            //{
            //    startPage = _serviceProvider
            //        .GetRequiredService<LoginPage>();
            //}

            //return new Window(startPage);

            return new Window(_serviceProvider.GetRequiredService<AppShell>());

        }

    }
}
