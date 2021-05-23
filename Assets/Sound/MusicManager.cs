using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Sound
{
    /// <summary>
    /// A singleton GameObject that will play persistent music between scenes.
    /// </summary>
    public class MusicManager : MonoBehaviour
    {
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

        public void Init()
        {
            const string MusicPath = @"Audio\Music\Default";

            Songs = Resources.LoadAll<AudioClip>(MusicPath);

            RandomUtil.Shuffle(Songs);

            Debug.Log(Songs.Length);
            for(int i = 0; i < Songs.Length; i++)
            {
                var song = Songs[i];
                Debug.Log($"[{i}] {song.name} ({song.length})");
            }

            MusicPlayer.clip = Songs.First();
            MusicPlayer.Play();
        }
    }
}
