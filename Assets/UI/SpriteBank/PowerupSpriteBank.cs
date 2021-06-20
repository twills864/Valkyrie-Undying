using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI.SpriteBank
{
    public class PowerupSpriteBank : SpecializedSpriteBank
    {
        protected override string SpritePath => "Sprites/Powerups";

        /// <summary>
        /// NULL
        /// </summary>
        public Sprite Blank { get; private set; }

        /// <summary>
        /// blood
        /// </summary>
        public Sprite Bloodlust { get; private set; }

        /// <summary>
        /// bulletimpacts
        /// </summary>
        public Sprite PiercingRounds { get; private set; }

        /// <summary>
        /// contract
        /// </summary>
        public Sprite Jettison { get; private set; }

        /// <summary>
        /// crosshair
        /// </summary>
        public Sprite Victim { get; private set; }

        /// <summary>
        /// explodingplanet
        /// </summary>
        public Sprite Shrapnel { get; private set; }

        /// <summary>
        /// flametunnel
        /// </summary>
        public Sprite Inferno { get; private set; }

        /// <summary>
        /// guards
        /// </summary>
        public Sprite Sentinel { get; private set; }

        /// <summary>
        /// heavyrain
        /// </summary>
        public Sprite Monsoon { get; private set; }

        /// <summary>
        /// ice-iris
        /// </summary>
        public Sprite CryogenicRounds { get; private set; }

        /// <summary>
        /// lightningdissipation
        /// </summary>
        public Sprite Smite { get; private set; }

        /// <summary>
        /// mirrormirror
        /// </summary>
        public Sprite Othello { get; private set; }

        /// <summary>
        /// mite
        /// </summary>
        public Sprite InfestedRounds { get; private set; }

        /// <summary>
        /// mortar
        /// </summary>
        public Sprite Mortar { get; private set; }

        /// <summary>
        /// portal
        /// </summary>
        public Sprite Void { get; private set; }

        /// <summary>
        /// reloadgunbarrel
        /// </summary>
        public Sprite FireSpeed { get; private set; }

        /// <summary>
        /// returnarrow
        /// </summary>
        public Sprite Rebound { get; private set; }

        /// <summary>
        /// shatter
        /// </summary>
        public Sprite Splinter { get; private set; }

        /// <summary>
        /// slap
        /// </summary>
        public Sprite PestControl { get; private set; }

        /// <summary>
        /// snaketongue
        /// </summary>
        public Sprite SnakeBite { get; private set; }

        /// <summary>
        /// splitarrows
        /// </summary>
        public Sprite Split { get; private set; }

        /// <summary>
        /// stonetower
        /// </summary>
        public Sprite Parapet { get; private set; }

        /// <summary>
        /// supersonicbullet
        /// </summary>
        public Sprite AugmentedRounds { get; private set; }

        /// <summary>
        /// voodoodoll
        /// </summary>
        public Sprite CollectivePunishment { get; private set; }

        public PowerupSpriteBank()
        {
            LoadPowerups();
        }

        private void LoadPowerups()
        {
            Blank = null;
            Bloodlust = LoadSprite("blood");
            PiercingRounds = LoadSprite("bullet-impacts");
            Jettison = LoadSprite("contract");
            Victim = LoadSprite("crosshair");
            Shrapnel = LoadSprite("exploding-planet");
            Inferno = LoadSprite("flame-tunnel");
            Sentinel = LoadSprite("guards");
            Monsoon = LoadSprite("heavy-rain");
            CryogenicRounds = LoadSprite("ice-iris");
            Smite = LoadSprite("lightning-dissipation");
            Othello = LoadSprite("mirror-mirror");
            InfestedRounds = LoadSprite("mite");
            Mortar = LoadSprite("mortar");
            Void = LoadSprite("portal");
            FireSpeed = LoadSprite("reload-gun-barrel");
            Rebound = LoadSprite("return-arrow");
            Splinter = LoadSprite("shatter");
            PestControl = LoadSprite("slap");
            SnakeBite = LoadSprite("snake-tongue");
            Split = LoadSprite("split-arrows");
            Parapet = LoadSprite("stone-tower");
            AugmentedRounds = LoadSprite("supersonic-bullet");
            CollectivePunishment = LoadSprite("voodoo-doll");
        }
    }
}
