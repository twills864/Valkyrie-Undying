using System;
using Assets.ColorManagers;
using UnityEngine;

namespace Assets.ObjectPooling
{
    /// <summary>
    /// Manages each Object Pool contained within the main game scene.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        /// <summary>
        /// A static reference to the single instance of the Pool Manager.
        /// </summary>
        public static PoolManager Instance { get; private set; }

        public BulletPoolList BulletPool;
        public EnemyPoolList EnemyPool;
        public EnemyBulletPoolList EnemyBulletPool;
        public UIElementPoolList UIElementPool;

        /// <summary>
        /// An array of each Object Pool managed by this class.
        /// </summary>
        private PoolList[] AllPoolLists => new PoolList[]
        {
            BulletPool,
            EnemyBulletPool,

            // EnemyPool depends on EnemyBulletPool in order to load fire strategies.
            EnemyPool,
            UIElementPool
        };

        public void Init(in ColorManager colorManager)
        {
            foreach(var pool in AllPoolLists)
                pool.Init(in colorManager);
        }

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Calls RecolorObjects() on each player-based Object Pool managed by this class.
        /// </summary>
        /// <param name="color">The color to give to each object.</param>
        public void RecolorPlayerActivity(Color color)
        {
            BulletPool.RecolorElements(color);
        }
    }
}
