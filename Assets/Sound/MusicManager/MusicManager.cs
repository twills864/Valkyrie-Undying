using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.Sound
{
    /// <summary>
    /// A singleton GameObject that will play persistent music between scenes.
    /// </summary>
    public class MusicManager : MonoBehaviour
    {
        private static string PlaylistSerializationPath => $@"{Application.dataPath}\Resources\Audio\Music\Playlists.txt";

        #region Singleton

        public static MusicManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
            }
        }

        #endregion Singleton

        #region Prefabs

        [SerializeField]
        private AudioSource _MusicPlayer = null;

        #endregion Prefabs


        #region Prefab Properties

        private AudioSource MusicPlayer => _MusicPlayer;

        #endregion Prefab Properties



        private AudioClip[] Songs { get; set; }

        private AudioClip CurrentTrack { get; set; }
        private AsyncAudioClip NextTrack { get; set; }

        private void Init()
        {
#if UNITY_EDITOR
            GenerateSoundtrackFile();
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
            const string MusicPath = @"Audio\Music\Default";

            if (MusicPlayer.playOnAwake)
            {
                Songs = Resources.LoadAll<AudioClip>(MusicPath);

                RandomUtil.Shuffle(Songs);

                Debug.Log(Songs.Length);
                for (int i = 0; i < Songs.Length; i++)
                {
                    var song = Songs[i];
                    Debug.Log($"[{i}] {song.name} ({song.length})");

                    //NotificationManager.AddNotification(song.name);
                }

                MusicPlayer.clip = Songs.First();
                MusicPlayer.Play();
            }

            var allLines = File.ReadAllLines(PlaylistSerializationPath);
            var test = Playlist.DeserializePlaylists(allLines);
        }
    }
}
