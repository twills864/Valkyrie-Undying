using System.Collections;
using System.Collections.Generic;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    public abstract class EnemyStatusBarBase : UIElement
    {
        protected sealed override ColorHandler DefaultColorHandler() => new NullColorHandler();

        #region Prefabs

        //[SerializeField]
        //private EnemyStatusSprite _Burning = null;

        #endregion Prefabs


        #region Prefab Properties

        //// Increasing DoT
        //private EnemyStatusSprite Burning => _Burning;

        #endregion Prefab Properties

        protected abstract void OnEnemyStatusBarInit();
        protected sealed override void OnUIElementInit()
        {
            OnEnemyStatusBarInit();
        }
    }
}