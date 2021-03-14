using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class RingEnemy : PermanentVelocityEnemy
    {
        [SerializeField]
        private float RotationSpeed = GameConstants.PrefabNumber;
        [SerializeField]
        private float MinimumTravelTime = GameConstants.PrefabNumber;

        public RingEnemyRing Ring { get; set; }

        private EaseIn Ease { get; set; }

        protected override EnemyFireStrategy InitialFireStrategy()
            => new VariantLoopingEnemyFireStrategy<RingEnemyBullet>(FireSpeed, FireSpeedVariance);

        protected override void OnEnemySpawn()
        {
            Ring = PoolManager.Instance.EnemyPool.Get<RingEnemyRing>();
            Ring.transform.position = transform.position;
            Ring.OnSpawn();
            Ring.Host = this;

            float maxY = SpaceUtil.WorldMap.Top.y - Ring.HeightHalf;
            float minY = SpaceUtil.WorldMap.Center.y + Ring.HeightHalf;

            float targetY = RandomUtil.Float(minY, maxY);
            Vector3 destination = new Vector3(transform.position.x, targetY);

            float distance = transform.position.y - targetY;
            float duration = MinimumTravelTime + distance;

            var moveTo = new MoveTo(this, destination, duration);
            Ease = new EaseIn(moveTo);
        }

        protected override void OnFireStrategyEnemyFrame(float deltaTime)
        {
            Ease.RunFrame(deltaTime);
            RotateSprite(deltaTime * RotationSpeed);

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