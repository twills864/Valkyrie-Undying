﻿using System.Collections.Generic;
using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Pickups;
using Assets.Powerups;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The explosion activated after the player takes damage.
    /// The retribution bullet scales in to eventually cover the entire screen,
    /// drastically slowing down the time scale of every object it collides with
    /// before resuming the normal game speed over time.
    /// </summary>
    /// <inheritdoc/>
    public class RetributionBullet : PermanentVelocityPlayerBullet
    {
        protected override bool AutomaticallyDeactivate => false;
        protected override bool ShouldDeactivateOnDestructor => false;
        protected override bool ShouldMarkSelfCollision => false;

        public override AudioClip FireSound => SoundBank.Silence;

        #region Property Fields

        //private bool _ShouldEraseBullet;

        #endregion Property Fields

        #region Prefabs

        [SerializeField]
        private float _ScaleInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _TimeScaleAlphaScale = GameConstants.PrefabNumber;
        private bool _shouldEraseBullet;

        #endregion Prefabs


        #region Prefab Properties

        private float FadeTime => _FadeTime;
        private float ScaleInTime => _ScaleInTime;
        public float TimeScaleAlphaScale => _TimeScaleAlphaScale;

        #endregion Prefab Properties


        public float RetributionTimeScaleValue => 1.0f - (Alpha * TimeScaleAlphaScale);

        private float Scale { get; set; }

        private bool IsExploding { get; set; }

        private MoveTo MoveToCenter { get; set; }
        private Sequence Sequence { get; set; }

        private bool ShouldEraseBullet => false;
        //{
        //    get { _ShouldEraseBullet = !_ShouldEraseBullet; return _ShouldEraseBullet; }
        //    set => _ShouldEraseBullet = !value;
        //}

        // Currently unused
        //private List<ValkyrieSprite> ManagedMiscSprites { get; set; }
        private TrackedPooledObjectSet<Enemy> ManagedEnemies { get; set; }
        private TrackedPooledObjectSet<EnemyBullet> ManagedEnemyBullets { get; set; }
        private TrackedPooledObjectSet<PlayerBullet> ManagedPlayerBullets { get; set; }
        private TrackedPooledObjectSet<Pickup> ManagedPickups { get; set; }

        public sealed override Vector3 GetHitPosition(Enemy enemy) => enemy.transform.position;

        public static RetributionBullet StartRetribution(Vector3 position)
        {
            var bullet = PoolManager.Instance.BulletPool.Get<RetributionBullet>(position);
            bullet.OnSpawn();

            return bullet;
        }

        protected override void OnPermanentVelocityBulletInit()
        {
            Scale = SpaceUtil.WorldMap.Height;

            var scaleIn = new ScaleTo(this, float.Epsilon, Scale, ScaleInTime);
            var scaleEase = new EaseIn3(scaleIn);

            MoveToCenter = new MoveTo(this, Vector3.zero, new Vector3(1f, 1f), ScaleInTime);

            var scaleAndMove = new ConcurrentGameTask(scaleEase, MoveToCenter);

            var calmExplosion = new GameTaskFunc(this, () => IsExploding = false);

            var fade = new FadeTo(this, 0, FadeTime);
            var fadeEase = new EaseOut(fade);

            Sequence = new Sequence(scaleAndMove, calmExplosion, fadeEase);

            //ManagedMiscSprites = new List<ValkyrieSprite>();
            ManagedEnemies = new TrackedPooledObjectSet<Enemy>();
            ManagedEnemyBullets = new TrackedPooledObjectSet<EnemyBullet>();
            ManagedPlayerBullets = new TrackedPooledObjectSet<PlayerBullet>();
            ManagedPickups = new TrackedPooledObjectSet<Pickup>();

        }

        protected override void OnActivate()
        {
            Color color = Sprite.color;
            color.a = 1.0f;
            Sprite.color = color;

            IsExploding = true;

            //ShouldEraseBullet = true;

            //ManagedMiscSprites.Clear();
            ManagedEnemies.Clear();
            ManagedEnemyBullets.Clear();
            ManagedPlayerBullets.Clear();
            ManagedPickups.Clear();

            Sequence.ResetSelf();
        }

        protected override void OnBulletSpawn()
        {
            MoveToCenter.ReinitializeMove(transform.position, SpaceUtil.WorldMap.Center);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!Sequence.IsFinished)
            {
                Sequence.RunFrame(deltaTime);

                SetRetributionScales(RetributionTimeScaleValue);
            }
            else
            {
                DeactivateSelf();
            }
        }

        #region Collision

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsEnemyBullet(collision))
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();
                if (IsExploding && ShouldEraseBullet)
                    enemyBullet.DeactivateSelf();
                else
                    ManagedEnemyBullets.Add(enemyBullet);
            }
            else if (CollisionUtil.IsPlayerBullet(collision))
            {
                var playerBullet = collision.GetComponent<PlayerBullet>();

                ManagedPlayerBullets.Add(playerBullet);
            }
            else if (CollisionUtil.IsPickup(collision))
            {
                var pickup = collision.GetComponent<Pickup>();

                ManagedPickups.Add(pickup);
            }
            // Enemy is handled in OnCollideWithEnemy()
        }

        public override bool CollidesWithEnemy(Enemy enemy)
        {
            bool alreadyManaged = ManagedEnemies.Contains(enemy);
            return !alreadyManaged;
        }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            ManagedEnemies.Add(enemy);
            enemy.RetributionBulletCollisionEnter(this);
        }

        #endregion Collision

        private IEnumerable<IEnumerable<ValkyrieSprite>> AllManagedEnumerables
        {
            get
            {
                //yield return ManagedMiscSprites;
                yield return ManagedEnemies;
                yield return ManagedEnemyBullets;
                yield return ManagedPlayerBullets;
                yield return ManagedPickups;
            }
        }

        private void SetRetributionScales(float scale)
        {
            foreach (var enumerable in AllManagedEnumerables)
            {
                foreach (var sprite in enumerable)
                    sprite.SetRetributionTimeScale(this);
            }

            SetInstanceRetributionScales(scale);
        }

        private void SetInstanceRetributionScales(float scale)
        {
            Player.Instance.SetRetributionTimeScale(this);
            Monsoon.Instance.SetRetributionTimeScale(this);
            SentinelManager.Instance.SetRetributionTimeScale(this);
            Director.SetRetributionTimeScale(this);
        }

        protected override void OnBulletDeactivate()
        {
            transform.localScale = Vector3.zero;
            SetRetributionScales(1.0f);
        }
    }
}