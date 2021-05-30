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

        public Vector2 Size { get; private set; }
        private bool HasBeenPickedUp { get; set; }

        protected virtual void OnPickupInit() { }
        protected sealed override void OnInit()
        {
            VelocityY = PermanentVelocityY;

            var renderer = GetComponent<Renderer>();
            Size = renderer.bounds.size;

            OnPickupInit();
        }

        protected virtual void OnPickupSpawn() { }
        public sealed override void OnSpawn()
        {
            HasBeenPickedUp = false;
            OnPickupSpawn();
        }

        protected virtual void OnPickupFrameRun(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            OnPickupFrameRun(deltaTime, realDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayer(collision))
                PickUp();
        }

        protected abstract void OnPickUp();
        public void PickUp()
        {
            if (!HasBeenPickedUp)
            {
                HasBeenPickedUp = true;

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
