using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Powerups;
using UnityEngine;

namespace Assets.UI
{
    public class PowerupGuiIcon : ValkyrieSprite
    {
        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        [SerializeField]
        private TextMesh _LevelText = null;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;
        private TextMesh LevelText => _LevelText;

        #endregion Prefab Properties

        public override TimeScaleType TimeScale => TimeScaleType.Default;
        protected override ColorHandler DefaultColorHandler() => new SpriteColorHandler(Sprite);


        public Powerup Powerup { get; private set; }

        public Vector2 SpriteSize => GetComponent<Renderer>().bounds.size;

        public void Init(Powerup powerup)
        {
            Init();

            Powerup = powerup;
            Sprite.sprite = powerup.PowerupSprite;
        }

        public void UpdateLabel()
        {
            if (Powerup.Level != 1)
                LevelText.text = Powerup.Level.ToString();
            else
                LevelText.text = "";
        }
    }
}
