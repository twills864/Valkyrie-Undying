using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assets.Hierarchy.ColorHandlers;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.Sound
{
    /// <summary>
    /// A singleton GameObject that will play persistent music between scenes.
    /// </summary>
    public class MusicManager : SingletonValkyrieSprite
    {
        private const string PlaylistsTextResourcePath = @"Audio\Music\Playlists";
        private static string PlaylistSerializationPath => $@"{Application.dataPath}\Resources\{PlaylistsTextResourcePath}.txt";

        protected override ColorHandler DefaultColorHandler() => new NullColorHandler();

        #region Prefabs

        [SerializeField]
        private AudioSource _MusicPlayer = null;

        #endregion Prefabs


        #region Prefab Properties

        private AudioSource MusicPlayer => _MusicPlayer;

        #endregion Prefab Properties

        private CircularSelector<string> SongPaths { get; set; }

        private bool ShouldPlayMusic { get; set; }
        private AudioClip CurrentTrack
        {
            get => MusicPlayer.clip;
            set
            {
                MusicPlayer.clip?.UnloadAudioData();

                MusicPlayer.clip = value;
                MusicPlayer.Play();
                BeginLoadingNextTrack();

                TimeSpan duration = TimeSpan.FromSeconds(value.length);
                string dbMessage = $"{value.name.Replace('_', ' ')} ({duration})";
                Debug.Log(dbMessage);
                DebugUI.SetDebugLabel("SONG", dbMessage);
            }
        }
        private AsyncAudioClip NextTrack { get; set; }

        private void BeginLoadingNextTrack() => NextTrack = new AsyncAudioClip(SongPaths.GetAndIncrement());

        protected override void OnSingletonInit()
        {
#if UNITY_EDITOR
            GenerateSoundtrackFile();
            if (MusicPlayer.playOnAwake)
#endif
                LoadMusic();
        }

        #region Soundtrack

#if UNITY_EDITOR
        // All music files will be located under "Resources\Audio\Music\".
        // Each subdirectory will represent a playlist the user can enable or disable.
        // For example, the subdirectory "Resources\Audio\Music\Default\" will contain
        //     the default royalty-free soundtrack.
        // Users may add their own subdirectories representing new playlists.
        private void GenerateSoundtrackFile()
        {
            string FilesPath = $@"{Application.dataPath}\Resources\Audio\Music\";

            DirectoryInfo music = new DirectoryInfo(FilesPath);
            var lists = music.GetDirectories();

            List<Playlist> allPlaylists = lists.Select(x => new Playlist(x)).ToList();

            string[] lines = Playlist.SerializePlaylists(allPlaylists);
            File.WriteAllLines(PlaylistSerializationPath, lines);

        }
#endif

        #endregion Soundtrack

        private void LoadMusic()
        {
            ShouldPlayMusic = true;

#if UNITY_EDITOR
            // Don't call build version because that resource file was initialized when the game started,
            // and won't have the updated changes if any were applied.
            var allPlaylists = Playlist.DeserializePlaylists(PlaylistSerializationPath);
#else
            var allPlaylists = Playlist.DeserializePlaylistsFromBuild(PlaylistsTextResourcePath);
#endif

            SongPaths = new CircularSelector<string>(allPlaylists.SelectMany(x => x.AllResourceNames()));
            RandomUtil.Shuffle(SongPaths);

            CurrentTrack = Resources.Load<AudioClip>(SongPaths.GetAndIncrement());

            Debug.Log(SongPaths.Count);
            for (int i = 0; i < SongPaths.Count; i++)
            {
                var song = SongPaths[i];
                Debug.Log($"[{i}] {song}");

                //NotificationManager.AddNotification(song.name);
            }

            Debug.Log(CurrentTrack.length);
        }

        private void Update()
        {
            if (ShouldPlayMusic && !MusicPlayer.isPlaying)
                CurrentTrack = NextTrack.Clip;
        }
    }
}
