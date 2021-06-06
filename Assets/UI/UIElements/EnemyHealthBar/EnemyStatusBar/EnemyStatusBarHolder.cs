using System.Collections;
using System.Collections.Generic;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    public class EnemyStatusBarHolder : UIElement
    {
        protected override ColorHandler DefaultColorHandler() => new NullColorHandler();

        #region Prefabs

        [SerializeField]
        private EnemyStatusBar _Left = null;

        #endregion Prefabs


        #region Prefab Properties

        public EnemyStatusBar Left => _Left;

        #endregion Prefab Properties

        private EnemyStatusBar Right { get; set; }

        protected override void OnUIElementInit()
        {
            Right = Instantiate(Left);
        }
    }
}