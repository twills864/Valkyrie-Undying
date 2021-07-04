using Assets.Constants;
using Assets.Enemies;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// A bullet that will fire in a very large cone-shaped spread,
    /// and will add chill time to any enemies hit.
    /// </summary>
    /// <inheritdoc/>
    public class SpreadBullet : HighVelocityPlayerBullet
    {
        public override int Damage => CurrentDamage;
        public int CurrentDamage { get; private set; }
        public override AudioClip FireSound => SoundBank.ShotgunHard;

        #region Prefabs

        [SerializeField]
        private int _AdditionalBulletDamage = GameConstants.PrefabNumber;

        [SerializeField]
        private float _BulletVelocityY = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FireRadius = GameConstants.PrefabNumber;

        [SerializeField]
        private float _AngleBetweenLanesInDegrees = GameConstants.PrefabNumber;

        [SerializeField]
        private float _DampX = GameConstants.PrefabNumber;

        [SerializeField]
        private int _ChillTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public int AdditionalBulletDamage => _AdditionalBulletDamage;
        public float BulletVelocityY => _BulletVelocityY;
        public float FireRadius => _FireRadius;
        public float AngleBetweenLanesInDegrees => _AngleBetweenLanesInDegrees;
        public float DampX => _DampX;
        private int ChillTime => _ChillTime;

        #endregion Prefab Properties

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            enemy.AddChill(ChillTime);
        }


        public void SetDamage(bool isMainBullet)
        {
            CurrentDamage = isMainBullet ? BaseDamage : AdditionalBulletDamage;
        }
    }
}