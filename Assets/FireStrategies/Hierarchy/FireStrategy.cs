using Assets.Bullets;
using Assets.Util;

namespace Assets.FireStrategies
{
    /// <inheritdoc/>
    public abstract class FireStrategy
    {
        public abstract LoopingFrameTimer FireTimer { get; protected set; }
        public virtual void Reset() => FireTimer.ActivateSelf();

        public bool UpdateActivates(float deltaTime) => FireTimer.UpdateActivates(deltaTime);
    }

    /// <inheritdoc/>
    public abstract class FireStrategy<TBullet> : FireStrategy where TBullet : Bullet
    {
        protected TBullet ObjectPrefab { get; }

        public FireStrategy() : this(null)
        {
        }
        public FireStrategy(TBullet bulletPrefab)
        {
            ObjectPrefab = bulletPrefab;
        }
    }
}
