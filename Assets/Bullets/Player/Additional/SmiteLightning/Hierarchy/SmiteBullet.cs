using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using System.Collections.Generic;
using Assets.Enemies;
using Assets.Bullets.EnemyBullets;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public abstract class SmiteBullet : PlayerBullet
    {
        public sealed override int Damage => SmiteDamage;

        // Backwards-facing linked list with head node
        public SmiteBullet Head { get; protected set; }
        public SmiteBullet Previous { get; protected set; }

        public List<PooledObjectTracker> HitEnemies { get; } = new List<PooledObjectTracker>();

        public int SmiteDamage { get; set; }
        public abstract float Scale { get; set; }

        public PooledObjectTracker TargetEnemy { get; } = new PooledObjectTracker();
        public Vector3 TargetPosition { get; set; }

        public Sequence FadeOutSequence { get; set; }
        private bool IsDeactivating { get; set; }

        protected virtual void OnSmiteBulletInit() { }
        protected sealed override void OnPlayerBulletInit()
        {
            var fade = new FadeTo(this, 0f, 1.0f);
            var deactivate = new GameTaskFunc(this, DeactivateSelf);
            FadeOutSequence = new Sequence(fade, deactivate);

            OnSmiteBulletInit();
        }

        public sealed override void OnSpawn()
        {
            Alpha = 1.0f;
            IsDeactivating = false;
            FadeOutSequence.ResetSelf();
        }

        // Smite bullets use the realDeltaTime for updates.
        protected virtual void OnSmiteBulletFrameRun(float realDeltaTime) { }
        protected sealed override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!IsDeactivating)
            {
                if (TargetEnemy.IsActive)
                    TargetPosition = TargetEnemy.Target.transform.position;

                OnSmiteBulletFrameRun(realDeltaTime);
            }
            else
                FadeOutSequence.RunFrame(realDeltaTime);

        }

        protected sealed override void OnDeactivate()
        {
            HitEnemies.Clear();
        }

        public void InitFromPreviousLink(SmiteBullet existingLink)
        {
            Previous = existingLink;
            Head = existingLink.Head;
            SmiteDamage = existingLink.SmiteDamage;
            TargetEnemy.CloneFrom(existingLink.TargetEnemy);
            TargetPosition = existingLink.TargetPosition;

            if (!TargetEnemy.IsActive)
                TargetPosition = VectorUtil.WithY(TargetPosition, SpaceUtil.WorldMap.Top.y);
        }

        public sealed override bool CollidesWithEnemy(Enemy enemy)
        {
            bool isNewEnemy = !Head.HitEnemies.Where(x => x.IsTarget(enemy)).Any();
            return isNewEnemy;
        }

        public sealed override void OnCollideWithEnemy(Enemy enemy)
        {
            Head.HitEnemies.Add(enemy);

            if (TargetEnemy.IsTarget(enemy) && enemy.DiesOnSmite)
            {
                enemy.KillEnemy(this);
                DeactivateAllLinks();
            }
        }

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsEnemyBullet(collision))
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();
                GameManager.Instance.ReflectBullet(enemyBullet);
            }
        }

        public void BeginDeactivating()
        {
            IsDeactivating = true;
        }

        public void SetFadeOutSequenceFadeTime(float fadeTime)
        {
            FadeOutSequence.Duration = fadeTime;
        }

        public void DeactivateAllLinks()
        {
            SmiteBullet link = this;

            do
            {
                link.BeginDeactivating();
                link = link.Previous;
            } while (link != null);
        }

        //[Obsolete("Test method")]
        //public static void DebugTestSmite()
        //{
        //    Vector3 startPosition = Player.Instance.FirePosition();
        //    Vector3 targetPosition = SpaceUtil.WorldPositionUnderMouse();

        //    var enemy = GameManager.Instance._DebugEnemy;

        //    //SmiteJointBullet.StartSmite(startPosition, targetPosition, 10);
        //    SmiteJointBullet.StartSmite(startPosition, enemy);
        //}
    }
}