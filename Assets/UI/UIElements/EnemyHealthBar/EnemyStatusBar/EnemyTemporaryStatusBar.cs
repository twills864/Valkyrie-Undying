using System.Collections;
using System.Collections.Generic;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    /// <summary>
    /// The status bar that will hold status effects that may go away before the enemy dies.
    /// </summary>
    /// <inheritdoc/>
    public class EnemyTemporaryStatusBar : EnemyStatusBarBase
    {
        #region Prefabs

        [SerializeField]
        private EnemyStatusSprite _VoidPaused = null;

        [SerializeField]
        private EnemyStatusSprite _Chilled = null;

        [SerializeField]
        private EnemyStatusSprite _Acidic = null;

        //[SerializeField]
        //private EnemyStatusSprite _Silenced = null;

        //[SerializeField]
        //private EnemyStatusSprite _Shocked = null;

        #endregion Prefabs


        #region Prefab Properties

        // Paused until they exit void
        private EnemyStatusSprite VoidPaused => _VoidPaused;

        // Reduces update speed
        private EnemyStatusSprite Chilled => _Chilled;

        // Decreasing DoT
        private EnemyStatusSprite Acidic => _Acidic;

        //// Can't fire bullets
        //private EnemyStatusSprite Silenced => _Silenced;

        //// Temporarily paused
        //private EnemyStatusSprite Shocked => _Shocked;


        #endregion Prefab Properties

        protected override List<EnemyStatusSprite> InitialStatusSprites() => new List<EnemyStatusSprite>()
        {
            VoidPaused,
            Chilled,
            Acidic,
        };

        protected override void OnEnemyStatusBarInit()
        {
            VoidPaused.Init();
            Chilled.Init();
            Acidic.Init();
        }

        protected override void OnEnemyStatusBarSpawn()
        {

        }


        public bool IsVoidPaused
        {
            get => VoidPaused.Value > 0;
            set
            {
                SetAndRecalculate(VoidPaused, value ? 1 : 0);

                if(!value)
                    RecalculateStatusBar();
            }
        }

        public int ChilledTime
        {
            get => Chilled.Value;
            set => SetAndRecalculate(Chilled, value);
        }

        public int AcidDamage
        {
            get => Acidic.Value;
            set => SetAndRecalculate(Acidic, value);
        }
    }
}