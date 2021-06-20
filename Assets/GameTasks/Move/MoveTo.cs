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
        #region Property Fields
        private Vector3 _destination;
        private Vector3 _totalPositionDifference;
        private Vector3 _startPosition;
        #endregion Property Fields

        public Vector3 Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                TotalPositionDifference = value - StartPosition;
            }
        }

        public Vector3 StartPosition
        {
            get => _startPosition;
            set
            {
                _startPosition = value;
                TotalPositionDifference = Destination - value;
            }
        }

        public Vector3 TotalPositionDifference
        {
            get => _totalPositionDifference;
            private set
            {
                _totalPositionDifference = value;
                DistanceChanged();
            }
        }

        private void DistanceChanged()
        {
            Velocity = TotalPositionDifference / Duration;
        }

        public MoveTo(ValkyrieSprite target, Vector3 from, Vector3 to, float duration) : base(target, duration)
        {
            _destination = to;
            StartPosition = from;
        }

        public MoveTo(ValkyrieSprite target, Vector3 to, float duration) : this(target, target.transform.position, to, duration)
        {

        }

        public void ReinitializeMove(Vector3 startPosition, Vector3 destination)
        {
            _destination = destination;
            StartPosition = startPosition;
        }

        public static MoveTo Default(ValkyrieSprite target, float duration)
        {
            var ret = new MoveTo(target, target.transform.position, duration);
            ret.FinishSelf();
            return ret;
        }
    }
}
