using Assets.Constants;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public class CradleEnemyBullet : EnemyBullet
    {
        public override int ReflectedDamage => 10;

        [SerializeField]
        public float Speed = GameConstants.PrefabNumber;
    }
}