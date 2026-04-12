using AIMediaPlayer.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using System;

namespace AIMediaPlayer.Views;

public partial class MainWindow : Window
{
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;
    private DispatcherTimer _inactivityTimer;

    public MainWindow()
    {
        InitializeComponent();

        LibVLCSharp.Shared.Core.Initialize();

        _libVLC = new LibVLC();

        _mediaPlayer = new MediaPlayer(_libVLC);

        _mediaPlayer.Playing += (s, e) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                PlayPauseButton.Content = "❚❚";
            });
        };

        _mediaPlayer.Paused += (s, e) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                PlayPauseButton.Content = "▶";
            });
        };

        this.Loaded += (s, e) =>
        {
            VideoPlayer.MediaPlayer = _mediaPlayer;
        };

        DataContextChanged += (s, e) =>
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.RequestPlay += () => _mediaPlayer?.Play();
                vm.RequestPause += () => _mediaPlayer?.Pause();

                vm.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(vm.VideoSource) && vm.VideoSource != null)
                    {
                        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                        // TO DO
                        //pentru subitrari, merge doar pentru un anumit fisier, nu pentru toate !!!!!!
                        //string subtitlePath = System.IO.Path.Combine(baseDir, "resources", "transcript.srt");

                        //if (!System.IO.File.Exists(subtitlePath))
                        //{
                        //    System.Diagnostics.Debug.WriteLine($"!!! EROARE: Fișierul nu există la: {subtitlePath}");
                        //    return;
                        //}

                        var media = new Media(_libVLC, vm.VideoSource);
                        //media.AddOption($":sub-file={subtitlePath}");
                        //media.AddOption(":subsdec-encoding=UTF-8"); 

                        _mediaPlayer.Media = media;
                        _mediaPlayer.Play();
                    }
                };
            }
        };

        _inactivityTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(5)
        };

        _inactivityTimer.Tick += (sender, e) =>
        {
            ControlBar.Opacity = 0;
            ControlBar.IsHitTestVisible = false;

            this.Cursor = new Cursor(StandardCursorType.None);

            _inactivityTimer.Stop();     
        };

        InputLayer.PointerMoved += OnUserActivity;
        ControlBar.PointerMoved += OnUserActivity;
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
    private void OnUserActivity(object? sender, PointerEventArgs e)
    {
        ControlBar.Opacity = 1;
        ControlBar.IsHitTestVisible = true;

        this.Cursor = new Cursor(StandardCursorType.Arrow);

        _inactivityTimer.Stop();
        _inactivityTimer.Start();
    }
    //protected override void OnPointerMoved(PointerEventArgs e)
    //{
    //    base.OnPointerMoved(e);

    //    ControlBar.IsVisible = true;

    //    _inactivityTimer.Stop();
    //    _inactivityTimer.Start();
    //}
    //private void Play_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    //{
    //    _mediaPlayer.Play();
        
    //}
    protected override void OnClosed(EventArgs e)
    {
        _mediaPlayer?.Dispose();
        _libVLC?.Dispose();
        base.OnClosed(e);
    }
    //private void Pause_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    //{
    //    if (_mediaPlayer.IsPlaying)
    //    {
    //        _mediaPlayer.SetPause(true);
    //    }
    //}

    private void PlayPause_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_mediaPlayer == null) return;

        if (_mediaPlayer.IsPlaying)
        {
            _mediaPlayer.SetPause(true);
        }
        else
        {
            _mediaPlayer.Play();
        }
    }
}