using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// A bullet that will hit an enemy, bounce in a random direction, and
    /// change its velocity over time to again be traveling towards the enemy.
    ///
    /// The effect resembles electrons orbiting the nucleus of an atom, and will
    /// hit an enemy multiple times before deactivating. The number of times
    /// a bullet can hit increases with the bullet level.
    /// </summary>
    /// <inheritdoc/>
    public class AtomBullet : BouncingBullet
    {
        public override AudioClip FireSound => SoundBank.LaserPew;

        #region Prefabs

        [SerializeField]
        private float _VelocityChangerDuration = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float VelocityChangerDuration => _VelocityChangerDuration;

        #endregion Prefab Properties


        private VelocityChange VelocityChange { get; set; }
        private Vector2 MostRecentTargetVelocity { get; set; }

        public AtomTrail Trail { get; set; }

        protected override void OnBouncingBulletInit()
        {
            VelocityChange = VelocityChange.Default(this, VelocityChangerDuration);
        }

        protected override void OnBouncingBulletActivate()
        {
            VelocityChange.FinishSelf();
            MostRecentTargetVelocity = Vector2.zero;
        }

        protected override void OnBouncingBulletSpawn()
        {
            Trail = PoolManager.Instance.UIElementPool.Get<AtomTrail>();
            Trail.transform.position = transform.position;
            Trail.OnSpawn();
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            VelocityChange.RunFrame(deltaTime);
            Trail.transform.position = transform.position;

            // Basic (and deliberately-flawed) homing system - apply velocity of most recent target
            ApplyVelocity(MostRecentTargetVelocity, deltaTime);
        }

        protected override void OnBounce(Enemy enemy)
        {
            MostRecentTargetVelocity = enemy.Velocity;

            var direction = RandomUtil.RandomDirectionVector(Speed);
            VelocityChange.Init(direction, -direction);
        }

        protected override void OnBouncingBulletDeactivate()
        {
            Trail.StartDeactivation();
            Trail = null;
        }
    }
}