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
    public abstract class EnemyLootPickup : Pickup
    {
        private const float LeftRotation = 360f;
        private const float RightRotation = -LeftRotation;

        #region Prefab Substitutes

        private const float FlyInTime = 0.8f;
        private const float FlyInDistanceXMax = 1f;
        private const float FlyInDistanceY = 2f;

        #endregion Prefab Substitutes

        private float FlyInMoveX { set => FlyInMove.DistanceX = value; }
        private MoveBy FlyInMove { get; set; }
        private EaseIn3 FlyIn { get; set; }

        private RotateTo SpawnRotate { get; set; }
        private EaseIn SpawnRotateEase { get; set; }

        protected virtual void OnEnemyLootPickupInit() { }
        protected sealed override void OnPickupInit()
        {
            Vector3 move = new Vector3(0.0f, FlyInDistanceY);
            FlyInMove = new MoveBy(this, move, FlyInTime);
            FlyIn = new EaseIn3(FlyInMove);

            SpawnRotate = new RotateTo(this, 0f, 360f, FlyInTime);
            SpawnRotateEase = new EaseIn(SpawnRotate);

            OnEnemyLootPickupInit();
        }

        protected virtual void OnEnemyLootPickupSpawn() { }
        protected sealed override void OnPickupSpawn()
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

            OnEnemyLootPickupSpawn();
        }

        protected virtual void OnEnemyLootPickupFrameRun(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnPickupFrameRun(float deltaTime, float realDeltaTime)
        {
            FlyIn.RunFrame(deltaTime);
            SpawnRotateEase.RunFrame(deltaTime);

            OnEnemyLootPickupFrameRun(deltaTime, realDeltaTime);
        }
    }
}
