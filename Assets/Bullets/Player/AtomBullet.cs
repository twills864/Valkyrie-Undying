using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.UI;
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

        public AtomTrail Trail { get; set; }

        protected override void OnActivate()
        {
            CurrentDamage = BaseDamage;
            Velocity = InitialVelocity;
            VelocityChanger.Init(InitialVelocity);
            MostRecentTargetVelocity = Vector2.zero;

            Trail = GameManager.Instance.GetAtomTrail();
        }
        protected override void OnDeactivate()
        {
            Trail.StartDeactivation();
            Trail = null;
        }

        protected override void OnPlayerBulletInit()
        {
            InitialVelocity = new Vector2(0, Speed);
            VelocityChanger = new LinearVelocityChanger(this);
        }


        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            VelocityChanger.RunFrame(deltaTime);
            Trail.transform.position = transform.position;
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