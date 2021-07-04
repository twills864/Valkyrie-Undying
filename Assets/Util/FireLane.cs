using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Information about a bullet fired being fired as part of a spread
    /// that shows how much its position should be offset from the initial
    /// fire point, and what its velocity should be.
    /// </summary>
    public struct FireLane
    {
        public Vector3 PositionOffset;
        public Vector2 Velocity;

        public FireLane(Vector3 positionOffset, Vector2 velocity)
        {
            PositionOffset = positionOffset;
            Velocity = velocity;
        }

        public void ApplyToSprite(PooledObject sprite, Vector3 basePosition)
        {
            sprite.transform.position = basePosition + PositionOffset;
            sprite.Velocity = Velocity;

            sprite.OnSpawn();
        }
    }

    /// <summary>
    /// Information about a bullet fired being fired as part of a spread
    /// that shows how much its position should be offset from the initial
    /// fire point, what its velocity should be, and what angle the sprite
    /// should be rotated to.
    /// </summary>
    public struct FireLaneWithAngle
    {
        public Vector3 PositionOffset;
        public Vector2 Velocity;
        public float Angle;

        public FireLaneWithAngle(Vector3 positionOffset, Vector2 velocity, float angle)
        {
            PositionOffset = positionOffset;
            Velocity = velocity;
            Angle = angle;
        }

        public void ApplyToSprite(PooledObject sprite, Vector3 basePosition)
        {
            sprite.transform.position = basePosition + PositionOffset;
            sprite.Velocity = Velocity;
            sprite.RotationDegrees = Angle;

            sprite.OnSpawn();
        }
    }
}
