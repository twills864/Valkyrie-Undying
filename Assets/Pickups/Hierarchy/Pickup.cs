using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
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

        [SerializeField]
        private float _PermanentVelocityY = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;
        public float PermanentVelocityY => _PermanentVelocityY;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        protected override void OnInit()
        {
            VelocityY = PermanentVelocityY;
        }

        protected abstract void OnPickUp();
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsPlayer(collision))
            {
                GameManager.Instance.CreateFleetingText("PICKUP", SpaceUtil.WorldMap.Center);
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
