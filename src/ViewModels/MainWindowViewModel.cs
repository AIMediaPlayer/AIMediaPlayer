using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using System.Linq;

namespace AIMediaPlayer.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private Uri? _videoSource;

        public event Action? RequestPlay;
        public event Action? RequestPause;

        [RelayCommand]
        private async Task OpenFile(IStorageProvider storageProvider)
        {
            var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Alege un fișier video",
                // Create a custom filter for video files
                FileTypeFilter = new[]
                {
            new FilePickerFileType("Fișiere Video")
            {
                Patterns = new[] { "*.mp4", "*.mkv", "*.avi", "*.mov", "*.wmv" }
            }
        },
                AllowMultiple = false
            });

            var file = result.FirstOrDefault();
            if (file != null)
            {
                VideoSource = file.Path;
                RequestPlay?.Invoke();
            }
        }

        [RelayCommand]
        private void Play() => RequestPlay?.Invoke();

        [RelayCommand]
        private void Pause() => RequestPause?.Invoke();
    }
}