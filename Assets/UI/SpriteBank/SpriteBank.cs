using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets
{
    public static class SpriteBank
    {
        /// <summary>
        /// Empty
        /// </summary>
        public static Sprite Empty { get; private set; }

        /// <summary>
        /// heart-plus
        /// </summary>
        public static Sprite OneUp { get; private set; }

        public static PowerupSpriteBank Powerups { get; private set; }

        public static void Init()
        {
            Empty = LoadSprite("Sprites/Empty");
            OneUp = LoadSprite("Sprites/heart-plus");
            Powerups = new PowerupSpriteBank();
        }

        private static Sprite LoadSprite(string path)
        {
            Sprite ret = Resources.Load<Sprite>(path);
            return ret;
        }
    }
}
