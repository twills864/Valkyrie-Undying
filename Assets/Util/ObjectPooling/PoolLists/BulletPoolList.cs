using Assets.Bullets;
using Assets.Bullets.PlayerBullets;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    /// <inheritdoc/>
    public class BulletPoolList : PoolList<Bullet>
    {
        [SerializeField]
        private BasicBullet BasicPrefab;
        [SerializeField]
        private ShotgunBullet ShotgunPrefab;
    }
}
