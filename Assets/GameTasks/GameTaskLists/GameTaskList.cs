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
    /// <summary>
    /// Represents a lit of GameTasks currently being managed by the GameManager.
    /// Contains logic to ensure that ValkyrieSprites deactivated during each frame run
    /// safely have their relevant GameTasks removed from this list.
    /// </summary>
    public class GameTaskList : List<GameTask>
    {
        private bool CurrentlyIterating = false;
        private HashSet<ValkyrieSprite> TargetsToErase { get; } = new HashSet<ValkyrieSprite>();

        public void RunFrames(float deltaTime)
        {
            CurrentlyIterating = true;
            TargetsToErase.Clear();

            for (int i = Count - 1; i >= 0; i--)
            {
                var task = this[i];
                var target = task.Target;

                if (target.isActiveAndEnabled)
                {
                    if (!target.IsPaused)
                    {
                        float scaledTime = deltaTime * target.TimeScaleModifier * target.RetributionTimeScale;
                        task.RunFrame(scaledTime);
                        if (task.IsFinished)
                            RemoveAt(i);
                    }
                }
            }

            CurrentlyIterating = false;

            if (TargetsToErase.Any())
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    var task = this[i];

                    if (TargetsToErase.Contains(task.Target))
                        RemoveAt(i);
                }
            }
        }

        public void RemoveTasksRelatedToTarget(ValkyrieSprite target)
        {
            if (!CurrentlyIterating)
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    var task = this[i];

                    if (task.Target == target)
                        RemoveAt(i);
                }
            }
            else
                TargetsToErase.Add(target);
        }
    }
}
