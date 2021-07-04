using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <summary>
    /// Runs a given Action as soon as the GameTask is activated.
    /// Automatically applies a duration of float.Epsilon to give the illusion
    /// of taking no time, while also allowing for convenient application of
    /// timer activation rules.
    /// </summary>
    /// <inheritdoc/>
    public class GameTaskFunc : FiniteTimeGameTask
    {
        private Action Func;
        public string MethodName => Func.Method.Name;

        public GameTaskFunc(ValkyrieSprite target, Action func) : base(target, float.Epsilon)
        {
            Func = func;
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            Func();
        }

        public static GameTaskFunc DeactivateSelf(PooledObject runner)
        {
            Action deactivateSelf = runner.DeactivateSelf;
            GameTaskFunc ret = new GameTaskFunc(runner, deactivateSelf);
            return ret;
        }

        protected override string ToFiniteTimeGameTaskString() => MethodName;
    }
}
