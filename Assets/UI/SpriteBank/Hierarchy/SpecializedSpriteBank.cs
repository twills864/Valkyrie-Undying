using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI.SpriteBank
{
    /// <summary>
    /// A class that will contain sprites in the game that can be loaded in as resources
    /// and be assigned at runtime.
    /// </summary>
    public abstract class SpecializedSpriteBank
    {
        protected abstract string SpritePath { get; }

        protected Sprite LoadSprite(string name)
        {
            string path = $"{SpritePath}/{name}";

            Sprite ret = Resources.Load<Sprite>(path);
            return ret;
        }
    }
}
