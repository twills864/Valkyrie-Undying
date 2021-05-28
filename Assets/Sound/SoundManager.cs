using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Util;
using UnityEngine;

namespace Assets
{
    public static class SoundManager
    {
        private const string PathSoundEffect = "Audio/Sounds/";

        // Can't set this as a prefab because it has to be consistent between instances.
        public const float MaxSoundEffectVolume = 1.0f;
        private static float SoundEffectVolume { get; set; }

        public static AudioSource AudioSource { get; private set; }

        private const int NumPanSources = 11;
        private const int PanCenterIndex = NumPanSources / 2;
        public static AudioSource[] PanSources { get; set; }

        const float PanOffset = 1f / (NumPanSources * 2);

        public static void Init(AudioSource audioSource)
        {
            AudioSource = audioSource;

            InitPanSources();
            SoundBank.Init();
        }

        private static void InitPanSources()
        {
            PanSources = new AudioSource[NumPanSources];

            SoundEffectVolume = 0.01f * PlayerPrefs.GetInt(PlayerPrefsKeys.SoundEffectVolumeKey, 100);

            for (int i = 0; i < NumPanSources; i++)
            {
                if (i != PanCenterIndex)
                    PanSources[i] = AudioSource.Instantiate(AudioSource);
                else
                    PanSources[i] = AudioSource;

                float f = (float)i;

                // Pan represented from 0f to 2f
                float pan = ((f / NumPanSources) + PanOffset) * 2f;

                // Pan represented from -1f to 1f, as Unity expects
                pan = pan - 1f;

                PanSources[i].panStereo = pan;

            }
        }

        public static void PlaySound(AudioClip clip, float volume = 1.0f)
        {
            float scaledVolume = volume * SoundEffectVolume;
            AudioSource.PlayOneShot(clip, scaledVolume);
        }

        public static void PlaySoundWithPan(AudioClip clip, float pan, float volumeScale = 1.0f)
        {
            // Represent pan from 0f to 2f
            float source = (pan + 1f);

            // Represent as a ratio from 0f to 1f
            source *= 0.5f;

            int sourceIndex = (int)(source * NumPanSources);
            sourceIndex = Mathf.Clamp(sourceIndex, 0, NumPanSources - 1);

            AudioSource audioSource = PanSources[sourceIndex];

            float volume = volumeScale * SoundEffectVolume;
            audioSource.PlayOneShot(clip, volume);

            //DebugUI.SetDebugLabel("SOUND", $"{sourceIndex} {audioSource.panStereo.ToString("0.00")} {source.ToString("0.00")} ({((source-0.5f)*2f).ToString("0.00")})");
        }

        /// <summary>
        /// Sets the volume of the game's sound effect as a <paramref name="percent"/> from 0 to 100.
        /// </summary>
        /// <param name="percent">The game's sound effect volume as a percent between 0 and 100.</param>
        public static void SetSoundEffectVolume(float percent)
        {
            SoundEffectVolume = percent * MaxSoundEffectVolume * 0.01f;

            PlayerPrefs.SetInt(PlayerPrefsKeys.SoundEffectVolumeKey, (int)percent);
        }
    }
}
