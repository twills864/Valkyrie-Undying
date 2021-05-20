using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public static class SoundManager
    {
        private const string PathSoundEffect = "Audio/Sounds/";

        public static AudioSource AudioSource { get; private set; }
        public static AudioClip TestFire { get; private set; }

        public static void Init(AudioSource audioSource)
        {
            AudioSource = audioSource;
            TestFire = LoadSoundEffect("laser3");
        }

        private static AudioClip LoadSoundEffect(string name)
        {
            string path = $"{PathSoundEffect}{name}";

            AudioClip ret = Resources.Load<AudioClip>(path);
            return ret;
        }

        public static void PlayTestFire()
        {
            AudioSource.PlayOneShot(TestFire);
        }
    }
}
