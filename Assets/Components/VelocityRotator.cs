using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Components
{
    /// <summary>
    /// Tracks a given ValkyrieSprite's velocity, and ensures that it's always
    /// rotated to face the direction it's traveling in.
    /// </summary>
    /// <inheritdoc/>
    public class VelocityRotator : ValkyrieComponent
    {
        private Vector2 LastVelocity;

        public VelocityRotator(ValkyrieSprite host) : base(host)
        {
        }

        public override void RunFrame(float deltaTime, float realDeltaTime)
        {
            if(Host.Velocity != LastVelocity)
            {
                LastVelocity = Host.Velocity;
                AdjustAngle();
            }
        }

        private void AdjustAngle()
        {
            float angle = Vector2.Angle(Vector2.right, LastVelocity);

            if (LastVelocity.y < 0)
                angle = 360f - angle;

            Host.RotationDegrees = angle;
        }
    }
}
