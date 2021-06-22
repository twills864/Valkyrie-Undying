using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Components
{
    public class PositionRotator : ValkyrieComponent
    {
        private Vector3 LastPosition;

        public PositionRotator(ValkyrieSprite host) : base(host)
        {
        }

        public override void RunFrame(float deltaTime, float realDeltaTime)
        {
            Vector3 thisPos = Host.transform.position;
            if (thisPos != LastPosition)
            {
                AdjustAngle(in thisPos);
                LastPosition = thisPos;
            }
        }

        private void AdjustAngle(in Vector3 thisPos)
        {
            Vector3 diff = thisPos - LastPosition;
            float angle = Vector2.Angle(Vector2.right, diff);

            if (diff.y < 0)
                angle = 360f - angle;

            Host.RotationDegrees = angle;
        }
    }
}
