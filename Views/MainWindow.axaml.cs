using AIMediaPlayer.ViewModels;
using Avalonia.Controls;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using System;

namespace AIMediaPlayer.Views;

public partial class MainWindow : Window
{
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;

    public MainWindow()
    {
        InitializeComponent();

        LibVLCSharp.Shared.Core.Initialize();

        _libVLC = new LibVLC();

        _mediaPlayer = new MediaPlayer(_libVLC);

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
                        _mediaPlayer.Media = new Media(_libVLC, vm.VideoSource);
                        _mediaPlayer.Play();
                    }
                };
            }
        };
    }
    

    private void Play_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mediaPlayer.Play();
    }
    protected override void OnClosed(EventArgs e)
    {
        _mediaPlayer?.Dispose();
        _libVLC?.Dispose();
        base.OnClosed(e);
    }
    private void Pause_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_mediaPlayer.IsPlaying)
        {
            _mediaPlayer.SetPause(true);
        }
    }
}