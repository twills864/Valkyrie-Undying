using System.Collections;
using System.Collections.Generic;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    public class EnemyStatusBar : UIElement
    {
        protected override ColorHandler DefaultColorHandler() => new NullColorHandler();

        #region Prefabs

        [SerializeField]
        private EnemyStatusSprite _Burning = null;

        #endregion Prefabs


        #region Prefab Properties

        // Increasing DoT
        private EnemyStatusSprite Burning => _Burning;

        #endregion Prefab Properties

        //// Constant DoT
        //private EnemyStatusSprite Poisoned { get; set; }

        //// Decreasing DoT
        //private EnemyStatusSprite Acidic { get; set; }

        //// Can't fire bullets
        //private EnemyStatusSprite Silenced { get; set; }

        //// Temporarily paused
        //private EnemyStatusSprite Shocked { get; set; }

        //// Paused until they exit void
        //private EnemyStatusSprite VoidPaused { get; set; }

        //// Time scale down
        //private EnemyStatusSprite Chilled { get; set; }

        protected override void OnUIElementInit()
        {
            //Poisoned = Instantiate(Burning);
        }
    }
}