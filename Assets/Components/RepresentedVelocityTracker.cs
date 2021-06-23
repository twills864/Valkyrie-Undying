using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Components
{
    public class RepresentedVelocityTracker : ValkyrieComponent
    {
        private const int FrameHistoryLength = 4;
        private const float OneFrame = 1 / 60f;

        private class FrameHistory
        {
            public Vector2 Position;
            public float RealDeltaTime;

            public void Set(Vector2 position, float realDeltaTime)
            {
                Position = position;
                RealDeltaTime = realDeltaTime;
            }

            public void Reset(Vector2 startingPosition)
            {
                Position = Vector2.zero;
                RealDeltaTime = OneFrame;
            }
        }

        private CircularSelector<FrameHistory> FrameHistories { get; }

        private Vector2 ThisPosition => FrameHistories.Current.Position;
        private Vector2 LastPosition => FrameHistories.Previous.Position;
        private float TotalDeltaTime => FrameHistories.Select(x => x.RealDeltaTime).Sum();

        public Vector2 DistanceDelta => ThisPosition - LastPosition;
        public Vector2 RepresentedVelocity => DistanceDelta / TotalDeltaTime;

        public Vector3 CurrentWorldPosition => Host.transform.TransformPoint(Vector3.zero);

        public RepresentedVelocityTracker(ValkyrieSprite host) : base(host)
        {
            FrameHistories = new CircularSelector<FrameHistory>(FrameHistoryLength);

            for(int i = 0; i < FrameHistoryLength; i++)
            {
                FrameHistory history = new FrameHistory();
                history.Reset(Vector3.zero);
                FrameHistories.Add(history);
            }
        }

        public void OnSpawn()
        {
            Vector3 currentWorldPosition = CurrentWorldPosition;
            for (int i = 0; i < FrameHistories.Count; i++)
                FrameHistories[i].Reset(currentWorldPosition);
        }

        public override void RunFrame(float deltaTime, float realDeltaTime)
        {
            FrameHistories.Increment();
            FrameHistories.Current.RealDeltaTime = realDeltaTime;
        }

        public void OnLateUpdate()
        {
            FrameHistories.Current.Position = CurrentWorldPosition;
        }
    }
}
