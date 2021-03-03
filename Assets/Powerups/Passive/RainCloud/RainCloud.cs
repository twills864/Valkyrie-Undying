using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    public class RainCloud : ManagedVelocityObject
    {
        public static RainCloud Instance { get; set; }

        [SerializeField]
        private float Speed;

        [SerializeField]
        private float _OffsetFromBottom;
        public float OffsetFromBottom => _OffsetFromBottom;

        private Vector2 Size { get; set; }
        private float BufferX;
        private SpriteBoxMap BoxMap;

        private LoopingFrameTimer FireTimer = new LoopingFrameTimer(0.5f);

        private int Level;
        private int Damage;

        private ObjectPool<PlayerBullet> RaindropPool { get; set; }

        public override void OnInit()
        {
            var sprite = GetComponent<SpriteRenderer>();
            Size = sprite.size;
            BufferX = Size.x;
            BoxMap = new SpriteBoxMap(this);
            RaindropPool = PoolManager.Instance.BulletPool.GetPool<RaindropBullet>();
        }

        public void OnSpawn(float xPosition)
        {
            float newX = xPosition;
            float newY = SpaceUtil.WorldMap.Bottom.y + OffsetFromBottom;
            transform.position = new Vector3(newX, newY, 0);
            VelocityX = Speed;
        }

        protected override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            var targetX = Player.Instance.transform.position.x;

            var diffX = transform.position.x - targetX;

            if (Mathf.Abs(diffX) > BufferX)
            {
                if (transform.position.x < targetX)
                    VelocityX = Speed;
                else
                    VelocityX = -Speed;
            }

            if (FireTimer.UpdateActivates(deltaTime))
                CreateRaindrop(FirePosition, Damage);
        }

        public void Activate(float xPosition)
        {
            gameObject.SetActive(true);
            OnSpawn(xPosition);
        }

        public void LevelUp(int damage, float fireSpeed)
        {
            Damage = damage;
            FireTimer.ActivationInterval = fireSpeed;
        }

        public Vector2 FirePosition
        {
            get
            {
                var top = BoxMap.Top;

                var x = top.x + RandomUtil.Float(-BufferX, BufferX);
                var y = top.y;

                var ret = new Vector2(x, y);
                return ret;
            }
        }


        public void CreateRaindrop(Vector2 position, int damage)
        {
            var raindrop = (RaindropBullet) RaindropPool.Get();
            raindrop.transform.position = position;
            raindrop.RaindropDamage = damage;
        }
    }
}
