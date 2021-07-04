using System.Collections.Generic;
using System.Linq;
using Assets.Bullets;
using Assets.Bullets.PlayerBullets;
using Assets.ColorManagers;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.ObjectPooling
{
    /// <summary>
    /// A PoolList that contains PlayerBullet pools.
    /// </summary>
    /// <inheritdoc/>
    public class BulletPoolList : PoolList<PlayerBullet>
    {
#pragma warning disable 0414

        #region Fired Bullets

        [SerializeField]
        private DefaultBullet DefaultPrefab = null;
        [SerializeField]
        private ShotgunBullet ShotgunPrefab = null;
        [SerializeField]
        private BurstBullet BurstPrefab = null;
        [SerializeField]
        private BounceBullet BouncePrefab = null;
        [SerializeField]
        private AtomBullet AtomPrefab = null;
        [SerializeField]
        private SpreadBullet SpreadPrefab = null;
        [SerializeField]
        private FlakBullet FlakPrefab = null;
        [SerializeField]
        private TrampolineBullet TrampolinePrefab = null;
        [SerializeField]
        private WormholeBullet WormholePrefab = null;
        [SerializeField]
        private GatlingBullet GatlingPrefab = null;
        [SerializeField]
        private BfgBullet BfgPrefab = null;
        [SerializeField]
        private OneManArmyBullet OneManArmyPrefab = null;
        [SerializeField]
        private DeadlyDiamondBullet DeadlyDiamondPrefab = null;
        [SerializeField]
        private ReflectBullet ReflectPrefab = null;

        #endregion Fired Bullets

        #region Additional Bullets

        [SerializeField]
        private ReflectedBullet ReflectedPrefab = null;
        [SerializeField]
        private ShrapnelBullet ShrapnelPrefab = null;
        [SerializeField]
        private RaindropBullet RaindropPrefab = null;
        [SerializeField]
        private PestControlBullet PestControlPrefab = null;
        [SerializeField]
        private SentinelBullet SentinelPrefab = null;
        [SerializeField]
        private OthelloBullet OthelloPrefab = null;
        [SerializeField]
        private VictimBullet VictimPrefab = null;
        [SerializeField]
        private InfernoBullet InfernoPrefab = null;
        [SerializeField]
        private VoidBullet VoidPrefab = null;
        [SerializeField]
        private RetributionBullet RetributionPrefab = null;
        [SerializeField]
        private BfgBulletSpawner BfgSpawnerPrefab = null;
        [SerializeField]
        private BfgBulletFallout BfgFalloutPrefab = null;
        [SerializeField]
        private MortarBullet MortarPrefab = null;
        [SerializeField]
        private SmiteLightningBullet SmiteLightningPrefab = null;
        [SerializeField]
        private SmiteJointBullet SmiteJointPrefab = null;
        [SerializeField]
        private ParapetBullet ParapetPrefab = null;
        [SerializeField]
        private VoidProjectileBullet VoidProjectilePrefab = null;
        [SerializeField]
        private SentinelProjectileBullet SentinelProjectilePrefab = null;
        [SerializeField]
        private DefaultExtraBullet DefaultExtraPrefab = null;
        [SerializeField]
        private ParasiteBullet ParasitePrefab = null;
        [SerializeField]
        private TargetPracticeBullet TargetPracticePrefab = null;

        #endregion Additional Bullets

#pragma warning restore 0414

        protected override Color GetDefaultColor(in ColorManager colorManager)
            => colorManager.DefaultPlayer;

        protected override void OnInitSprites(in ColorManager colorManager)
        {
            var player = colorManager.Player;
            Color defaultAdditional = colorManager.DefaultPlayerAdditionalColorPseudoAlpha();

            ReflectedPrefab.SpriteColor = player.Reflected;
            ShrapnelPrefab.SpriteColor = defaultAdditional;
            RaindropPrefab.SpriteColor = defaultAdditional;
            PestControlPrefab.SpriteColor = defaultAdditional;
            SentinelPrefab.SpriteColor = player.Sentinel;
            SentinelProjectilePrefab.CloneFromSentinel(SentinelPrefab);
            OthelloPrefab.SpriteColor = defaultAdditional;
            VictimPrefab.SpriteColor = defaultAdditional;
            VoidPrefab.SpriteColor = player.Void;
            VoidProjectilePrefab.SpriteColor = player.Void;
            RetributionPrefab.SpriteColor = player.Retribution;
            MortarPrefab.SpriteColor = defaultAdditional;

            BfgBulletSpawner.StaticInit();
            BfgPrefab.InitSpawner();
            BfgPrefab.InitFallout();

            ParasitePrefab.SpriteColor = player.Parasite;
        }

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
        public TGet Get<TGet>(Vector3 position, int weaponLevel) where TGet : PlayerBullet
        {
            var ret = Get<TGet>(position);
            ret.BulletLevel = weaponLevel;
            ret.OnSpawn();
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
        public TGet Get<TGet>(Vector3 position, Vector2 velocity, int weaponLevel) where TGet : PlayerBullet
        {
            var ret = Get<TGet>(position, velocity);
            ret.BulletLevel = weaponLevel;
            ret.OnSpawn();
            return ret;
        }

        /// <summary>
        /// Accesses the Object Pool indexed by <typeparamref name="T"/>
        /// and returns a given number of fresh instances from that Pool.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="amountToGet">The number of fresh instances to get.</param>
        /// <param name="position">The position to give to the fresh instance.</param>
        /// <returns>The array of fresh instances of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet[] GetMany<TGet>(int amountToGet, Vector3 position) where TGet : PlayerBullet
        {
            TGet[] ret = LinqUtil.Array(amountToGet, () => Get<TGet>(position));
            return ret;
        }

        /// <summary>
        /// Accesses the Object Pool indexed by <typeparamref name="T"/>
        /// and returns a given number of fresh instances from that Pool.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="amountToGet">The number of fresh instances to get.</param>
        /// <param name="position">The position to give to the fresh instance.</param>
        /// <param name="weaponLevel">The bullet level to give to the fresh instance.</param>
        /// <returns>The array of fresh instances of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet[] GetMany<TGet>(int amountToGet, Vector3 position, int weaponLevel) where TGet : PlayerBullet
        {
            TGet[] ret = LinqUtil.Array(amountToGet, () => Get<TGet>(position, weaponLevel));
            return ret;
        }

        #region Debug

        public void DebugPrintTrails()
        {
            var activeBullets = Pools.SelectMany(x => x)
                .Where(x => x.isActiveAndEnabled)
                .OrderBy(x => x.name)
                ;


            Dictionary<BulletTrail, int> counts = new Dictionary<BulletTrail, int>();

            foreach (var bullet in activeBullets)
            {
                var trail = bullet.CurrentBulletTrail;

                bullet.Log();

                if (trail != null)
                {
                    Debug.Log(trail, trail);

                    if (!counts.ContainsKey(trail))
                    {
                        counts[trail] = 0;
                    }
                    else
                    {
                        Debug.Break();
                        System.Diagnostics.Debugger.Break();
                    }
                    counts[trail]++;
                }
                else
                {
                    Debug.Log("NULL", bullet);
                }
            }
        }

        #endregion
    }
}
