using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class AtomBullet : BouncingBullet
    {
        [SerializeField]
        public float VelocityChangerDuration;

        //private LinearVelocityChanger VelocityChanger { get; set; }
        private VelocityChange VelocityChange { get; set; }
        private Vector2 MostRecentTargetVelocity { get; set; }


        public AtomTrail Trail { get; set; }

        protected override void OnBouncingBulletInit()
        {
            //VelocityChanger = new LinearVelocityChanger(this);
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

        protected override void OnBouncingBulletDeactivate()
        {
            Trail.StartDeactivation();
            Trail = null;
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
    }
}