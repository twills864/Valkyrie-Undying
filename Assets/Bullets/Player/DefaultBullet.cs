using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class DefaultBullet : PlayerBullet
    {
        public override int Damage => CalculateDamage();
        public override AudioClip FireSound => SoundBank.LaserBasic;

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
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {

        }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            GameManager.Instance.OnEnemyHitWithDefaultWeapon(enemy, this, hitPosition);
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