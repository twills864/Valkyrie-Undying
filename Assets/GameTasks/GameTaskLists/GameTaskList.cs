using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks.GameTaskLists
{
    public class GameTaskList : List<GameTask>
    {
        public void RunFrames(float deltaTime)
        {
            for (int i = Count-1; i >= 0; i--)
            {
                var task = this[i];

                task.RunFrame(deltaTime);
                if (task.IsFinished)
                    RemoveAt(i);
            }
        }
    }
}
