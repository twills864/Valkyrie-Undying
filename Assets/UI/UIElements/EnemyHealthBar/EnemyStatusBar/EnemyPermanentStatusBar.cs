using System.Collections;
using System.Collections.Generic;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    public class EnemyPermanentStatusBar : EnemyStatusBarBase
    {
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


        protected override void OnEnemyStatusBarInit()
        {
            Burning.Init();
        }
    }
}