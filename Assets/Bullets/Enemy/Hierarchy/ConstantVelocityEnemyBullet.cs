namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public abstract class ConstantVelocityEnemyBullet : EnemyBullet
    {
        protected virtual void OnConstantVelocityEnemyBulletInit() { }
        protected override sealed void OnEnemyBulletInit()
        {
            OnConstantVelocityEnemyBulletInit();
        }

        protected virtual void RunConstantVelocityEnemyBulletFrame(float deltaTime) { }
        public override void RunFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
            RunConstantVelocityEnemyBulletFrame(deltaTime);
        }
    }
}