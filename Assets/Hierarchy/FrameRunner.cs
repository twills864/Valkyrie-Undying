namespace Assets
{
    /// <summary>
    /// Represents an object that will manually handle frame updates
    /// instead of relying on Unity's implementation of Update().
    /// Useful for controlling delta times.
    /// </summary>
    /// <inheritdoc/>
    public abstract class FrameRunner : ValkyrieSprite
    {
        public float TotalTime { get; set; }

        public abstract void RunFrame(float deltaTime);
    }
}