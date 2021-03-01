using Assets.FireStrategies.EnemyFireStrategies;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class RingEnemy : PermanentVelocityEnemy
    {
        [SerializeField]
        private float RotationSpeed;

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