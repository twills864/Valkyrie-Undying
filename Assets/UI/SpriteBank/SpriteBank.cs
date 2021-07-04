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
    /// <summary>
    /// Contains all sprites in the game that can be loaded in as resources
    /// and be assigned at runtime.
    /// </summary>
    /// <inheritdoc/>
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

        /// <summary>
        /// upgrade
        /// </summary>
        public static Sprite HeavyWeaponLevelUp { get; private set; }

        public static PowerupSpriteBank Powerups { get; private set; }
        public static HeavyWeaponSpriteBank HeavyWeapons { get; private set; }

        public static void Init()
        {
            Empty = LoadSprite("Sprites/Empty");
            OneUp = LoadSprite("Sprites/heart-plus");
            HeavyWeaponLevelUp = LoadSprite("Sprites/upgrade");
            Powerups = new PowerupSpriteBank();
            HeavyWeapons = new HeavyWeaponSpriteBank();
        }

        private static Sprite LoadSprite(string path)
        {
            Sprite ret = Resources.Load<Sprite>(path);
            return ret;
        }
    }
}
