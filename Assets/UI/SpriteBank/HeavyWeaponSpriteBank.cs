using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI.SpriteBank
{
    /// <summary>
    /// A SpecializedSpriteBank that contains sprites representing heavy weapon fire strategies.
    /// </summary>
    /// <inheritdoc/>
    public class HeavyWeaponSpriteBank : SpecializedSpriteBank
    {
        protected override string SpritePath => "Sprites/HeavyWeapons";

        /// <summary>
        /// atom
        /// </summary>
        public Sprite Atom { get; private set; }

        /// <summary>
        /// backup
        /// </summary>
        public Sprite OneManArmy { get; private set; }

        /// <summary>
        /// bouncingspring
        /// </summary>
        public Sprite Trampoline { get; private set; }

        /// <summary>
        /// bouncingsword
        /// </summary>
        public Sprite Bounce { get; private set; }

        /// <summary>
        /// hypersonicbolt
        /// </summary>
        public Sprite Bfg { get; private set; }

        /// <summary>
        /// machinegun
        /// </summary>
        public Sprite Gatling { get; private set; }

        /// <summary>
        /// shieldreflect
        /// </summary>
        public Sprite Reflect { get; private set; }

        /// <summary>
        /// shotgunrounds
        /// </summary>
        public Sprite Shotgun { get; private set; }

        /// <summary>
        /// strikingarrows
        /// </summary>
        public Sprite Spread { get; private set; }

        /// <summary>
        /// strikingballs
        /// </summary>
        public Sprite Flak { get; private set; }

        /// <summary>
        /// strikingclamps
        /// </summary>
        public Sprite Burst { get; private set; }

        /// <summary>
        /// strikingdiamonds
        /// </summary>
        public Sprite Diamond { get; private set; }

        /// <summary>
        /// vortex
        /// </summary>
        public Sprite Wormhole { get; private set; }

        public HeavyWeaponSpriteBank()
        {
            LoadHeavyWeapons();
        }

        private void LoadHeavyWeapons()
        {
            Atom = LoadSprite("atom");
            OneManArmy = LoadSprite("backup");
            Trampoline = LoadSprite("bouncing-spring");
            Bounce = LoadSprite("bouncing-sword");
            Bfg = LoadSprite("hypersonic-bolt");
            Gatling = LoadSprite("machine-gun");
            Reflect = LoadSprite("shield-reflect");
            Shotgun = LoadSprite("shotgun-rounds");
            Spread = LoadSprite("striking-arrows");
            Flak = LoadSprite("striking-balls");
            Burst = LoadSprite("striking-clamps");
            Diamond = LoadSprite("striking-diamonds");
            Wormhole = LoadSprite("vortex");
        }
    }
}
