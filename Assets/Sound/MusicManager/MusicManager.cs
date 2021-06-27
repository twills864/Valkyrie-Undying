using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assets.Constants;
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
        private const string AudioMusicResourcePath = @"Audio\Music\";

        private const string DefaultPlaylistTextResourcePath = AudioMusicResourcePath + "DefaultPlaylist";
        private const string ExtraPlaylistsTextResourcePath = AudioMusicResourcePath + "ExtraPlaylists";

        private static string DefaultPlaylistSerializationPath => $@"{Application.dataPath}\Resources\{DefaultPlaylistTextResourcePath}.txt";
        private static string ExtraPlaylistsSerializationPath => $@"{Application.dataPath}\Resources\{ExtraPlaylistsTextResourcePath}.txt";

        // Can't set this as a prefab because it has to be consistent between instances.
        public const float MaxMusicVolume = 0.8f;

        public static MusicManager MusicManagerInstance => (MusicManager)Instance;
        public static List<Playlist> AllPlaylists => new List<Playlist>(MusicManagerInstance.AllPlaylistsSource);

        protected override ColorHandler DefaultColorHandler() => new NullColorHandler();

        #region Prefabs

        [SerializeField]
        private AudioSource _MusicPlayer = null;

        #endregion Prefabs


        #region Prefab Properties

        private AudioSource MusicPlayer => _MusicPlayer;

        #endregion Prefab Properties

        private List<Playlist> AllPlaylistsSource { get; set; }
        private CircularSelector<AudioClip> Songs { get; set; }

        private bool ShouldPlayMusic { get; set; }
        private AudioClip CurrentTrack
        {
            get => MusicPlayer.clip;
            set
            {
                MusicPlayer.clip = value;
                MusicPlayer.Play();

                TimeSpan duration = TimeSpan.FromSeconds(value.length);
                string dbMessage = $"{value.name.Replace('_', ' ')} ({duration})";
                Debug.Log(dbMessage);
                DebugUI.SetDebugLabel("SONG", dbMessage);
            }
        }
        private AsyncAudioClip NextTrack { get; set; }

        protected override void OnSingletonInit()
        {
            SetMusicVolume(PlayerPrefs.GetInt(PlayerPrefsUtil.MusicVolumeKey, 100));

#if UNITY_EDITOR
            GenerateSoundtrackFiles();
#endif
            LoadMusic();

#if UNITY_EDITOR
            if (MusicPlayer.playOnAwake)
#endif
                ReloadActiveSongs();
        }

        #region Soundtrack

#if UNITY_EDITOR
        // All music files will be located under "Resources\Audio\Music\".
        // Each subdirectory will represent a playlist the user can enable or disable.
        // For example, the subdirectory "Resources\Audio\Music\Default\" will contain
        //     the default royalty-free soundtrack.
        // Users may add their own subdirectories representing new playlists.
        // Additional user playlists will be saved under ExtraPlaylists.txt,
        // while the default soundtrack will be saved under DefaultPlaylist.txt.
        private void GenerateSoundtrackFiles()
        {
            string FilesPath = $@"{Application.dataPath}\Resources\Audio\Music\";

            DirectoryInfo music = new DirectoryInfo(FilesPath);
            DirectoryInfo[] lists = music.GetDirectories();

            List<Playlist> allPlaylists = lists
                .Select(x => new Playlist(x)).ToList();

            // One playlist, but Playlist.SerialzePlaylists expects a List<Playist>
            List<Playlist> defaultPlaylist = allPlaylists.Where(x => x.Name == Playlist.DefaultPlaylistName).ToList();
            string[] defaultLines = Playlist.SerializePlaylists(defaultPlaylist);
            File.WriteAllLines(DefaultPlaylistSerializationPath, defaultLines);

            List<Playlist> extraPlaylists = allPlaylists.Where(x => x.Name != Playlist.DefaultPlaylistName).ToList();
            string[] extraLines = Playlist.SerializePlaylists(extraPlaylists);
            File.WriteAllLines(ExtraPlaylistsSerializationPath, extraLines);
        }
#endif

        #endregion Soundtrack

        private void LoadMusic()
        {
#if UNITY_EDITOR
            // Don't call build version because that resource file was initialized when the game started,
            // and won't have the updated changes if any were applied.
            AllPlaylistsSource = Playlist.DeserializePlaylists(DefaultPlaylistSerializationPath, ExtraPlaylistsSerializationPath);
#else

            AllPlaylistsSource = Playlist.DeserializePlaylistsFromBuild(DefaultPlaylistTextResourcePath, ExtraPlaylistsTextResourcePath);
#endif
        }

        public void ReloadActiveSongs()
        {
            ShouldPlayMusic = true;

            List<string> activePlaylists = Playlist.ActivePlaylists;
            List<string> songPaths = AllPlaylistsSource
                .Where(playlist => activePlaylists.Contains(playlist.Name))
                .SelectMany(x => x.AllResourceNames()).ToList();
            RandomUtil.Shuffle(songPaths);

            Songs = new CircularSelector<AudioClip>(songPaths.Select(x => Resources.Load<AudioClip>(x)));
            CurrentTrack = Songs.GetAndIncrement();
        }

        private void Update()
        {
            if (ShouldPlayMusic && !MusicPlayer.isPlaying)
                CurrentTrack = Songs.GetAndIncrement();
        }

        /// <summary>
        /// Sets the volume of the game's music as a <paramref name="percent"/> from 0 to 100.
        /// </summary>
        /// <param name="percent">The game music volume as a percent between 0 and 100.</param>
        public static void SetMusicVolume(float percent)
        {
            float volume = percent * MaxMusicVolume * 0.01f;
            MusicManagerInstance.MusicPlayer.volume = volume;

            PlayerPrefs.SetInt(PlayerPrefsUtil.MusicVolumeKey, (int)percent);
        }
    }
}
