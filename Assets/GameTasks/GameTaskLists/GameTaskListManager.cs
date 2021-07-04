using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Util;

namespace Assets.GameTasks.GameTaskLists
{
    /// <summary>
    /// Manages the list of GameTasks currently being handled by the GameManager.
    /// </summary>
    public class GameTaskListManager
    {
        private GameTaskList Tasks { get; } = new GameTaskList();

        public void RunFrames(float deltaTime)
        {
            Tasks.RunFrames(deltaTime);
        }

        public void AddTask(GameTask task)
        {
            Tasks.Add(task);
        }

        public void GameTaskRunnerDeactivated(ValkyrieSprite target)
        {
            var taskType = target.TimeScale;
            Tasks.RemoveTasksRelatedToTarget(target);
        }
    }
}
