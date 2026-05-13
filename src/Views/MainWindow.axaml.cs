using AIMediaPlayer.Services;
using AIMediaPlayer.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIMediaPlayer.Views;

public partial class MainWindow : Window
{
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;
    private DispatcherTimer _inactivityTimer;
    private DispatcherTimer _uiTimer;
    private IPlaylistManager _playlistManager;
    private bool _isDraggingProgress = false;
    private string _currentPlaylistPath = "playlist.json";

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

                var titleLabel = this.FindControl<TextBlock>("TitleLabel");
                if (titleLabel != null && _mediaPlayer.Media != null)
                {
                    titleLabel.Text = _mediaPlayer.Media.Meta(MetadataType.Title) ?? "Unknown Media";
                }

                UpdateSubtitleList();
                SyncPlaylistSelection();
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

            InputLayer.IsHitTestVisible = true;
            InputLayer.Background = Brushes.Transparent;

            var overlay = this.FindControl<Grid>("VideoOverlay");
            if (overlay != null)
            {
                overlay.IsHitTestVisible = true;
                overlay.Background = Brushes.Transparent;
                overlay.AddHandler(InputElement.PointerMovedEvent, OnUserActivity, RoutingStrategies.Tunnel);
                overlay.AddHandler(InputElement.PointerEnteredEvent, OnUserActivity, RoutingStrategies.Tunnel);
                overlay.AddHandler(InputElement.PointerPressedEvent, OnUserActivity, RoutingStrategies.Tunnel);
            }

            var overlayControls = this.FindControl<DockPanel>("OverlayControls");
            if (overlayControls != null)
            {
                overlayControls.AddHandler(InputElement.PointerMovedEvent, OnUserActivity, RoutingStrategies.Tunnel);
                overlayControls.AddHandler(InputElement.PointerEnteredEvent, OnUserActivity, RoutingStrategies.Tunnel);
            }

            InputLayer.AddHandler(InputElement.PointerMovedEvent, OnUserActivity, RoutingStrategies.Tunnel);
            InputLayer.AddHandler(InputElement.PointerEnteredEvent, OnUserActivity, RoutingStrategies.Tunnel);
            InputLayer.AddHandler(InputElement.PointerPressedEvent, OnUserActivity, RoutingStrategies.Tunnel);
        };

        _uiTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        _uiTimer.Tick += (sender, e) =>
        {
            if (_mediaPlayer.Media != null && _mediaPlayer.Media.Duration > 0)
            {
                if (_mediaPlayer.IsPlaying && !_isDraggingProgress)
                {
                    var progressSliderLocal = this.FindControl<Slider>("ProgressSlider");
                    if (progressSliderLocal != null)
                    {
                        progressSliderLocal.Value = (double)_mediaPlayer.Time / _mediaPlayer.Media.Duration;
                    }
                }

                var timeLabel = this.FindControl<TextBlock>("TimeLabel");
                if (timeLabel != null)
                {
                    TimeSpan current = TimeSpan.FromMilliseconds(_mediaPlayer.Time);
                    TimeSpan total = TimeSpan.FromMilliseconds(_mediaPlayer.Media.Duration);
                    timeLabel.Text = $"{current:hh\\:mm\\:ss} / {total:hh\\:mm\\:ss}";
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

        _inactivityTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2.5) };
        _inactivityTimer.Tick += (sender, e) =>
        {
            var overlayControls = this.FindControl<DockPanel>("OverlayControls");
            if (overlayControls != null)
            {
                overlayControls.Opacity = 0;
                overlayControls.IsHitTestVisible = false;
            }

            if (VideoPlayer.IsPointerOver)
            {
                this.Cursor = new Cursor(StandardCursorType.None);
            }
            _inactivityTimer.Stop();
        };
        _inactivityTimer.Start();

        InputLayer.PointerPressed += (s, e) =>
        {
            if (_mediaPlayer == null) return;

            var overlayControls = this.FindControl<DockPanel>("OverlayControls");
            if (overlayControls != null && overlayControls.IsPointerOver) return;

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
            UpdateMediaListUI();

            var titleList = _playlistManager.ListAll();
            if (titleList != null)
            {
                _playlistManager.SetCurrentIndex(titleList.Count - 1);
            }

            var media = _playlistManager.GetCurrent();
            _mediaPlayer.Media = media;
            _mediaPlayer.Play();
        }
    }

    private void UpdateMediaListUI()
    {
        var listBox = this.FindControl<ListBox>("PlaylistBox");
        string playlistName = System.IO.Path.GetFileNameWithoutExtension(_currentPlaylistPath);

        var headerLabel = this.FindControl<TextBlock>("PlaylistHeaderLabel");
        if (headerLabel != null) headerLabel.Text = playlistName;

        if (listBox != null)
        {
            var uiItems = new List<AIMediaPlayer.Models.PlaylistItemUI>();

            Avalonia.Media.Imaging.Bitmap? defaultThumb = null;
            try
            {
                var uri = new Uri("avares://AIMediaPlayer/Assets/default-preview.png");
                defaultThumb = new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(uri));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            var playlistInfo = _playlistManager.GetPlaylistInfo();

            if (playlistInfo != null && playlistInfo.Count > 0)
            {
                foreach (var item in playlistInfo)
                {
                    Avalonia.Media.Imaging.Bitmap? thumbToUse = defaultThumb;
                    if (!string.IsNullOrEmpty(item.ThumbnailPath) && System.IO.File.Exists(item.ThumbnailPath))
                    {
                        try { thumbToUse = new Avalonia.Media.Imaging.Bitmap(item.ThumbnailPath); }
                        catch { }
                    }

                    string mediaTypeLabel = "Video File";
                    try
                    {
                        if (!string.IsNullOrEmpty(item.Mrl))
                        {
                            string localPath = Uri.UnescapeDataString(new Uri(item.Mrl).LocalPath);
                            string extension = System.IO.Path.GetExtension(localPath).ToLower();
                            if (extension == ".mp3" || extension == ".wav" || extension == ".flac" ||
                                extension == ".m4a" || extension == ".wma" || extension == ".aac")
                            {
                                mediaTypeLabel = "Audio File";
                            }
                        }
                    }
                    catch { }

                    uiItems.Add(new AIMediaPlayer.Models.PlaylistItemUI
                    {
                        Title = item.Title,
                        Thumbnail = thumbToUse,
                        MediaType = mediaTypeLabel
                    });
                }

                listBox.ItemsSource = uiItems;

                var statusLabel = this.FindControl<TextBlock>("PlaylistStatusLabel");
                if (statusLabel != null) statusLabel.Text = $"{playlistName} - 0 / {playlistInfo.Count}";
            }
            else
            {
                listBox.ItemsSource = null;
                var statusLabel = this.FindControl<TextBlock>("PlaylistStatusLabel");
                if (statusLabel != null) statusLabel.Text = $"{playlistName} - 0 / 0";
            }

            SyncPlaylistSelection();
        }
    }


    private void SyncPlaylistSelection()
    {
        var media = _playlistManager.GetCurrent();
        if (media == null) return;

        var listBox = this.FindControl<ListBox>("PlaylistBox");
        string playlistName = System.IO.Path.GetFileNameWithoutExtension(_currentPlaylistPath);
        if (listBox != null)
        {
            string? currentTitle = media.Meta(MetadataType.Title);

            if (string.IsNullOrEmpty(currentTitle))
            {
                currentTitle = System.IO.Path.GetFileName(Uri.UnescapeDataString(media.Mrl));
            }

            var items = listBox.ItemsSource as List<AIMediaPlayer.Models.PlaylistItemUI>;

            if (items != null && currentTitle != null)
            {
                int idx = items.FindIndex(i => i.Title == currentTitle);
                if (idx != -1)
                {
                    listBox.SelectedIndex = idx;
                }

                var statusLabel = this.FindControl<TextBlock>("PlaylistStatusLabel");
                if (statusLabel != null)
                {
                    int currentNumber = idx == -1 ? 0 : idx + 1;
                    statusLabel.Text = $"{playlistName} - {currentNumber} / {items.Count}";
                }
            }
        }
    }

    private void OnUserActivity(object? sender, PointerEventArgs e)
    {
        if (_isDraggingProgress) return;

        var overlayControls = this.FindControl<DockPanel>("OverlayControls");
        if (overlayControls != null)
        {
            overlayControls.Opacity = 1;
            overlayControls.IsHitTestVisible = true;
        }

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

    private void Stop_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_mediaPlayer != null && _mediaPlayer.IsPlaying)
        {
            _mediaPlayer.Stop();
            var playPauseBtn = this.FindControl<Button>("PlayPauseButton");
            if (playPauseBtn != null) playPauseBtn.Content = "▶";

            var titleLabel = this.FindControl<TextBlock>("TitleLabel");
            if (titleLabel != null) titleLabel.Text = "No media loaded";
        }
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

    private void Shuffle_OnClick(object? sender, RoutedEventArgs e)
    {
        _playlistManager.Shuffle();
        UpdateMediaListUI();
        PlayNextMedia();
    }

    private void Repeat_OnClick(object? sender, RoutedEventArgs e)
    {
        _playlistManager.Repeat();
    }

    private void PlaylistBox_DoubleTapped(object? sender, TappedEventArgs e)
    {
        var listBox = sender as ListBox;
        if (listBox != null && listBox.SelectedIndex != -1)
        {
            _playlistManager.SetCurrentIndex(listBox.SelectedIndex);
            var media = _playlistManager.GetCurrent();
            if (media != null)
            {
                _mediaPlayer.Media = media;
                _mediaPlayer.Play();
            }
        }
    }

    private async void AddMedia_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = true,
            Title = "Load Media",
            FileTypeFilter = new[] { new FilePickerFileType("Media Files") { Patterns = new[] { "*.mp4", "*.mkv", "*.avi", "*.mov", "*.wmv", "*.mp3", "*.wav", "*.m4a" } } }
        });

        foreach (var file in files)
        {
            await _playlistManager.Add(file.Path);
        }
        UpdateMediaListUI();
    }

    private void Remove_OnClick(object? sender, RoutedEventArgs e)
    {
        var listBox = this.FindControl<ListBox>("PlaylistBox");
        if (listBox != null && listBox.SelectedIndex != -1)
        {
            _playlistManager.Remove(listBox.SelectedIndex, _currentPlaylistPath);
            UpdateMediaListUI();
        }
    }

    private async void SavePlaylist_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Playlist",
            DefaultExtension = "json",
            ShowOverwritePrompt = true
        });

        if (file != null)
        {
            _currentPlaylistPath = file.Path.LocalPath;
            _playlistManager.SavePlaylist(_currentPlaylistPath);
        }
    }

    private async void LoadPlaylist_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            Title = "Load Playlist",
            FileTypeFilter = new[] { new FilePickerFileType("JSON Files") { Patterns = new[] { "*.json" } } }
        });

        if (files.Count > 0)
        {
            _currentPlaylistPath = files[0].Path.LocalPath;

            // Adăugăm await aici pentru a aștepta încărcarea și parsarea
            await _playlistManager.Load(_currentPlaylistPath);

            UpdateMediaListUI();

            var currentMedia = _playlistManager.GetCurrent();
            if (currentMedia != null)
            {
                _mediaPlayer.Media = currentMedia;
                _mediaPlayer.Play();
            }
        }


    }

    private async void AddSubtitleFile_Click(object? sender, RoutedEventArgs e)
    {
        if (_mediaPlayer == null || !_mediaPlayer.IsPlaying) return;

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            Title = "Add Subtitle File",
            FileTypeFilter = new[] { new FilePickerFileType("Subtitle Files") { Patterns = new[] { "*.srt", "*.ass", "*.ssa", "*.sub" } } }
        });

        if (files.Count > 0)
        {
            var uri = files[0].Path;
            _mediaPlayer.AddSlave(MediaSlaveType.Subtitle, uri.AbsoluteUri, true);
            UpdateSubtitleList();
        }
    }

    private void UpdateSubtitleList()
    {
        var subtitleListMenu = this.FindControl<MenuItem>("SubtitleListMenu");
        if (subtitleListMenu == null) return;

        var items = new List<MenuItem>();
        var descriptions = _mediaPlayer.SpuDescription;

        if (descriptions != null && descriptions.Length > 0)
        {
            subtitleListMenu.IsEnabled = true;
            int currentSubtitleID = _mediaPlayer.Spu;

            foreach (var item in descriptions)
            {
                var menuItem = new MenuItem
                {
                    Header = item.Id == currentSubtitleID ? $"✔ {item.Name}" : item.Name,
                    Tag = item.Id
                };

                menuItem.Click += (s, e) =>
                {
                    _mediaPlayer.SetSpu(item.Id);
                    UpdateSubtitleList();
                };
                items.Add(menuItem);
            }
            subtitleListMenu.ItemsSource = items;
        }
        else
        {
            subtitleListMenu.IsEnabled = false;
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
        if (e.Property.Name == "Value" && _mediaPlayer != null && e.NewValue is double newValue)
        {
            _mediaPlayer.Volume = (int)newValue;
        }
    }
}