using System.Collections;
using System.Collections.Generic;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    /// <summary>
    /// The prefab that holds and manages an instance of an EnemyTemporaryStatusBar
    /// and an EnemyPermanentStatusBar.
    /// </summary>
    /// <inheritdoc/>
    public class EnemyStatusBarHolder : UIElement
    {
        protected override ColorHandler DefaultColorHandler() => new NullColorHandler();

        #region Prefabs

        [SerializeField]
        private EnemyTemporaryStatusBar _TemporaryStatuses = null;

        [SerializeField]
        private EnemyPermanentStatusBar _PermanentStatuses = null;

        [SerializeField]
        private float _HealthBarMargin = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private EnemyTemporaryStatusBar TemporaryStatuses => _TemporaryStatuses;
        private EnemyPermanentStatusBar PermanentStatuses => _PermanentStatuses;
        private float HealthBarMargin => _HealthBarMargin;

        #endregion Prefab Properties

        public Assets.UI.EnemyHealthBar HealthBar { get; set; }
        public float HealthBarWidth => HealthBar.TextMesh.GetComponent<Renderer>().bounds.size.x;


        protected override void OnUIElementInit()
        {
            TemporaryStatuses.Init();
            PermanentStatuses.Init();
        }

        public override void OnSpawn()
        {
            transform.localPosition = new Vector3(0, 0, LocalPositionZ);

            float totalOffset = (HealthBarWidth * 0.5f) + HealthBarMargin;

            TemporaryStatuses.LocalPositionX = -totalOffset;
            PermanentStatuses.LocalPositionX = totalOffset;

            TemporaryStatuses.OnSpawn();
            PermanentStatuses.OnSpawn();
        }


        public int BurningDamage
        {
            get => PermanentStatuses.BurningDamage;
            set => PermanentStatuses.BurningDamage = value;
        }

        public int PoisonDamage
        {
            get => PermanentStatuses.PoisonDamage;
            set => PermanentStatuses.PoisonDamage = value;
        }

        public int ParasiteDamage
        {
            get => PermanentStatuses.ParasiteDamage;
            set => PermanentStatuses.ParasiteDamage = value;
        }

        public bool IsVoidPaused
        {
            get => TemporaryStatuses.IsVoidPaused;
            set => TemporaryStatuses.IsVoidPaused = value;
        }

        public int ChilledTime
        {
            get => TemporaryStatuses.ChilledTime;
            set => TemporaryStatuses.ChilledTime = value;
        }

        public int AcidDamage
        {
            get => TemporaryStatuses.AcidDamage;
            set => TemporaryStatuses.AcidDamage = value;
        }
    }
}