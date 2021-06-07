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


        //[SerializeField]
        //private EnemyStatusSprite _Poisoned = null;
        //[SerializeField]
        //private EnemyStatusSprite _Chilled = null;

        #endregion Prefabs


        #region Prefab Properties

        // Increasing DoT
        private EnemyStatusSprite Burning => _Burning;

        //// Constant DoT
        //private EnemyStatusSprite Poisoned => _Poisoned;

        //// Time scale down
        //private EnemyStatusSprite Chilled => _Chilled;

        #endregion Prefab Properties

        protected override List<EnemyStatusSprite> InitialStatusSprites() => new List<EnemyStatusSprite>()
        {
            Burning
        };


        protected override void OnEnemyStatusBarInit()
        {
            Burning.Init();
        }

        protected override void OnEnemyStatusBarSpawn()
        {
            BurningDamage = 0;

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

    }
}