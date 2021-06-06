using System.Collections;
using System.Collections.Generic;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    public class EnemyStatusBarHolder : UIElement
    {
        protected override ColorHandler DefaultColorHandler() => new NullColorHandler();

        #region Prefabs

        [SerializeField]
        private EnemyTemporaryStatusBar _TemporaryStatuses = null;

        [SerializeField]
        private EnemyPermanentStatusBar _PermanentStatuses = null;

        [SerializeField]
        private float _OffsetFromCenter = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public EnemyTemporaryStatusBar TemporaryStatuses => _TemporaryStatuses;
        public EnemyPermanentStatusBar PermanentStatuses => _PermanentStatuses;
        public float OffsetFromCenter => _OffsetFromCenter;

        #endregion Prefab Properties

        protected override void OnUIElementInit()
        {
            TemporaryStatuses.PositionX = -OffsetFromCenter;
            PermanentStatuses.PositionX = OffsetFromCenter;

            TemporaryStatuses.Init();
            PermanentStatuses.Init();
        }
    }
}