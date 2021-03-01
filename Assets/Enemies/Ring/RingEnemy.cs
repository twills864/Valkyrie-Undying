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
        private float RotationSpeed;
        [SerializeField]
        private float MinimumTravelTime;

        public override int BaseSpawnHealth => 100;
        public override float SpawnHealthScaleRate => 1.25f;

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new RingEnemyStrategy();

        public RingEnemyRing Ring { get; set; }

        public override void OnSpawn()
        {
            Ring = PoolManager.Instance.EnemyPool.Get<RingEnemyRing>();
            Ring.Init(this.transform.position);
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
                Ring.transform.position = transform.position;
        }

        protected override void OnDeactivate()
        {
            if (Ring != null)
            {
                Ring.DeactivateSelf();
                Ring = null;
            }
        }
    }
}