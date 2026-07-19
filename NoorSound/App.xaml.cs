using NoorSound.Services;
using System.Diagnostics;

namespace NoorSound
{
    public partial class App : Application
    {
        //private readonly IServiceProvider _serviceProvider;
        private readonly IStartupService _startupService;
        private readonly IDialogService _dialogService;
        private readonly AppShell _appShell;

        private Task? _initializationTask;

        public App(/*IServiceProvider serviceProvider,*/ IStartupService startupService, AppShell appShell, IDialogService dialogService)
        {
            InitializeComponent();
          //  _serviceProvider = serviceProvider;
            _startupService = startupService;
            _appShell = appShell;
            _dialogService = dialogService;


        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(_appShell);

            window.Created += OnWindowCreated;

            return window;

        }

        private async void OnWindowCreated(
            object? sender,
            EventArgs e)
        {
            try
            {
                // Prevent initialization from running more than once.
                //_initializationTask ??= _startupService.InitializeAsync();
                _initializationTask ??= InitializeApplicationAsync();

                await _initializationTask;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                

                // Permit another attempt if initialization is started again.
                _initializationTask = null;

                await _dialogService.ShowAlert(
                    "Startup error",
                    "The application could not be initialized.");
            }
        }

        private async Task InitializeApplicationAsync()
        {
            // First initialize Supabase.
            await _startupService.InitializeAsync();

            // Then check the authenticated user and navigate.
            await _appShell.InitializeAsync();
        }


    }
}

