﻿using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class VoidBullet : PlayerBullet
    {
        [SerializeField]
        private float ScaleTime;

        private float InitialScale => float.Epsilon;

        private float Scale { get; set; }
        private float Duration { get; set; }

        private EaseIn ScaleIn { get; set; }
        private Delay Delay { get; set; }
        private EaseOut ScaleOut { get; set; }

        private SequenceGameTask Sequence { get; set; }

        public static VoidBullet StartVoid(Vector3 position, int level, float scale, float duration)
        {
            var bullet = PoolManager.Instance.BulletPool.Get<VoidBullet>(position);
            bullet.Init(level, scale, duration);
            bullet.OnSpawn();

            return bullet;
        }

        private void Init(int level, float scale, float duration)
        {
            BulletLevel = level;
            Scale = scale;
            Duration = duration;

            var scaleIn = new ScaleTo(this, InitialScale, Scale, ScaleTime);
            ScaleIn = new EaseIn(scaleIn);
            Delay = new Delay(this, Duration);
            var scaleOut = new ScaleTo(this, Scale, InitialScale, ScaleTime);
            ScaleOut = new EaseOut(scaleOut);

            Sequence = new SequenceGameTask(this, ScaleIn, Delay, ScaleOut);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            if (!Sequence.IsFinished)
                Sequence.RunFrame(deltaTime);
            else
                DeactivateSelf();
        }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            enemy.VoidPause();
        }

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsEnemyBullet(collision))
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();

                if (BulletLevel == 1)
                    enemyBullet.DeactivateSelf();
                else
                    GameManager.Instance.ReflectBullet(enemyBullet);
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if(CollisionUtil.IsEnemy(collision))
            {
                var enemy = collision.GetComponent<Enemy>();
                enemy.VoidResume();
            }
        }
    }
}