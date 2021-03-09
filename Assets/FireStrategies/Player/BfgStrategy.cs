using Assets.Bullets.PlayerBullets;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    /// Two-step fire strategy:
    /// 1. Spawn a Battle-Frenzied Guillotine spawner that acts as an outline for the laser about to fire.
    /// 2. Fire a Battle-Frenzied Guillotine.
    /// </summary>
    /// <inheritdoc/>
    public class BfgStrategy : PlayerFireStrategy<BfgBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(1f);

        BulletPoolList PoolList { get; set; }

        public BfgStrategy(BfgBullet bullet) : base(bullet)
        {
            PoolList = PoolManager.Instance.BulletPool;
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            PlayerBullet[] ret;

            if (BfgBulletSpawner.TryGetInactiveSpawner(out BfgBulletSpawner spawner))
            {
                ret = new PlayerBullet[]
                {
                    PoolList.Get<BfgBulletSpawner>(playerFirePos, weaponLevel)
                };
            }
            else
            {
                ret = base.GetBullets(weaponLevel, playerFirePos);

                // Don't need to call DeactivateSelft() - SetActive(false) is enough.
                spawner.gameObject.SetActive(false);
            }

            return ret;
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}
