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
            EnemyPool,
            EnemyBulletPool,
            UIElementPool
        };


        public void Init()
        {
            foreach(var pool in AllPoolLists)
                pool.Init();
        }

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Calls RunFrames() on each Object Pool managed by this class.
        /// </summary>
        /// <param name="bulletDt">Delta time as experienced by each bullet.</param>
        /// <param name="enemyDt">Delta time as experienced by each enemy.</param>
        public void RunPoolFrames(float bulletDt, float enemyDt)
        {
            BulletPool.RunFrames(bulletDt);
            EnemyPool.RunFrames(enemyDt);
            EnemyBulletPool.RunFrames(enemyDt);

            // UIElementPool should run last to take into account any changes in position, etc.
            // of elements from previous pools.
            UIElementPool.RunFrames(bulletDt);
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
