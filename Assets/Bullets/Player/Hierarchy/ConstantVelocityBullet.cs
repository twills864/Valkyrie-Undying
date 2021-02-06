namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public abstract class ConstantVelocityBullet : PlayerBullet
    {
        protected virtual void OnConstantVelocityBulletInit() { }
        protected override void OnPlayerBulletInit()
        {
            OnConstantVelocityBulletInit();
        }

        protected virtual void RunConstantVelocityBulletFrame(float deltaTime) { }
        public sealed override void RunFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
            RunConstantVelocityBulletFrame(deltaTime);
        }
    }
}