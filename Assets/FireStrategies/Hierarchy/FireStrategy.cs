using Assets.Bullets;
using Assets.FireStrategyManagers;
using Assets.Util;

namespace Assets.FireStrategies
{
    /// <inheritdoc/>
    public abstract class FireStrategy
    {
        public LoopingFrameTimer FireTimer { get; protected set; }

        public FireStrategy()
        {
        }

        protected virtual void OnReset() { }
        public void Reset()
        {
            FireTimer.ActivateSelf();
            OnReset();
        }

        public bool UpdateActivates(float deltaTime) => FireTimer.UpdateActivates(deltaTime);
        public bool UpdateActivates(float deltaTime, out float overflowDeltaTime) => FireTimer.UpdateActivates(deltaTime, out overflowDeltaTime);
    }

    /// <inheritdoc/>
    public abstract class FireStrategy<TBullet> : FireStrategy where TBullet : Bullet
    {
        protected TBullet ObjectPrefab { get; }

        //public FireStrategy() : this(null)
        //{
        //}
        public FireStrategy(TBullet bulletPrefab) : base()
        {
            ObjectPrefab = bulletPrefab;
        }
    }
}
