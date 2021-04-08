using Assets.Constants;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class SpreadBullet : PlayerBullet
    {
        public override int Damage => CurrentDamage;
        public int CurrentDamage { get; private set; }

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

        #endregion Prefabs

        #region Prefab Properties

        public int AdditionalBulletDamage => _AdditionalBulletDamage;
        public float BulletVelocityY => _BulletVelocityY;
        public float FireRadius => _FireRadius;
        public float AngleBetweenLanesInDegrees => _AngleBetweenLanesInDegrees;
        public float DampX => _DampX;

        #endregion Prefab Properties

        public void SetDamage(bool isMainBullet)
        {
            CurrentDamage = isMainBullet ? BaseDamage : AdditionalBulletDamage;
        }
    }
}