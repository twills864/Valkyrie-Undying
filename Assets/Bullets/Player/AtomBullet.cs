using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
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

        private LinearVelocityChanger VelocityChanger { get; set; }
        private Vector2 MostRecentTargetVelocity { get; set; }

        public AtomTrail Trail { get; set; }

        protected override void OnBouncingBulletInit()
        {
            VelocityChanger = new LinearVelocityChanger(this);
        }

        protected override void OnBouncingBulletActivate()
        {
            VelocityChanger.Init(InitialVelocity);
            MostRecentTargetVelocity = Vector2.zero;

            Trail = GameManager.Instance.GetAtomTrail();
        }
        protected override void OnBouncingBulletDeactivate()
        {
            Trail.StartDeactivation();
            Trail = null;
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            VelocityChanger.RunFrame(deltaTime);
            Trail.transform.position = transform.position;

            // Basic (and deliberately-flawed) homing system - apply velocity of most recent target
            ApplyVelocity(MostRecentTargetVelocity, deltaTime);
        }

        protected override void OnBounce(Enemy enemy)
        {
            MostRecentTargetVelocity = enemy.Velocity;

            var direction = RandomUtil.RandomDirectionVector(Speed);
            VelocityChanger.Init(direction, -direction, VelocityChangerDuration);
        }
    }
}