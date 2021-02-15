using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <inheritdoc/>
    public class MoveTo : FiniteMovementGameTask
    {
        private Vector2 Destination { get; set; }

        private Vector2 StartPosition { get; set; }
        private Vector2 TotalPositionDifference { get; set; }

        public MoveTo(GameTaskRunner target, Vector2 to, Vector2 from, float duration) : base(target, duration)
        {
            Destination = to;
            StartPosition = from;
            TotalPositionDifference = to - from;

            Velocity = TotalPositionDifference / Duration;
        }

        public MoveTo(GameTaskRunner target, Vector2 to, float duration) : base(target, duration)
        {
            Destination = to;
            StartPosition = target.transform.position;
            TotalPositionDifference = to - StartPosition;

            Velocity = TotalPositionDifference / Duration;
        }
    }
}
