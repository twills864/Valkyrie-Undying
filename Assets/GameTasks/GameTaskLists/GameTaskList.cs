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
                var target = task.Target;

#if DEBUG
                if (!target.isActiveAndEnabled)
                {
                    LogUtil.Log("Disabled task is running in GameTaskList!", target);
                    System.Diagnostics.Debugger.Break();
                }
#endif

                if (!target.IsPaused)
                {
                    float scaledTime = deltaTime * target.TimeScaleModifier * target.RetributionTimeScale;
                    task.RunFrame(scaledTime);
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
