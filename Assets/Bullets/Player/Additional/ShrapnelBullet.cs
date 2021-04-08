using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class ShrapnelBullet : PlayerBullet
    {
        protected override bool ShouldMarkSelfCollision => false;

        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;

        #endregion Prefab Properties


        public bool IsBurning => FireDamage != 0;
        public int FireDamage { get; set; }

        public PooledObjectTracker Parent { get; private set; }

        protected override void OnPlayerBulletInit()
        {
            Parent = new PooledObjectTracker();
        }

        protected override void OnActivate()
        {
            FireDamage = 0;
        }

        public override bool CollidesWithEnemy(Enemy enemy)
        {
            bool ret = !Parent.IsTarget(enemy);
            return ret;
        }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            // Burn enemy if applicable before deactivating.
            if(IsBurning && !enemy.IsBurning)
                enemy.Ignite(FireDamage, FireDamage);

            base.OnCollideWithEnemy(enemy);
        }
    }
}