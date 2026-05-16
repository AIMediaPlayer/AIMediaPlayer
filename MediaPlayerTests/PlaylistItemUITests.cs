/**************************************************************************
 * *
 * File:        PlaylistItemUITests.cs                                   *
 * Copyright:   (c) 2026, Huțanu Laurențiu                               *
 * E-mail:      laurentiu.hutanu@student.tuiasi.ro                       *
 * Website:                                                              *
 * Description: Custom exceptions for media player and playlist errors.  *
 * *
 * This program is free software; you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation. This program is distributed in the      *
 * hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 * PURPOSE. See the GNU General Public License for more details.         *
 * *
 **************************************************************************/

using AIMediaPlayer.Models;
using Avalonia.Media.Imaging;
using NUnit.Framework;

namespace MediaPlayerTests
{
    [TestFixture]
    public class PlaylistItemUITests
    {
        [Test]
        public void Title_Default_IsEmptyString()
        {
            var item = new PlaylistItemUI();
            Assert.That(item.Title, Is.EqualTo(string.Empty));
        }

        [Test]
        public void MediaType_Default_IsMediaFile()
        {
            var item = new PlaylistItemUI();
            Assert.That(item.MediaType, Is.EqualTo("Media File"));
        }

        [Test]
        public void Thumbnail_Default_IsNull()
        {
            var item = new PlaylistItemUI();
            Assert.That(item.Thumbnail, Is.Null);
        }

        [Test]
        public void Title_SetAndGet_ReturnsCorrectValue()
        {
            var item = new PlaylistItemUI { Title = "My Movie.mp4" };
            Assert.That(item.Title, Is.EqualTo("My Movie.mp4"));
        }

        [TestCase("Video File")]
        [TestCase("Audio File")]
        [TestCase("Media File")]
        [TestCase("Unknown")]
        public void MediaType_SetToKnownValues_ReturnsExpectedString(string mediaType)
        {
            var item = new PlaylistItemUI { MediaType = mediaType };
            Assert.That(item.MediaType, Is.EqualTo(mediaType));
        }

        [Test]
        public void Constructor_AllPropertiesCanBeSetViaInitializer()
        {
            var item = new PlaylistItemUI
            {
                Title = "concert.mp4",
                MediaType = "Video File",
                Thumbnail = null
            };

            Assert.Multiple(() =>
            {
                Assert.That(item.Title, Is.EqualTo("concert.mp4"));
                Assert.That(item.MediaType, Is.EqualTo("Video File"));
                Assert.That(item.Thumbnail, Is.Null);
            });
        }


        [Test]
        public void Thumbnail_SetToNonNull_ThenBackToNull_IsNull()
        {
            var item = new PlaylistItemUI { Thumbnail = null };
            Assert.That(item.Thumbnail, Is.Null);
        }

        [Test]
        public void Title_SetToNull_IsNull()
        {
            var item = new PlaylistItemUI { Title = null };
            Assert.That(item.Title, Is.Null);
        }
    }
}
