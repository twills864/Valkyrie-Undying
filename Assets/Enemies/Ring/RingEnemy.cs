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
        public override AudioClip FireSound => SoundBank.GunPistol;

        #region Prefabs

        [SerializeField]
        private float _MinimumTravelTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float MinimumTravelTime => _MinimumTravelTime;

        #endregion Prefab Properties



        public RingEnemyRing Ring { get; set; }

        private EaseIn Ease { get; set; }

        protected override EnemyFireStrategy InitialFireStrategy()
            => new RingEnemyFireStrategy(VariantFireSpeed);

        protected override void OnEnemySpawn()
        {
            Ring = PoolManager.Instance.EnemyPool.Get<RingEnemyRing>();
            Ring.transform.position = transform.position;
            Ring.OnSpawn();
            Ring.Host = this;

            PointValue += Ring.CurrentHealth;

            float maxY = SpaceUtil.WorldMap.Top.y - Ring.HeightHalf;
            float minY = SpaceUtil.WorldMap.Center.y + Ring.HeightHalf;

            float targetY = RandomUtil.Float(minY, maxY);
            Vector3 destination = new Vector3(transform.position.x, targetY);

            float distance = transform.position.y - targetY;
            float duration = MinimumTravelTime + distance;

            var moveTo = new MoveTo(this, destination, duration);
            Ease = new EaseIn(moveTo);
        }

        protected override void OnFireStrategyEnemyFrame(float deltaTime, float realDeltaTime)
        {
            Ease.RunFrame(deltaTime);

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