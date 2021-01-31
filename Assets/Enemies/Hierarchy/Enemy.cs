using Assets;
using Assets.Bullets;
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

        public int CurrentHealth { get; set; }

        // The health this enemy spawns with at the start of the game
        public abstract int BaseSpawnHealth { get; }

        // The rate at which the spawn health of this enemy increases as the game progresses
        public abstract float SpawnHealthScaleRate { get; }

        [SerializeField]
        public EnemyHealthBar HealthBar;

        public void UpdateHealthBar() => HealthBar.SetText(CurrentHealth);


        public override void Init()
        {
            base.Init();
            CurrentHealth = BaseSpawnHealth;

            HealthBar = FindChildEnemyHealthBar();
            UpdateHealthBar();
        }

        public override void RunFrame(float deltaTime)
        {
            throw new System.NotImplementedException();
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
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayerBullet(collision))
            {
                Bullet bullet = collision.GetComponent<Bullet>();
                CollideWithBullet(bullet);
            }
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