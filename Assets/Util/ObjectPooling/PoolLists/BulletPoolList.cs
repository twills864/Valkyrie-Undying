using Assets.Bullets.PlayerBullets;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    /// <inheritdoc/>
    public class BulletPoolList : PoolList<PlayerBullet>
    {
        [SerializeField]
        private BasicBullet BasicPrefab;
        [SerializeField]
        private ShotgunBullet ShotgunPrefab;
        [SerializeField]
        private BurstBullet BurstPrefab;
        [SerializeField]
        private BounceBullet BouncePrefab;

        /// <summary>
        /// Accesses the object pool indexed by<typeparamref name="TGet"/>,
        /// returns a fresh instance from that pool,
        /// sets its initial position to a specified <paramref name="position"/>,
        /// and sets its BulletLevel to a specified weapon level.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="position">The position to give to the fresh instance.</param>
        /// <param name="weaponLevel">The bullet level to give to the fresh instance.</param>
        /// <returns>The initialized fresh instance of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet Get<TGet>(Vector2 position, int weaponLevel) where TGet : PlayerBullet
        {
            var ret = Get<TGet>(position);
            ret.BulletLevel = weaponLevel;
            return ret;
        }

        /// <summary>
        /// Accesses the object pool indexed by<typeparamref name="TGet"/>,
        /// returns a fresh instance from that pool,
        /// sets its initial position to a specified <paramref name="position"/>,
        /// sets its initial velocity to a specified <paramref name="velocity"/>,
        /// and sets its BulletLevel to a specified weapon level.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="position">The position to give to the fresh instance.</param>
        /// <param name="velocity">The velocity to give to the fresh instance.</param>
        /// <param name="weaponLevel">The bullet level to give to the fresh instance.</param>
        /// <returns>The initialized fresh instance of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet Get<TGet>(Vector2 position, Vector2 velocity, int weaponLevel) where TGet : PlayerBullet
        {
            var ret = Get<TGet>(position, velocity);
            ret.BulletLevel = weaponLevel;
            return ret;
        }

        /// <summary>
        /// Accesses the Object Pool indexed by <typeparamref name="T"/>
        /// and returns a given number of fresh instances from that Pool.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="amountToGet">The number of fresh instances to get.</param>
        /// <param name="weaponLevel">The bullet level to give to the fresh instance.</param>
        /// <returns>The array of fresh instances of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet[] Get<TGet>(int amountToGet, int weaponLevel) where TGet : PlayerBullet
        {
            TGet[] ret = LinqUtil.Array(amountToGet, () => Get<TGet>());
            return ret;
        }
    }
}
