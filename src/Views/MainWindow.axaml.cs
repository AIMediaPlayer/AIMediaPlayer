/**************************************************************************
 * *
 * File:        MainWindow.axaml.cs                                      *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: Code-behind for main window, managing UI and LibVLC.     *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/
using AIMediaPlayer.Exceptions;
using AIMediaPlayer.Services;
using AIMediaPlayer.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


namespace AIMediaPlayer.Views;

/// <summary>
/// Reprezintă fereastra principală a aplicației, care gestionează interfața grafică,
/// inițializarea player-ului LibVLC și evenimentele de interacțiune ale utilizatorului.
/// </summary>
public partial class MainWindow : Window
{
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;
    private DispatcherTimer _inactivityTimer;
    private DispatcherTimer _uiTimer;
    private IPlaylistManager _playlistManager;
    private bool _isDraggingProgress = false;
    private string _currentPlaylistPath = "playlist.json";

    /// <summary>
    /// Expune instanța MediaPlayer-ului pentru a putea fi accesată de clasele de stare (State Pattern).
    /// </summary>
    public LibVLCSharp.Shared.MediaPlayer MediaPlayer => _mediaPlayer;

    /// <summary>
    /// Reprezintă starea curentă a player-ului media (ex. PlayingState, PausedState, StoppedState).
    /// </summary>
    private States.IPlayerState _currentState;

    /// <summary>
    /// Constructorul ferestrei principale. Inițializează componentele UI, engine-ul LibVLC,
    /// playlist-ul, starea inițială și se abonează la evenimentele necesare.
    /// </summary>
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
        _mediaPlayer.Volume = 100;
        _playlistManager = new PlaylistManager(_libVLC);

        _currentState = new States.StoppedState(); // Starea inițială

        //abonare pentru observer
        _playlistManager.PlaylistUpdated += () =>
        {
            Dispatcher.UIThread.Post(() => UpdateMediaListUI());
        };

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

    /// <summary>
    /// Modifică starea player-ului media conform State Pattern și actualizează iconița butonului Play/Pause.
    /// </summary>
    /// <param name="newState">Noua stare a player-ului.</param>
    public void SetState(States.IPlayerState newState)
    {
        _currentState = newState;

        // Actualizăm UI-ul butonului
        var playPauseBtn = this.FindControl<Avalonia.Controls.Button>("PlayPauseButton");
        if (playPauseBtn != null)
            playPauseBtn.Content = _currentState.GetButtonIcon();
    }

    /// <summary>
    /// Adaugă un fișier media în playlist pe baza unui URI și începe redarea acestuia imediat.
    /// </summary>
    /// <param name="videoSource">Sursa (URI) fișierului media.</param>
    /// <returns>Task asincron pentru operațiunea de adăugare.</returns>
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
            SetState(new States.PlayingState());
        }
    }

    /// <summary>
    /// Actualizează elementele grafice din componenta ListBox aferentă playlist-ului.
    /// Încarcă thumbnail-urile și recunoaște tipul fișierului.
    /// </summary>
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

    /// <summary>
    /// Sincronizează vizual selecția elementului din ListBox cu piesa redată curent.
    /// </summary>
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

    /// <summary>
    /// Event handler apelat atunci când se detectează activitate (mișcarea mouse-ului), 
    /// reafișând panoul de control al player-ului.
    /// </summary>
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

    /// <summary>
    /// Metodă apelată la închiderea ferestrei pentru eliberarea memoriei și oprirea proceselor (dispose).
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
        _uiTimer?.Stop();
        _mediaPlayer?.Dispose();
        _libVLC?.Dispose();
        base.OnClosed(e);
    }

    /// <summary>
    /// Gestionează click-ul pe butonul de Play/Pause. Acțiunea efectivă depinde de starea (State) curentă.
    /// </summary>
    private void PlayPause_OnClick(object? sender, RoutedEventArgs e)
    {
        var newState = _currentState.PlayPause(_mediaPlayer);
        SetState(newState);
    }

    /// <summary>
    /// Gestionează click-ul pe butonul de Stop, oprind media curentă și trecând în StoppedState.
    /// </summary>
    private void Stop_OnClick(object? sender, RoutedEventArgs e)
    {
        var newState = _currentState.Stop(_mediaPlayer);
        SetState(newState);
        var titleLabel = this.FindControl<TextBlock>("TitleLabel");
        if (titleLabel != null) titleLabel.Text = "No media loaded";
    }

    /// <summary>
    /// Trece la următoarea piesă/următorul clip video din playlist.
    /// </summary>
    private void Next_OnClick(object? sender, RoutedEventArgs e) => PlayNextMedia();

    /// <summary>
    /// Trece la piesa anterioară/clipul video anterior din playlist.
    /// </summary>
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

    /// <summary>
    /// Obține fișierul următor din managerul de playlist și îi declanșează redarea.
    /// </summary>
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

    /// <summary>
    /// Gestionează click-ul pe butonul Shuffle. Reordonează elementele și începe redarea noului playlist.
    /// </summary>
    private void Shuffle_OnClick(object? sender, RoutedEventArgs e)
    {
        _playlistManager.Shuffle();
        UpdateMediaListUI();
        PlayNextMedia();
    }

    /// <summary>
    /// Gestionează click-ul pe butonul de Repeat. Comută starea de repetare a playlist-ului.
    /// </summary>
    private void Repeat_OnClick(object? sender, RoutedEventArgs e)
    {
        _playlistManager.Repeat();
    }

    /// <summary>
    /// Event handler pentru dublu-click pe un element din listă. Determină redarea imediată a elementului respectiv.
    /// </summary>
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

    /// <summary>
    /// Deschide un FilePicker pentru adăugarea de fișiere media multiple în playlist-ul curent.
    /// </summary>
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

    /// <summary>
    /// Șterge elementul selectat curent din controlul ListBox aferent playlist-ului.
    /// </summary>
    private void Remove_OnClick(object? sender, RoutedEventArgs e)
    {
        var listBox = this.FindControl<ListBox>("PlaylistBox");
        if (listBox != null && listBox.SelectedIndex != -1)
        {
            _playlistManager.Remove(listBox.SelectedIndex, _currentPlaylistPath);
            UpdateMediaListUI();
        }
    }

    /// <summary>
    /// Deschide un SaveFilePicker pentru a salva starea curentă a playlist-ului sub formă de fișier JSON.
    /// </summary>
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

    /// <summary>
    /// Deschide un OpenFilePicker pentru a încărca un playlist salvat anterior dintr-un fișier JSON.
    /// </summary>
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

            try
            {
                await _playlistManager.Load(_currentPlaylistPath);
                UpdateMediaListUI();

                var currentMedia = _playlistManager.GetCurrent();
                if (currentMedia != null)
                {
                    _mediaPlayer.Media = currentMedia;
                    _mediaPlayer.Play();
                }
            }
            catch (MediaPlayerException customEx)
            {
                Console.WriteLine($"[EROARE {customEx.OperationName}]: {customEx.Message}");
            }
        }
    }

    /// <summary>
    /// Permite selectarea unui fișier local de subtitrare și îl atașează media player-ului (Slave).
    /// </summary>
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

    /// <summary>
    /// Extrage și populează un meniu UI cu track-urile de subtitrare disponibile în fișierul curent.
    /// </summary>
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

    /// <summary>
    /// Oprește temporar actualizarea slider-ului de progres atunci când utilizatorul dă click pe el pentru a căuta (seek).
    /// </summary>
    private void ProgressSlider_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _isDraggingProgress = true;
    }

    /// <summary>
    /// Reia actualizarea slider-ului de progres și modifică poziția redării video la eliberarea click-ului.
    /// </summary>
    private void ProgressSlider_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isDraggingProgress = false;
        var slider = sender as Slider;
        if (slider != null && _mediaPlayer?.Media != null)
        {
            _mediaPlayer.Position = (float)slider.Value;
        }
    }

    /// <summary>
    /// Actualizează volumul player-ului media în funcție de modificarea valorii slider-ului de volum.
    /// </summary>
    private void VolumeSlider_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property.Name == "Value" && _mediaPlayer != null && e.NewValue is double newValue)
        {
            _mediaPlayer.Volume = (int)newValue;
        }
    }

    /// <summary>
    /// Deschide fișierul .chm despre program
    /// </summary>
    private async void About_Click(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "hh.exe",
            Arguments = "AIMediaPlayer_About.chm",
            UseShellExecute = true,
        });
    }


    /// <summary>
    /// Deschide un manual de utilizare pentru utilizatori care vor sa foloseasca aplicatia
    /// </summary>
    private async void UserManual_Click(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "hh.exe",
            Arguments = "AIMediaPlayer_UserManual.chm",
            UseShellExecute = true,
        });
    }
}