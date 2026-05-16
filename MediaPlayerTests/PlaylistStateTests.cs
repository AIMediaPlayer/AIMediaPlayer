/**************************************************************************
 * *
 * File:        PlaylistStateTests.cs                                    *
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
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

namespace MediaPlayerTests
{

    //  Default / property tests
    [TestFixture]
    public class PlaylistItemStateTests
    {

        [Test]
        public void Mrl_SetAndGet_ReturnsCorrectValue()
        {
            var item = new PlaylistItemState { Mrl = "file:///music/song.mp3" };
            Assert.That(item.Mrl, Is.EqualTo("file:///music/song.mp3"));
        }

        [Test]
        public void Title_SetAndGet_ReturnsCorrectValue()
        {
            var item = new PlaylistItemState { Title = "My Song" };
            Assert.That(item.Title, Is.EqualTo("My Song"));
        }

        [Test]
        public void ThumbnailPath_SetAndGet_ReturnsCorrectValue()
        {
            var item = new PlaylistItemState { ThumbnailPath = @"C:\thumbs\cover.jpg" };
            Assert.That(item.ThumbnailPath, Is.EqualTo(@"C:\thumbs\cover.jpg"));
        }

        [Test]
        public void JsonSerialization_RoundTrip_PreservesAllFields()
        {
            var original = new PlaylistItemState
            {
                Mrl = "file:///media/video.mp4",
                Title = "Test Video",
                ThumbnailPath = "/tmp/thumb.png"
            };

            string json = JsonConvert.SerializeObject(original);
            var restored = JsonConvert.DeserializeObject<PlaylistItemState>(json);

            Assert.Multiple(() =>
            {
                Assert.That(restored.Mrl, Is.EqualTo(original.Mrl));
                Assert.That(restored.Title, Is.EqualTo(original.Title));
                Assert.That(restored.ThumbnailPath, Is.EqualTo(original.ThumbnailPath));
            });
        }

        [Test]
        public void JsonDeserialization_MissingFields_DefaultsToNull()
        {
            string json = "{}";
            var item = JsonConvert.DeserializeObject<PlaylistItemState>(json);

            Assert.Multiple(() =>
            {
                Assert.That(item.Mrl, Is.Null);
                Assert.That(item.Title, Is.Null);
                Assert.That(item.ThumbnailPath, Is.Null);
            });
        }
    }

    [TestFixture]
    public class PlaylistStateTests
    {
        [Test]
        public void CurrentIndex_SetAndGet_ReturnsCorrectValue()
        {
            var state = new PlaylistState { CurrentIndex = 3 };
            Assert.That(state.CurrentIndex, Is.EqualTo(3));
        }

        [Test]
        public void Items_SetAndGet_ReturnsCorrectList()
        {
            var items = new List<PlaylistItemState>
            {
                new PlaylistItemState { Title = "Track A" },
                new PlaylistItemState { Title = "Track B" }
            };

            var state = new PlaylistState { CurrentIndex = 0, Items = items };

            Assert.Multiple(() =>
            {
                Assert.That(state.Items.Count, Is.EqualTo(2));
                Assert.That(state.Items[0].Title, Is.EqualTo("Track A"));
                Assert.That(state.Items[1].Title, Is.EqualTo("Track B"));
            });
        }

        [Test]
        public void Items_DefaultsToNull_WhenNotAssigned()
        {
            var state = new PlaylistState { CurrentIndex = 0 };
            Assert.That(state.Items, Is.Null);
        }

        [Test]
        public void JsonSerialization_RoundTrip_PreservesIndexAndItems()
        {
            var original = new PlaylistState
            {
                CurrentIndex = 1,
                Items = new List<PlaylistItemState>
                {
                    new PlaylistItemState { Mrl = "file:///a.mp3", Title = "A", ThumbnailPath = null },
                    new PlaylistItemState { Mrl = "file:///b.mp3", Title = "B", ThumbnailPath = "/b.png" }
                }
            };

            string json = JsonConvert.SerializeObject(original, Formatting.Indented);
            var restored = JsonConvert.DeserializeObject<PlaylistState>(json);

            Assert.Multiple(() =>
            {
                Assert.That(restored.CurrentIndex, Is.EqualTo(1));
                Assert.That(restored.Items.Count, Is.EqualTo(2));
                Assert.That(restored.Items[0].Title, Is.EqualTo("A"));
                Assert.That(restored.Items[1].Mrl, Is.EqualTo("file:///b.mp3"));
                Assert.That(restored.Items[1].ThumbnailPath, Is.EqualTo("/b.png"));
            });
        }
    }

}
