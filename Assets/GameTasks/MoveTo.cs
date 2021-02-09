using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    public class MoveTo : GameTask
    {
        private Vector2 Destination { get; set; }

        private Vector2 StartPosition { get; set; }
        private Vector2 PositionDifference { get; set; }

        public MoveTo(GameTaskRunner target, Vector2 to, Vector2 from, float duration) : base(target, duration)
        {
            Destination = to;
            StartPosition = from;
            PositionDifference = to - from;
        }

        public override void RunFrame(float deltaTime)
        {
            if (!Timer.Activated)
            {
                Timer.Increment(deltaTime);
                Target.transform.position = CalculateNewPosition();
            }
        }
        private Vector2 CalculateNewPosition()
        {
            var ret = StartPosition + (Timer.RatioComplete * PositionDifference);
            return ret;
        }
    }
}
