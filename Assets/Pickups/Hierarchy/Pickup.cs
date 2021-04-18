using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.Pickups
{
    public abstract class Pickup : PooledObject
    {
        public override string LogTagColor => "#999999";
        public override TimeScaleType TimeScale => TimeScaleType.Default;

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;

        #endregion Prefab Properties

        #region Prefab Substitutes

        private const float PermanentVelocityY = -1.5f;

        #endregion Prefab Substitutes


        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        protected virtual void OnPickupInit() { }
        protected sealed override void OnInit()
        {
            VelocityY = PermanentVelocityY;
            OnPickupInit();
        }

        protected virtual void OnPickupSpawn() { }
        public sealed override void OnSpawn()
        {
            OnPickupSpawn();
        }

        protected virtual void OnPickupFrameRun(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            OnPickupFrameRun(deltaTime, realDeltaTime);
        }

        protected abstract void OnPickUp();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsPlayer(collision))
            {
                OnPickUp();
                DeactivateSelf();
            }
        }

        protected virtual void OnDestructorDeactivation() { }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (CollisionUtil.IsDestructor(collision) && gameObject.activeSelf)
            {
                OnDestructorDeactivation();
                DeactivateSelf();
            }
        }
    }
}
