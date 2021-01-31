using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Bullets
{
    public abstract class ConstantVelocityBullet : Bullet
    {
        public override void RunFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
        }
    }
}