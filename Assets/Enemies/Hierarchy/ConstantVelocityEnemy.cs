namespace Assets.Enemies
{
    /// <inheritdoc/>
    public abstract class ConstantVelocityEnemy : Enemy
    {
        protected virtual void OnConstantVelocityEnemyFrame(float deltaTime) { }
        protected sealed override void OnEnemyFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
            base.OnEnemyFrame(deltaTime);
        }
    }
}