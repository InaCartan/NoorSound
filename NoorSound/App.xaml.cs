using NoorSound.Services;
using Supabase;

namespace NoorSound
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IStartupService _startupService;

        public App(IServiceProvider serviceProvider, IStartupService startupService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _startupService = startupService;


        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            _startupService.InitializeAsync().GetAwaiter().GetResult();

            var shell = _serviceProvider.GetRequiredService<AppShell>();

            return new Window(shell);

        }

    }
}
