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

        private EnemyTemporaryStatusBar TemporaryStatuses => _TemporaryStatuses;
        private EnemyPermanentStatusBar PermanentStatuses => _PermanentStatuses;
        private float OffsetFromCenter => _OffsetFromCenter;

        #endregion Prefab Properties


        protected override void OnUIElementInit()
        {
            TemporaryStatuses.LocalPositionX = -OffsetFromCenter;
            PermanentStatuses.LocalPositionX = OffsetFromCenter;

            TemporaryStatuses.Init();
            PermanentStatuses.Init();
        }

        public override void OnSpawn()
        {
            transform.localPosition = new Vector3(0, 0, LocalPositionZ);
            TemporaryStatuses.OnSpawn();
            PermanentStatuses.OnSpawn();
        }


        public int BurningDamage
        {
            get => PermanentStatuses.BurningDamage;
            set => PermanentStatuses.BurningDamage = value;
        }

        public bool IsVoidPaused
        {
            get => TemporaryStatuses.IsVoidPaused;
            set => TemporaryStatuses.IsVoidPaused = value;
        }
    }
}