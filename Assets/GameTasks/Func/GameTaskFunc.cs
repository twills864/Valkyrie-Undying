using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    public class GameTaskFunc : FiniteTimeGameTask
    {
        private Action Func;

        public GameTaskFunc(GameTaskRunner target, Action func) : base(target, 0f)
        {
            Func = func;
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            Func();
        }

        public static GameTaskFunc DeactivateSelf(PooledObject runner)
        {
            Action deactivateSelf = () => runner.DeactivateSelf();
            GameTaskFunc ret = new GameTaskFunc(runner, deactivateSelf);
            return ret;
        }
    }
}
