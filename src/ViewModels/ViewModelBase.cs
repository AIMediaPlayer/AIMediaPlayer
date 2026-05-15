/**************************************************************************
 * *
 * File:        ViewModelBase.cs                                         *
 * Copyright:   (c) 2026, Loghin Elisei                                  *
 * E-mail:      elisei.loghin2@student.tuiasi.ro                         *
 * Website:                                                              *
 * Description: Abstract base class for ViewModels (ObservableObject).   *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/
using CommunityToolkit.Mvvm.ComponentModel;

namespace AIMediaPlayer.ViewModels
{
    /// <summary>
    /// Clasa de bază abstractă pentru toate ViewModel-urile din aplicație.
    /// Moștenește <see cref="ObservableObject"/> pentru a oferi funcționalitatea 
    /// necesară notificării interfeței grafice (UI) la schimbarea proprietăților (Data Binding).
    /// </summary>
    public abstract class ViewModelBase : ObservableObject
    { 
    }
}
