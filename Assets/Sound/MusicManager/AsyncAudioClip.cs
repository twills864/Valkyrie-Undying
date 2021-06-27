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
        public AudioClip Clip { get; private set; }

        public bool IsLoaded
        {
            get
            {
                if (Request.isDone)
                {
                    SetClip();
                    return true;
                }
                else
                    return false;
            }
        }

        public void BeginLoading()
        {
            if (Request == null)
                Request = Resources.LoadAsync<AudioClip>(Path);
        }

        public void ForceLoad()
        {
            if (Request == null)
            {
                Request = Resources.LoadAsync<AudioClip>(Path);
                //Request.priority = (int)ThreadPriority.High;
                SetClip();
            }
        }

        public void SetClip() => Clip = (AudioClip)Request.asset;

        private string Path { get; }
        private ResourceRequest Request { get; set; }

        public AsyncAudioClip(string path)
        {
            Path = path;
        }
    }
}
