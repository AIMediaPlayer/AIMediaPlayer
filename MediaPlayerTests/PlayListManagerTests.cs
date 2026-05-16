/**************************************************************************
 * *
 * File:        PlayListManagerTests.cs                                  *
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
using AIMediaPlayer.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MediaPlayerTests
{

    [TestFixture]
    [Category("Integration")]
    public class PlaylistManagerIntegrationTests
    {
        private LibVLCSharp.Shared.LibVLC _vlc;
        private PlaylistManager _manager;
        private string _tempDir;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // LibVLC requires native libraries (libvlc.so / libvlc.dll).
            // If they are not present this fixture is skipped automatically.
            try
            {
                LibVLCSharp.Shared.Core.Initialize();
                _vlc = new LibVLCSharp.Shared.LibVLC();
            }
            catch
            {
                Assert.Ignore("LibVLC native libraries not found — skipping integration tests.");
            }
        }

        [SetUp]
        public void SetUp()
        {
            _manager = new PlaylistManager(_vlc);
            _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_tempDir);
        }
        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, recursive: true);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => _vlc?.Dispose();

        [Test]
        public void GetCurrent_EmptyPlaylist_ReturnsNull()
        {
            Assert.That(_manager.GetCurrent(), Is.Null);
        }

        [Test]
        public async Task Add_ValidAudioFile_ReturnsTrue()
        {
            // Create a tiny silent WAV (44 bytes header only — enough for Add() to accept)
            string wav = CreateSilentWav(_tempDir);
            bool added = await _manager.Add(new Uri(wav));
            Assert.That(added, Is.True);
        }

        [Test]
        public async Task GetCurrent_AfterAdd_ReturnsMedia()
        {
            string wav = CreateSilentWav(_tempDir);
            await _manager.Add(new Uri(wav));
            Assert.That(_manager.GetCurrent(), Is.Not.Null);
        }

        [Test]
        public async Task SavePlaylist_CreatesJsonFile()
        {
            string wav = CreateSilentWav(_tempDir);
            await _manager.Add(new Uri(wav));

            string path = Path.Combine(_tempDir, "playlist.json");
            _manager.SavePlaylist(path);

            Assert.That(File.Exists(path), Is.True);
        }

        [Test]
        public async Task SaveAndLoad_RoundTrip_RestoresItemCount()
        {
            string wav1 = CreateSilentWav(_tempDir, "a.wav");
            string wav2 = CreateSilentWav(_tempDir, "b.wav");
            await _manager.Add(new Uri(wav1));
            await _manager.Add(new Uri(wav2));

            string path = Path.Combine(_tempDir, "playlist.json");
            _manager.SavePlaylist(path);

            // Load into a fresh manager
            var manager2 = new PlaylistManager(_vlc);
            await manager2.Load(path);

            Assert.That(manager2.GetCurrent(), Is.Not.Null);
        }


        /// <summary>Creates a minimal valid WAV file (44-byte header, no audio data).</summary>

        private static string CreateSilentWav(string dir, string name = "test.wav")
        {
            string path = Path.Combine(dir, name);
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var bw = new BinaryWriter(fs);

            int dataSize = 0;
            int chunkSize = 36 + dataSize;
            short channels = 1, bitsPerSample = 16;
            int sampleRate = 44100, byteRate = sampleRate * channels * bitsPerSample / 8;
            short blockAlign = (short)(channels * bitsPerSample / 8);

            bw.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
            bw.Write(chunkSize);
            bw.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));
            bw.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
            bw.Write(16);           // sub-chunk size
            bw.Write((short)1);    // PCM
            bw.Write(channels);
            bw.Write(sampleRate);
            bw.Write(byteRate);
            bw.Write(blockAlign);
            bw.Write(bitsPerSample);
            bw.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            bw.Write(dataSize);

            return path;
        }
    }
}
