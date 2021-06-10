using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI.SpriteBank
{
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
