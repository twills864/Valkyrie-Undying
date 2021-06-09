using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    public class EnemyPermanentStatusBar : EnemyStatusBarBase
    {
        #region Property Fields
        private int _burningDamage;
        #endregion Property Fields

        #region Prefabs

        [SerializeField]
        private EnemyStatusSprite _Burning = null;

        [SerializeField]
        private EnemyStatusSprite _Poison = null;

        [SerializeField]
        private EnemyStatusSprite _Parasite = null;

        //[SerializeField]
        //private EnemyStatusSprite _Chilled = null;

        #endregion Prefabs


        #region Prefab Properties

        // Increasing DoT
        private EnemyStatusSprite Burning => _Burning;

        // Constant DoT
        private EnemyStatusSprite Poison => _Poison;

        // Stacking constant DoT
        private EnemyStatusSprite Parasite => _Parasite;

        //// Time scale down
        //private EnemyStatusSprite Chilled => _Chilled;

        #endregion Prefab Properties

        protected override List<EnemyStatusSprite> InitialStatusSprites() => new List<EnemyStatusSprite>()
        {
            Parasite,
            Burning,
            Poison,
        };


        protected override void OnEnemyStatusBarInit()
        {
            SpriteManager.Init();
        }

        protected override void OnEnemyStatusBarSpawn()
        {
            BurningDamage = 0;
            PoisonDamage = 0;

            //// Modify property field values directly because all statuses will be inactive,
            //// and the status bar doesn't need to be recalculated.
            //_burningDamage = 0;

            //RecalculateStatusBar();
        }


        public int BurningDamage
        {
            get => Burning.Value;
            set => SetAndRecalculate(Burning, value);
        }

        public int PoisonDamage
        {
            get => Poison.Value;
            set => SetAndRecalculate(Poison, value);
        }

        public int ParasiteDamage
        {
            get => Parasite.Value;
            set => SetAndRecalculate(Parasite, value);
        }
    }
}