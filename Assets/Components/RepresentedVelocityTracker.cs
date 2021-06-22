using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Components
{
    public class RepresentedVelocityTracker : ValkyrieComponent
    {
        private const float OneFrame = 1 / 60f;

        private Vector2 ThisPosition;
        private Vector2 LastPosition;
        private float LastRealDeltaTime = OneFrame;

        public Vector2 DistanceDelta => (ThisPosition - LastPosition);
        public Vector2 RepresentedVelocity => DistanceDelta / LastRealDeltaTime;

        public Vector3 CurrentWorldPosition => Host.transform.TransformPoint(Vector3.zero);

        public RepresentedVelocityTracker(ValkyrieSprite host) : base(host)
        {

        }

        public void OnSpawn()
        {
            ThisPosition = CurrentWorldPosition;
            LastPosition = ThisPosition;

            LastRealDeltaTime = OneFrame;
        }

        public override void RunFrame(float deltaTime, float realDeltaTime)
        {
            LastRealDeltaTime = realDeltaTime;
        }

        public void OnLateUpdate()
        {
            LastPosition = ThisPosition;
            ThisPosition = CurrentWorldPosition;
        }
    }
}
