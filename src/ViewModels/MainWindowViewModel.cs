/**************************************************************************
 * *
 * File:        MainWindowViewModel.cs                                   *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: ViewModel for the main window, handling media commands.  *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AIMediaPlayer.ViewModels
{
    /// <summary>
    /// Reprezintă ViewModel-ul principal al aplicației media player,
    /// responsabil pentru gestionarea comenzilor și a datelor utilizate
    /// de interfața grafică principală.
    /// </summary>
    /// <remarks>
    /// Clasa implementează logica specifică modelului arhitectural MVVM
    /// (Model-View-ViewModel), facilitând comunicarea dintre interfața
    /// utilizator și serviciile aplicației.
    /// </remarks>
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Uri? _videoSource;

        /// <summary>
        /// Deschide un dialog pentru selectarea unui fișier media de pe disc și setează VideoSource.
        /// </summary>
        /// <param name="storageProvider">Furnizorul de stocare Avalonia utilizat pentru a deschide File Picker-ul.</param>
        /// <returns>Un Task asincron reprezentând operațiunea.</returns>
        public ObservableCollection<string> PlaylistItems { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Deschide un dialog de selecție pentru alegerea unui fișier media
        /// de pe disc și actualizează sursa media curentă a aplicației.
        /// </summary>
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