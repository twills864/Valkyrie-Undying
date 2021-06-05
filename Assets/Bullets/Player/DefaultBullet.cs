using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.Powerups;
using Assets.ScreenEdgeColliders;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class DefaultBullet : PlayerBullet
    {
        public override int Damage => CalculateDamage();
        public override AudioClip FireSound => SoundBank.LaserBasic;

        #region Penetration

        public static int MaxPenetration { get; set; }
        private int NumberPenetrated { get; set; }

        protected override bool AutomaticallyDeactivate => NumberPenetrated >= MaxPenetration;

        #endregion Penetration

        public static bool ReboundActive { get; set; } = false;

        public static void StaticInit()
        {
            MaxPenetration = 0;
            ReboundActive = false;
        }

        #region Prefabs

        [SerializeField]
        private float _InitialSpeed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float InitialSpeed => _InitialSpeed;

        #endregion Prefab Properties

        protected override void OnPlayerBulletInit()
        {

        }

        protected override void OnActivate()
        {

        }

        public override void OnSpawn()
        {
            Velocity = CalculateVelocity();
            NumberPenetrated = 0;
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {

        }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            GameManager.Instance.OnEnemyHitWithDefaultWeapon(enemy, this, hitPosition);

            NumberPenetrated++;
        }

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if(ReboundActive && CollisionUtil.IsScreenEdge(collision, out ScreenSide screenSide)
                && screenSide == ScreenSide.Top)
            {
                ReboundPowerup.ReboundOffScreenEdge(this);
            }
        }

        private int CalculateDamage()
        {
            int ret = BaseDamage;

            // Future modifications to damage below

            return ret;
        }

        private Vector2 CalculateVelocity()
        {
            Vector2 ret = new Vector2(0, InitialSpeed);

            // Future modifications to Velocity below

            return ret;
        }
    }
}