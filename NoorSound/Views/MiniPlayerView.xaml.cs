using Microsoft.Extensions.DependencyInjection;
using NoorSound.ViewModels;

namespace NoorSound.Views;

public partial class MiniPlayerView : ContentView
{
    public MiniPlayerView()
    {
        InitializeComponent();
    }

    // This view is made by the ControlTemplate in App.xaml (not by dependency injection),
    // so it fetches the shared player ViewModel itself, as soon as it belongs to a page.
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler?.MauiContext?.Services is { } services && BindingContext is not AudioPlayerViewModel)
        {
            BindingContext = services.GetRequiredService<AudioPlayerViewModel>();
        }
    }
}
