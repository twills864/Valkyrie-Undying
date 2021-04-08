using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    public class Monsoon : ValkyrieSprite
    {
        public static Monsoon Instance { get; set; }

        public override TimeScaleType TimeScale => TimeScaleType.Player;

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _OffsetFromBottom = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;

        private float Speed => _Speed;

        public float OffsetFromBottom => _OffsetFromBottom;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        private Vector2 Size { get; set; }
        private float BufferX { get; set; }
        private SpriteBoxMap BoxMap { get; set; }

        private LoopingFrameTimer FireTimer { get; } = new LoopingFrameTimer(0.5f);

        private int Level { get; set; }
        private int Damage { get; set; }

        private ObjectPool<PlayerBullet> MonsoonPool { get; set; }

        protected override void OnInit()
        {
            Instance = this;

            var sprite = GetComponent<SpriteRenderer>();
            Size = sprite.size;
            BufferX = Size.x;
            BoxMap = new SpriteBoxMap(this);
            MonsoonPool = PoolManager.Instance.BulletPool.GetPool<RaindropBullet>();
        }

        public void OnSpawn(float xPosition)
        {
            float newX = xPosition;
            float newY = SpaceUtil.WorldMap.Bottom.y + OffsetFromBottom;
            transform.position = new Vector3(newX, newY, 0);
            VelocityX = Speed;
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
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

        public Vector3 FirePosition
        {
            get
            {
                var top = BoxMap.Top;

                var x = top.x + RandomUtil.Float(-BufferX, BufferX);
                var y = top.y;

                var ret = new Vector3(x, y);
                return ret;
            }
        }

        public void CreateRaindrop(Vector3 position, int damage)
        {
            var raindrop = (RaindropBullet) MonsoonPool.Get();
            raindrop.transform.position = position;
            raindrop.RaindropDamage = damage;
        }
    }
}
