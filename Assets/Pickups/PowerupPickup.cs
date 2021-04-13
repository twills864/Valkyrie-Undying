using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.GameTasks;
using Assets.Powerups;
using Assets.UI.PowerupMenu;
using Assets.Util;
using UnityEngine;

namespace Assets.Pickups
{
    public class PowerupPickup : Pickup
    {
        private const float LeftRotation = 360f;
        private const float RightRotation = -LeftRotation;
        #region Prefabs

        [SerializeField]
        private float _FlyInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private Vector2 _FlyInDistance = Vector2.zero;

        #endregion Prefabs


        #region Prefab Properties

        private float FlyInTime => _FlyInTime;
        private float FlyInDistanceXMax => _FlyInDistance.x;
        private float FlyInDistanceY => _FlyInDistance.y;

        #endregion Prefab Properties

        public Powerup TargetPowerup { get; set; }

        private float FlyInMoveX { set => FlyInMove.DistanceX = value; }
        private MoveBy FlyInMove { get; set; }
        private EaseIn3 FlyIn { get; set; }

        private RotateTo SpawnRotate { get; set; }
        private EaseIn SpawnRotateEase { get; set; }

        protected override void OnPickupInit()
        {
            Vector3 move = new Vector3(0.0f, FlyInDistanceY);
            FlyInMove = new MoveBy(this, move, FlyInTime);
            FlyIn = new EaseIn3(FlyInMove);

            SpawnRotate = new RotateTo(this, 0f, 360f, FlyInTime);
            SpawnRotateEase = new EaseIn(SpawnRotate);
        }

        public override void OnSpawn()
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
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            FlyIn.RunFrame(deltaTime);
            SpawnRotateEase.RunFrame(deltaTime);
        }

        protected override void OnPickUp()
        {
            GameManager.Instance.CreateFleetingText(TargetPowerup.PowerupName, SpaceUtil.WorldMap.Center);

            TargetPowerup.Level++;

            var type = TargetPowerup.GetType();
            PowerupMenu.Instance.SetLevel(type, TargetPowerup.Level);

            DebugUI.Instance.PowerupMenuPowerLevelRowSet(TargetPowerup, TargetPowerup.Level);
        }
    }
}
