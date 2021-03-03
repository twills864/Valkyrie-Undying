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
    public abstract class GameTaskRunner : ManagedVelocityObject
    {
        public abstract GameTaskType TaskType { get; }
        public bool IsPaused { get; set; }

        public void RunTask(GameTask task)
        {
            GameManager.Instance.StartTask(task, TaskType);
        }

        protected void ClearGameTasks()
        {
            GameManager.Instance.GameTaskRunnerDeactivated(this);
        }
    }
}