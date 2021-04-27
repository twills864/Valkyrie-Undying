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
        private Vector3 Destination { get; set; }

        private Vector3 StartPosition { get; set; }
        private Vector3 TotalPositionDifference { get; set; }

        public MoveTo(ValkyrieSprite target, Vector3 from, Vector3 to, float duration) : base(target, duration)
        {
            Destination = to;
            StartPosition = from;
            TotalPositionDifference = to - from;

            Velocity = TotalPositionDifference / Duration;
        }

        public MoveTo(ValkyrieSprite target, Vector3 to, float duration) : base(target, duration)
        {
            Destination = to;
            StartPosition = target.transform.position;
            TotalPositionDifference = to - StartPosition;

            Velocity = TotalPositionDifference / Duration;
        }

        public void ReinitializeMove(Vector3 startPosition, Vector3 destination)
        {
            StartPosition = startPosition;
            Destination = destination;

            TotalPositionDifference = destination - startPosition;

            Velocity = TotalPositionDifference / Duration;
        }
    }
}
