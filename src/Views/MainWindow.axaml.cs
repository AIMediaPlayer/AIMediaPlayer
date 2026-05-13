using AIMediaPlayer.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using AIMediaPlayer.Services;
using System;
using System.Threading.Tasks;
using Avalonia.Media;

namespace AIMediaPlayer.Views;

public partial class MainWindow : Window
{
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;
    private DispatcherTimer _inactivityTimer;
    private DispatcherTimer _uiTimer;
    private IPlaylistManager _playlistManager;
    private bool _isDraggingProgress = false;

    public MainWindow()
    {
        InitializeComponent();

        var progressSlider = this.FindControl<Slider>("ProgressSlider");
        if (progressSlider != null)
        {
            progressSlider.AddHandler(InputElement.PointerPressedEvent, ProgressSlider_PointerPressed, RoutingStrategies.Tunnel);
            progressSlider.AddHandler(InputElement.PointerReleasedEvent, ProgressSlider_PointerReleased, RoutingStrategies.Tunnel);
        }

        LibVLCSharp.Shared.Core.Initialize();
        _libVLC = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVLC);

        _playlistManager = new PlaylistManager(_libVLC);

        _mediaPlayer.Playing += (s, e) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var playPauseBtn = this.FindControl<Button>("PlayPauseButton");
                if (playPauseBtn != null) playPauseBtn.Content = "❚❚";
            });
        };

        _mediaPlayer.Paused += (s, e) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var playPauseBtn = this.FindControl<Button>("PlayPauseButton");
                if (playPauseBtn != null) playPauseBtn.Content = "▶";
            });
        };

        _mediaPlayer.EndReached += (s, e) =>
        {
            Dispatcher.UIThread.Post(() => PlayNextMedia());
        };

        this.Loaded += (s, e) =>
        {
            VideoPlayer.MediaPlayer = _mediaPlayer;

            // Asigură că InputLayer și VideoOverlay primesc evenimente chiar dacă ControlBar este invizibil
            InputLayer.IsHitTestVisible = true;
            InputLayer.Background = Brushes.Transparent;

            // VideoOverlay (în interiorul VideoView) trebuie să capteze mișcarea mouse-ului
            var overlay = this.FindControl<Grid>("VideoOverlay");
            if (overlay != null)
            {
                overlay.IsHitTestVisible = true;
                overlay.Background = Brushes.Transparent;
                overlay.AddHandler(InputElement.PointerMovedEvent, OnUserActivity, RoutingStrategies.Tunnel);
                overlay.AddHandler(InputElement.PointerEnteredEvent, OnUserActivity, RoutingStrategies.Tunnel);
                overlay.AddHandler(InputElement.PointerPressedEvent, OnUserActivity, RoutingStrategies.Tunnel);
            }

            // Prinde mișcarea mouse-ului la nivel de InputLayer (tunnel pentru a capta devreme)
            InputLayer.AddHandler(InputElement.PointerMovedEvent, OnUserActivity, RoutingStrategies.Tunnel);
            InputLayer.AddHandler(InputElement.PointerEnteredEvent, OnUserActivity, RoutingStrategies.Tunnel);
            InputLayer.AddHandler(InputElement.PointerPressedEvent, OnUserActivity, RoutingStrategies.Tunnel);

            // Fallback: prinde pointerul la nivel de fereastră
            this.AddHandler(InputElement.PointerMovedEvent, OnUserActivity, RoutingStrategies.Tunnel);
            this.AddHandler(InputElement.PointerEnteredEvent, OnUserActivity, RoutingStrategies.Tunnel);
            this.AddHandler(InputElement.PointerPressedEvent, OnUserActivity, RoutingStrategies.Tunnel);

            ControlBar.AddHandler(InputElement.PointerMovedEvent, OnUserActivity, RoutingStrategies.Tunnel);
            ControlBar.AddHandler(InputElement.PointerEnteredEvent, OnUserActivity, RoutingStrategies.Tunnel);
        };

        _uiTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        _uiTimer.Tick += (sender, e) =>
        {
            if (_mediaPlayer.IsPlaying && !_isDraggingProgress && _mediaPlayer.Media != null && _mediaPlayer.Media.Duration > 0)
            {
                var progressSlider = this.FindControl<Slider>("ProgressSlider");
                if (progressSlider != null)
                {
                    progressSlider.Value = (double)_mediaPlayer.Time / _mediaPlayer.Media.Duration;
                }
            }
        };
        _uiTimer.Start();

        DataContextChanged += (s, e) =>
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(vm.VideoSource) && vm.VideoSource != null)
                    {
                        _ = AddAndPlayMediaAsync(vm.VideoSource);
                    }
                };
            }
        };

        _inactivityTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
        _inactivityTimer.Tick += (sender, e) =>
        {
            ControlBar.Opacity = 0;
            ControlBar.IsHitTestVisible = false;
            this.Cursor = new Cursor(StandardCursorType.None);
            _inactivityTimer.Stop();
        };

        _inactivityTimer.Start();

        InputLayer.PointerPressed += (s, e) =>
        {
            if (_mediaPlayer == null) return;
            if (_mediaPlayer.IsPlaying)
                _mediaPlayer.SetPause(true);
            else
                _mediaPlayer.Play();
        };
    }

    private async Task AddAndPlayMediaAsync(Uri videoSource)
    {
        bool success = await _playlistManager.Add(videoSource);
        if (success)
        {
            var titleList = _playlistManager.ListAll();
            if (titleList != null)
            {
                _playlistManager.SetCurrentIndex(titleList.Count - 1);

                var listBox = this.FindControl<ListBox>("PlaylistBox");
                if (listBox != null)
                {
                    listBox.ItemsSource = titleList;
                    listBox.SelectedIndex = titleList.Count - 1;
                }
            }

            var media = _playlistManager.GetCurrent();
            _mediaPlayer.Media = media;
            _mediaPlayer.Play();
        }
    }

    private void OnUserActivity(object? sender, PointerEventArgs e)
    {
        if (_isDraggingProgress)
            return;

        ControlBar.Opacity = 1;
        ControlBar.IsHitTestVisible = true;
        this.Cursor = new Cursor(StandardCursorType.Arrow);
        _inactivityTimer.Stop();
        _inactivityTimer.Start();
    }

    protected override void OnClosed(EventArgs e)
    {
        _uiTimer?.Stop();
        _mediaPlayer?.Dispose();
        _libVLC?.Dispose();
        base.OnClosed(e);
    }

    private void PlayPause_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_mediaPlayer == null) return;
        if (_mediaPlayer.IsPlaying)
            _mediaPlayer.SetPause(true);
        else
            _mediaPlayer.Play();
    }

    private void Next_OnClick(object? sender, RoutedEventArgs e) => PlayNextMedia();

    private void Prev_OnClick(object? sender, RoutedEventArgs e)
    {
        _playlistManager.Previous();
        var prevMedia = _playlistManager.GetCurrent();
        if (prevMedia != null)
        {
            _mediaPlayer.Media = prevMedia;
            _mediaPlayer.Play();
        }
    }

    private void PlayNextMedia()
    {
        _playlistManager.Next();
        var nextMedia = _playlistManager.GetCurrent();
        if (nextMedia != null)
        {
            _mediaPlayer.Media = nextMedia;
            _mediaPlayer.Play();
        }
    }

    private void ProgressSlider_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _isDraggingProgress = true;
    }

    private void ProgressSlider_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isDraggingProgress = false;
        var slider = sender as Slider;

        if (slider != null && _mediaPlayer?.Media != null)
        {
            _mediaPlayer.Position = (float)slider.Value;
        }
    }

    private void VolumeSlider_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property.Name == "Value" && _mediaPlayer != null)
        {
            _mediaPlayer.Volume = (int)((double)e.NewValue);
        }
    }
}
