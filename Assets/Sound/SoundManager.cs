using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets
{
    public static class SoundManager
    {
        private const string PathSoundEffect = "Audio/Sounds/";

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

        public static void PlaySound(AudioClip clip)
        {
            AudioSource.PlayOneShot(clip);
        }

        public static void PlaySoundWithVolume(AudioClip clip, float volume)
        {
            AudioSource.PlayOneShot(clip, volume);
        }

        public static void PlaySoundWithPan(AudioClip clip, float pan)
        {
            // Represent pan from 0f to 2f
            float source = (pan + 1f);

            // Represent as a ratio from 0f to 1f
            source *= 0.5f;

            int sourceIndex = (int)(source * NumPanSources);
            sourceIndex = Mathf.Clamp(sourceIndex, 0, NumPanSources - 1);

            AudioSource audioSource = PanSources[sourceIndex];
            audioSource.PlayOneShot(clip);

            DebugUI.SetDebugLabel("SOUND", $"{sourceIndex} {audioSource.panStereo.ToString("0.00")} {source.ToString("0.00")} ({((source-0.5f)*2f).ToString("0.00")})");
        }
    }
}
