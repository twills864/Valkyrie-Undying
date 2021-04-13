﻿using System;
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
        private const float LeftRotation = 360f;
        private const float RightRotation = -LeftRotation;

        public override string LogTagColor => "#999999";
        public override TimeScaleType TimeScale => TimeScaleType.Default;

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);


        #region Prefab Substitutes

        private const float PermanentVelocityY = -1.5f;
        private const float FlyInTime = 0.8f;
        private const float FlyInDistanceXMax = 1f;
        private const float FlyInDistanceY = 2f;

        #endregion Prefab Substitutes

        private float FlyInMoveX { set => FlyInMove.DistanceX = value; }
        private MoveBy FlyInMove { get; set; }
        private EaseIn3 FlyIn { get; set; }

        private RotateTo SpawnRotate { get; set; }
        private EaseIn SpawnRotateEase { get; set; }



        protected virtual void OnPickupInit() { }
        protected sealed override void OnInit()
        {
            VelocityY = PermanentVelocityY;

            Vector3 move = new Vector3(0.0f, FlyInDistanceY);
            FlyInMove = new MoveBy(this, move, FlyInTime);
            FlyIn = new EaseIn3(FlyInMove);

            SpawnRotate = new RotateTo(this, 0f, 360f, FlyInTime);
            SpawnRotateEase = new EaseIn(SpawnRotate);

            OnPickupInit();
        }

        protected virtual void OnPickupSpawn() { }
        public sealed override void OnSpawn()
        {
            float xMove = RandomUtil.Float(FlyInDistanceXMax);

            bool onLeftOfScreen = PositionX > SpaceUtil.WorldMap.Center.x;
            if (onLeftOfScreen)
            {
                SpawnRotate.EndRotationDegrees = LeftRotation;
                xMove *= -1f;
            }
            else
            {
                SpawnRotate.EndRotationDegrees = RightRotation;
            }

            FlyInMoveX = xMove;

            FlyIn.ResetSelf();

            RotationDegrees = 0f;
            SpawnRotateEase.ResetSelf();

            OnPickupSpawn();
        }

        protected virtual void OnPickupFrameRun(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            FlyIn.RunFrame(deltaTime);
            SpawnRotateEase.RunFrame(deltaTime);

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

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (CollisionUtil.IsDestructor(collision))
                DeactivateSelf();
        }
    }
}
