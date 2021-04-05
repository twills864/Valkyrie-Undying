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
        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;
        public float Speed => _Speed;

        protected override bool ShouldMarkSelfCollision => false;

        public override bool CollidesWithEnemy(Enemy enemy)
        {
            bool ret = !Parent.IsTarget(enemy);
            return ret;
        }

        public PooledObjectTracker Parent { get; private set; }

        protected override void OnPlayerBulletInit()
        {
            Parent = new PooledObjectTracker();
        }
    }
}