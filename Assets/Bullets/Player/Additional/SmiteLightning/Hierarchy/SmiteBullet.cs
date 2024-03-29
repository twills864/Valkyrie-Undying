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
    /// The Smite powerup's lightning bolt effect is made of a series of smaller connected bullets.
    /// This class represents one of these smaller bullets in the bolt.
    /// Contains functionality to store each bullet as an element in a linked list representing the bolt.
    /// </summary>
    /// <inheritdoc/>
    public abstract class SmiteBullet : PlayerBullet
    {
        public sealed override int Damage => DamageOnHitAny;
        protected override bool AutomaticallyDeactivate => false;
        protected sealed override bool ShouldMarkSelfCollision => false;
        public override AudioClip FireSound => SoundBank.ExplosionMediumZap;
        public override AudioClip HitSound => SoundBank.ZapHard;
        public AudioClip HitSound2 => SoundBank.ExplosionMediumDeep;
        public void PlayAllHitSounds()
        {
            PlaySoundAtCenter(HitSound);
            PlaySoundAtCenter(HitSound2);
        }

        private Vector2 _representedVelocity;
        public override Vector2 RepresentedVelocity => _representedVelocity;
        public void SetRepresentedVelocity(Vector2 distanceDelta)
        {
            const float Vel = 40f;
            _representedVelocity = MathUtil.VelocityVector(distanceDelta, Vel);
        }

        // Backwards-facing linked list with head node
        public SmiteBullet Head { get; protected set; }
        public SmiteBullet Previous { get; protected set; }

        public List<PooledObjectTracker<Enemy>> HitEnemies { get; } = new List<PooledObjectTracker<Enemy>>();

        public int DamageOnHitAny { get; set; }
        public int DamageOnHitTarget { get; set; }
        public abstract float Scale { get; set; }

        public PooledObjectTracker<Enemy> TargetEnemy { get; } = new PooledObjectTracker<Enemy>();
        public Vector3 TargetPosition { get; set; }

        public Sequence FadeOutSequence { get; set; }
        private bool IsDeactivating { get; set; }
        private bool DamageActive { get; set; }

        protected virtual void OnSmiteBulletInit() { }
        protected sealed override void OnPlayerBulletInit()
        {
            const float BlinkAlpha = 0.5f;
            const float BlinkDelay = 0.05f;
            var blinkOut = new GameTaskFunc(this, () => Alpha = BlinkAlpha);
            var blinkDelay = new Delay(this, BlinkDelay);
            var blinkIn = new GameTaskFunc(this, () => Alpha = 1.0f);
            var turnOffDamage = new GameTaskFunc(this, () => DamageActive = false);

            var fade = new FadeTo(this, 0f, 1.0f);
            var deactivate = new GameTaskFunc(this, DeactivateSelf);
            FadeOutSequence = new Sequence(blinkOut, blinkDelay, blinkIn, blinkDelay,
                blinkOut, blinkDelay, blinkIn, turnOffDamage, fade, deactivate);

            OnSmiteBulletInit();
        }

        protected override void OnBulletSpawn()
        {
            Alpha = 1.0f;
            DamageActive = true;
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

        protected sealed override void OnBulletDeactivate()
        {
            HitEnemies.Clear();
        }

        public void InitFromPreviousLink(SmiteBullet existingLink)
        {
            Previous = existingLink;
            Head = existingLink.Head;
            DamageOnHitAny = existingLink.DamageOnHitAny;
            DamageOnHitTarget = existingLink.DamageOnHitTarget;
            TargetEnemy.CloneFrom(existingLink.TargetEnemy);
            TargetPosition = existingLink.TargetPosition;

            if (!TargetEnemy.IsActive)
                TargetPosition = TargetPosition.WithY(SpaceUtil.WorldMap.Top.y);
        }

        public sealed override bool CollidesWithEnemy(Enemy enemy)
        {
            if (!DamageActive)
                return false;
            else
            {
                bool isNewEnemy = !Head.HitEnemies.Where(x => x.IsTarget(enemy)).Any();
                return isNewEnemy;
            }
        }

        protected sealed override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            Head.HitEnemies.Add(enemy);

            if (TargetEnemy.IsTarget(enemy) && enemy.FullDamageOnSmite)
            {
                enemy.TrueDamage(DamageOnHitTarget, this);
                DeactivateAllLinks();
            }
        }

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if(DamageActive && CollisionUtil.IsEnemyBullet(collision))
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

        [Obsolete("Test method")]
        public static void DebugTestSmite()
        {
            Vector3 startPosition = Player.Instance.FirePosition;
            Vector3 targetPosition = SpaceUtil.WorldPositionUnderMouse();

            var enemy = GameManager.Instance._DebugEnemy;

            //SmiteJointBullet.StartSmite(startPosition, targetPosition, 10);
            SmiteJointBullet.StartSmite(startPosition, enemy);
        }
    }
}