using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet spawned after an enemy's death as part of the Void powerup.
    /// This bullet will Void Pause enemies it collides with, and will spawn
    /// smaller bullets from its center while it's active.
    /// </summary>
    /// <inheritdoc/>
    public class VoidBullet : VoidEffectBullet
    {
        public static AudioClip VoidBulletFireSound => SoundBank.ExplosionShortSweep;
        protected override bool ShouldReflectBullet => BulletLevel > 1;
        public override AudioClip FireSound => VoidBulletFireSound;
        public override float FireSoundVolume => 0.3f;

        #region Prefabs

        [SerializeField]
        private float _ScaleTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _TimeBetweenProjectiles = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float ScaleTime => _ScaleTime;
        public float TimeBetweenProjectiles => _TimeBetweenProjectiles;

        #endregion Prefab Properties

        private float Scale { get; set; }
        private float Duration { get; set; }

        private bool CanShootProjectiles { get; set; }
        private LoopingFrameTimer ProjectileTimer { get; set; }

        private EaseIn3 ScaleIn { get; set; }
        private GameTaskFunc EnableProjectiles { get; set; }
        private Delay Delay { get; set; }
        private GameTaskFunc DisableProjectiles { get; set; }
        private EaseOut3 ScaleOut { get; set; }

        private Sequence Behavior { get; set; }

        public sealed override bool CollidesWithEnemy(Enemy enemy) => base.ActivateOnCollideWithoutColliding(enemy);

        public static VoidBullet StartVoid(Vector3 position, int level, float scale, float duration, bool playAudio = true)
        {
            var bullet = PoolManager.Instance.BulletPool.Get<VoidBullet>(position);
            bullet.Init(level, scale, duration);
            bullet.OnSpawn();

            if(playAudio)
                bullet.PlayFireSound();

            return bullet;
        }

        private void Init(int level, float scale, float duration)
        {
            BulletLevel = level;
            Scale = scale;
            Duration = duration;

            var scaleIn = new ScaleTo(this, InitialScale, Scale, ScaleTime);
            ScaleIn = new EaseIn3(scaleIn);
            Delay = new Delay(this, Duration);
            var scaleOut = new ScaleTo(this, Scale, InitialScale, ScaleTime);
            ScaleOut = new EaseOut3(scaleOut);

            Behavior = new Sequence(ScaleIn, EnableProjectiles, Delay, DisableProjectiles, ScaleOut);
        }

        protected override void OnPlayerBulletInit()
        {
            EnableProjectiles = new GameTaskFunc(this, () => CanShootProjectiles = true);
            DisableProjectiles = new GameTaskFunc(this, () => CanShootProjectiles = false);
            ProjectileTimer = new LoopingFrameTimer(TimeBetweenProjectiles);
        }

        protected override void OnActivate()
        {
            CanShootProjectiles = false;

            // Fire projectile as soon as Void is done expanding
            ProjectileTimer.ActivateSelf();
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!Behavior.IsFinished)
            {
                Behavior.RunFrame(deltaTime);

                if (CanShootProjectiles && ProjectileTimer.UpdateActivates(deltaTime))
                {
                    var projectile = PoolManager.Instance.BulletPool.Get<VoidProjectileBullet>(transform.position);
                    projectile.OnSpawn();
                }
            }
            else
                DeactivateSelf();
        }
    }
}