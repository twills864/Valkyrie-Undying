using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    public class LinearVelocityChanger : VelocityChanger
    {
        private Vector2 StartVelocity { get; set; }
        private Vector2 VelocityDifference { get; set; }
        private FrameTimer FrameTimer { get; set; }


        private Vector2 EndVelocity { get; set; }
        private float Duration { get; set; }


        // Creates a new LinearVelocityChanger with no specified velocity change.
        public LinearVelocityChanger(ManagedVelocityObject target) : base(target)
        {
            FrameTimer = new FrameTimer(1f);
            FrameTimer.ActivateSelf();
        }

        // Creates a new LinearVelocityChanger with the specified velocity change.
        public LinearVelocityChanger(ManagedVelocityObject target,
            Vector2 startVelocity, Vector2 endVelocity, float duration) : base(target)
        {
            Init(startVelocity, endVelocity, duration);
        }

        // Initializes this LinearVelocityChanger with the specified velocity change.
        public void Init(Vector2 startVelocity, Vector2 endVelocity, float duration)
        {
            StartVelocity = startVelocity;
            VelocityDifference = endVelocity - startVelocity;
            FrameTimer = new FrameTimer(duration);

            EndVelocity = endVelocity;
            Duration = duration;
        }

        // Initializes this LinearVelocityChanger to the specified velocity change.
        public void Init(Vector2 velocity)
        {
            StartVelocity = velocity;
            VelocityDifference = Vector2.zero;
            FrameTimer = new FrameTimer(1f);
            FrameTimer.ActivateSelf();

            EndVelocity = velocity;
            Duration = 1f;
        }


        private Vector2 CalculateNewVelocity()
        {
            var ret = StartVelocity + (FrameTimer.RatioComplete * VelocityDifference);
            return ret;
        }
        public override void RunFrame(float deltaTime)
        {
            if (!FrameTimer.Activated)
            {
                FrameTimer.Increment(deltaTime);
                Target.Velocity = CalculateNewVelocity();
            }
        }
    }
}
