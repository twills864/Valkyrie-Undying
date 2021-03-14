using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using LogUtilAssets;
using UnityEngine;

namespace Assets.GameTasks.GameTaskLists
{
    public class GameTaskList : List<GameTask>
    {
        public void RunFrames(float deltaTime)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                var task = this[i];

#if DEBUG
                if (!task.Target.isActiveAndEnabled)
                {
                    LogUtil.Log("Disabled task is running in GameTaskList!", task.Target);
                    System.Diagnostics.Debugger.Break();
                }
#endif

                if (!task.Target.IsPaused)
                {
                    task.RunFrame(deltaTime);
                    if (task.IsFinished)
                        RemoveAt(i);
                }
            }
        }

        public void RemoveTasksRelatedToTarget(ValkyrieSprite target)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                var task = this[i];

                if (task.Target == target)
                    RemoveAt(i);
            }
        }
    }
}
