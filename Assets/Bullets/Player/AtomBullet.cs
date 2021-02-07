using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class AtomBullet : PlayerBullet
    {
        [SerializeField]
        private int DamageAfterBounce;
        [SerializeField]
        public float Speed;

        [SerializeField]
        public float VelocityChangerDuration;

        public int BouncesLeft { get; set; }
        private int CurrentDamage { get; set; }
        public override int Damage => CurrentDamage;

        private Vector2 InitialVelocity { get; set; }
        private LinearVelocityChanger VelocityChanger { get; set; }

        private Vector2 MostRecentTargetVelocity { get; set; }

        private TrailRenderer Trail { get; set; }

        protected override void OnActivate()
        {
            CurrentDamage = BaseDamage;
            Velocity = InitialVelocity;
            VelocityChanger.Init(InitialVelocity);
            MostRecentTargetVelocity = Vector2.zero;
        }

        protected override void OnDeactivate()
        {
            Trail.Clear();
        }

        protected override void OnPlayerBulletInit()
        {
            InitialVelocity = new Vector2(0, Speed);
            VelocityChanger = new LinearVelocityChanger(this);
            Trail = GetComponent<TrailRenderer>();
        }


        protected override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            VelocityChanger.RunFrame(deltaTime);
            ApplyVelocity(MostRecentTargetVelocity, deltaTime);
        }



        public override void OnCollideWithEnemy(Enemy enemy)
        {
            if (BouncesLeft > 0)
            {
                Bounce(enemy);
                MostRecentTargetVelocity = enemy.Velocity;
            }
            else
                DeactivateSelf();
        }

        #region Enemy Bounce

        private void Bounce(Enemy enemy)
        {
            BouncesLeft--;
            CurrentDamage = DamageAfterBounce;

            var direction = RandomUtil.RandomDirectionVector(Speed);
            VelocityChanger.Init(direction, -direction, VelocityChangerDuration);
        }

        #endregion Enemy Bounce


        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            // Todo - More elegant way to disable self-collision notification
        }
    }
}