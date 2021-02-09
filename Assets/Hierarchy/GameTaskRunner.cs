using System;
using Assets.GameTasks;
using LogUtilAssets;

namespace Assets
{
    /// <summary>
    /// Represents an object that will manually handle concurrent GameTasks
    /// instead of relying on Unity's implementation of StartCoroutine.
    /// Useful for controlling delta times, among other things.
    /// </summary>
    /// <inheritdoc/>
    public abstract class GameTaskRunner : FrameRunner
    {
        protected abstract GameTaskType TaskType { get; }

        public void StartTask(GameTask task)
        {
            GameManager.Instance.StartTask(task, TaskType);
        }
    }
}