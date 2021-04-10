using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using System.Collections.Generic;
using Assets.Enemies;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public abstract class SmiteBullet : PlayerBullet
    {
        public sealed override int Damage => SmiteDamage;

        #region Prefabs

        #endregion Prefabs


        #region Prefab Properties

        #endregion Prefab Properties

        //public SmiteBullet Next { get; protected set; }

        // Backwards-facing linked list
        public SmiteBullet Head { get; protected set; }
        public SmiteBullet Previous { get; protected set; }

        public List<PooledObjectTracker> HitEnemies { get; } = new List<PooledObjectTracker>();

        public int SmiteDamage { get; set; }
        public abstract float Scale { get; set; }

        public Vector3 TargetPosition { get; set; }

        public Sequence FadeOutSequence { get; set; }
        private bool IsDeactivating { get; set; }
        public void BeginDeactivating()
        {
            IsDeactivating = true;
        }
        //public PooledObjectTracker TargetEnemy { get; set; }

        //protected sealed override void OnPlayerBulletInit()
        //{

        //}

        //protected sealed override void OnActivate()
        //{
        //    Alpha = 1.0f;
        //}

        public sealed override void OnSpawn()
        {
            Alpha = 1.0f;
            IsDeactivating = false;
            FadeOutSequence.ResetSelf();
        }

        protected virtual void OnSmiteBulletFrameRun(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!IsDeactivating)
                OnSmiteBulletFrameRun(deltaTime, realDeltaTime);
            else
                FadeOutSequence.RunFrame(deltaTime);

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
            TargetPosition = existingLink.TargetPosition;
        }

        [Obsolete("Test method")]
        public static void DebugTestSmite()
        {
            Vector3 startPosition = Player.Instance.FirePosition();
            Vector3 targetPosition = SpaceUtil.WorldPositionUnderMouse();

            SmiteJointBullet.StartSmite(startPosition, targetPosition, 10);
        }

        public sealed override bool CollidesWithEnemy(Enemy enemy)
        {
            bool isNewEnemy = !Head.HitEnemies.Where(x => x.IsTarget(enemy)).Any();
            return isNewEnemy;
        }

        public sealed override void OnCollideWithEnemy(Enemy enemy)
        {
            Head.HitEnemies.Add(enemy);
        }

        public void SetFadeOutSequence(float fadeTime)
        {
            var fade = new FadeTo(this, 0.0f, fadeTime);
            var deactivate = new GameTaskFunc(this, DeactivateSelf);

            FadeOutSequence = new Sequence(fade, deactivate);
        }
    }
}