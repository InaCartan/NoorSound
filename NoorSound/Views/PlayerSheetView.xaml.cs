using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using NoorSound.ViewModels;

namespace NoorSound.Views;

public partial class PlayerSheetView : ContentView
{
    private AudioPlayerViewModel? _vm;
    private bool _animating;
    private bool _topBarWasVisible = true;

    public PlayerSheetView()
    {
        InitializeComponent();
    }

    // Created by the ControlTemplate in App.xaml, so it fetches the shared player ViewModel itself
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler is null)
        {
            if (_vm is not null)
            {
                _vm.PropertyChanged -= OnVmPropertyChanged;
                _vm = null;
            }

            return;
        }

        if (_vm is null && Handler.MauiContext?.Services is { } services)
        {
            _vm = services.GetRequiredService<AudioPlayerViewModel>();
            BindingContext = _vm;
            _vm.PropertyChanged += OnVmPropertyChanged;
        }
    }

    // IsPlayerExpanded changed (a tap, a swipe, the back button...) -> slide up or down
    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AudioPlayerViewModel.IsPlayerExpanded) && _vm is not null)
        {
            _ = MoveAsync(_vm.IsPlayerExpanded);
        }
    }

    private async Task MoveAsync(bool expanded)
    {
        _animating = true;

        try
        {
            if (expanded)
            {
                InputTransparent = false;                           // the player now catches touches
                await Sheet.TranslateToAsync(0, 0, 300, Easing.CubicOut);
                SetChromeVisible(false);                            // cover the whole screen
            }
            else
            {
                SetChromeVisible(true);
                await Sheet.TranslateToAsync(0, Height > 0 ? Height : 3000, 250, Easing.CubicIn);
                InputTransparent = true;                            // the page below works again
            }
        }
        finally
        {
            _animating = false;
        }
    }

    // Keeps the closed player right below the screen when the size changes (rotation, bars shown/hidden)
    private void OnSizeChanged(object? sender, EventArgs e)
    {
        if (_vm is { IsPlayerExpanded: false } && !_animating && Height > 0)
        {
            Sheet.TranslationY = Height;
        }
    }

    // The player follows the finger downwards. Let go after a quarter of the screen -> close, else snap back.
    private async void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (_vm is not { IsPlayerExpanded: true } || _animating) return;

        switch (e.StatusType)
        {
            case GestureStatus.Running:
                Sheet.TranslationY = Math.Max(0, e.TotalY);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (Sheet.TranslationY > Height / 4)
                {
                    _vm.CollapsePlayerCommand.Execute(null);
                }
                else
                {
                    await MoveAsync(true);
                }
                break;
        }
    }

    // Hides (or shows again) the tab bar and the top bar of the page on screen,
    // so the player covers the whole screen like in Spotify.
    private void SetChromeVisible(bool visible)
    {
        // Every tab page has its own player view; only the one on screen may change the bars
        if (FindPage() is not { } page || !ReferenceEquals(page, Shell.Current?.CurrentPage))
        {
            return;
        }

        if (!visible)
        {
            _topBarWasVisible = Shell.GetNavBarIsVisible(page);
        }

        Shell.SetTabBarIsVisible(page, visible);
        Shell.SetNavBarIsVisible(page, visible && _topBarWasVisible);
    }

    private Page? FindPage()
    {
        Element? element = this;

        while (element is not null and not Page)
        {
            element = element.Parent;
        }

        return element as Page;
    }

    // ------- Slider -------

    private void OnDragStarted(object? sender, EventArgs e)
    {
        _vm?.BeginSeek();
    }

    private async void OnDragCompleted(object? sender, EventArgs e)
    {
        if (_vm is not null)
        {
            await _vm.EndSeekAsync(ProgressSlider.Value);
        }
    }
}