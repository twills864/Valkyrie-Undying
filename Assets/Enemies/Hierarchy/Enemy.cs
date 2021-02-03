using Assets;
using Assets.Bullets;
using Assets.EnemyFireStrategies;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Enemies
{

    public abstract class Enemy : PooledObject
    {
        public override string LogTagColor => "#FFB697";

        public int PointValue { get; set; }
        public int CurrentHealth { get; set; }

        // The health this enemy spawns with at the start of the game
        public abstract int BaseSpawnHealth { get; }

        // The rate at which the spawn health of this enemy increases as the game progresses
        public abstract float SpawnHealthScaleRate { get; }

        [SerializeField]
        public EnemyHealthBar HealthBar;

        public virtual Vector2 FirePosition => BoxMap.Bottom;

        public void UpdateHealthBar() => HealthBar.SetText(CurrentHealth);

        protected TrackedBoxMap BoxMap { get; set; }

        protected abstract EnemyFireStrategy DefaultEnemyFireStrategy { get; }
        public EnemyFireStrategy FireStrategy { get; protected set; }


        protected virtual LoopingFrameTimer DefaultFireTimer => DefaultEnemyFireStrategy.FireTimer;
        public LoopingFrameTimer FireTimer { get; protected set; }

        public override void Init()
        {
            base.Init();
            OnActivate();
        }

        protected override void OnActivate()
        {
            BoxMap = new TrackedBoxMap(this);
            FireStrategy = DefaultEnemyFireStrategy;
            FireTimer = DefaultFireTimer;

            CurrentHealth = BaseSpawnHealth;
            PointValue = CurrentHealth;

            HealthBar = FindChildEnemyHealthBar();
            UpdateHealthBar();
        }

        public override void RunFrame(float deltaTime)
        {
            if(FireTimer.UpdateActivates(deltaTime))
            {
                var bullets = FireStrategy.GetBullets();
                foreach (var bullet in bullets)
                    bullet.transform.position = FirePosition;
            }
        }

        private void Start()
        {

            Init();
        }

        public bool DamageKills(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                return true;

            UpdateHealthBar();
            return false;
        }
        protected virtual void CollideWithBullet(Bullet bullet)
        {
            if (DamageKills(bullet.Damage))
            {
                // Kill enemy
                DeactivateSelf();
                CreateFleetingTextAtCenter(PointValue);
            }
            bullet.DeactivateSelf();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayerBullet(collision))
            {
                Bullet bullet = collision.GetComponent<Bullet>();
                CollideWithBullet(bullet);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (CollisionUtil.IsDestructor(collision))
            {
                CollideWithDestructor();
            }
        }
        protected virtual void CollideWithDestructor()
        {
            DeactivateSelf();
        }

        private EnemyHealthBar FindChildEnemyHealthBar()
        {
            var healthBarTransform = gameObject.transform.Find("EnemyHealthBar");
            var gObject = healthBarTransform.gameObject;
            var ret = gObject.GetComponent<EnemyHealthBar>();
            return ret;
        }
    }
}