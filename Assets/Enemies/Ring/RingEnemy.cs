using Assets.Constants;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class RingEnemy : Enemy
    {
        [SerializeField]
        private float RotationSpeed = GameConstants.PrefabNumber;
        [SerializeField]
        private float MinimumTravelTime = GameConstants.PrefabNumber;

        public override int BaseSpawnHealth => 75;
        public override float SpawnHealthScaleRate => 0.75f;

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new RingEnemyStrategy();

        public RingEnemyRing Ring { get; set; }

        protected override void OnEnemySpawn()
        {
            Ring = PoolManager.Instance.EnemyPool.Get<RingEnemyRing>();
            Ring.transform.position = transform.position;
            Ring.OnSpawn();
            Ring.Host = this;

            float maxY = SpaceUtil.WorldMap.Top.y - Ring.HeightHalf;
            float minY = SpaceUtil.WorldMap.Center.y + Ring.HeightHalf;

            float targetY = RandomUtil.Float(minY, maxY);
            Vector2 destination = new Vector2(transform.position.x, targetY);

            float distance = transform.position.y - targetY;
            float duration = MinimumTravelTime + distance;

            var moveTo = new MoveTo(this, destination, duration);
            var easeIn = new EaseIn(moveTo);
            RunTask(easeIn);
        }

        protected override void OnEnemyFrame(float deltaTime)
        {
            transform.Rotate(0, 0, deltaTime * RotationSpeed);

            if (Ring != null)
                Ring.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
        }

        protected override void OnEnemyDeactivate()
        {
            if (Ring != null)
            {
                Ring.DeactivateSelf();
                Ring = null;
            }
        }
    }
}