using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Sound
{
    /// <summary>
    /// Represents an AudioClip that will be loaded asynchronously.
    /// </summary>
    public class AsyncAudioClip
    {
        public AudioClip Clip => (AudioClip) Request.asset;
        private ResourceRequest Request { get; }

        public AsyncAudioClip(string path)
        {
            Request = Resources.LoadAsync<AudioClip>(path);
        }
    }
}
