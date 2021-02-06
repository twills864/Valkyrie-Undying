using Assets.Bullets.EnemyBullets;
using Assets.EnemyBullets;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    /// <inheritdoc/>
    public class EnemyBulletPoolList : PoolList<EnemyBullet>
    {
        [SerializeField]
        private BasicEnemyBullet BasicPrefab;
        [SerializeField]
        private TankEnemyBullet TankPrefab;
    }
}
