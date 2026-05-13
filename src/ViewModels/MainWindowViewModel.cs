using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AIMediaPlayer.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Uri? _videoSource;

        public ObservableCollection<string> PlaylistItems { get; set; } = new ObservableCollection<string>();

        [RelayCommand]
        private async Task OpenFile(IStorageProvider storageProvider)
        {
            var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Alege un fișier media",
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Fișiere Media")
                    {
                        Patterns = new[] { "*.mp4", "*.mkv", "*.avi", "*.mov", "*.wmv", "*.mp3", "*.wav", "*.m4a" }
                    }
                },
                AllowMultiple = false
            });

            var file = result.FirstOrDefault();
            if (file != null)
            {
                VideoSource = file.Path;
            }
        }
    }
}