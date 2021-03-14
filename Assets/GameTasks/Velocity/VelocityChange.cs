using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    public class VelocityChange : FiniteVelocityGameTask
    {
        private Vector2 StartVelocity { get; set; }
        private Vector2 EndVelocity { get; set; }
        private Vector2 VelocityDifference { get; set; }


        #region Create
        public VelocityChange(ValkyrieSprite target, Vector2 startVelocity, Vector2 endVelocity, float duration) : base(target, duration)
        {
            StartVelocity = startVelocity;
            EndVelocity = endVelocity;
            VelocityDifference = endVelocity - startVelocity;

            Velocity = StartVelocity;
        }

        public static VelocityChange Default(ValkyrieSprite target, float duration)
        {
            var ret = new VelocityChange(target, duration);
            ret.FinishSelf();
            return ret;
        }
        private VelocityChange(ValkyrieSprite target, float duration) : base(target, duration)
        {
            StartVelocity = Vector2.zero;
            EndVelocity = Vector2.zero;
            VelocityDifference = Vector2.zero;

            Velocity = Vector2.zero;
        }

        #endregion Create

        protected override void OnFiniteVelocityTaskFrameRun(float deltaTime)
        {
            Velocity = StartVelocity + (Timer.RatioComplete * VelocityDifference);
        }

        public void Init(Vector2 startVelocity, Vector2 endVelocity)
        {
            StartVelocity = startVelocity;
            EndVelocity = endVelocity;
            VelocityDifference = EndVelocity - StartVelocity;

            Velocity = StartVelocity;

            Timer.Reset();
        }

    }
}
