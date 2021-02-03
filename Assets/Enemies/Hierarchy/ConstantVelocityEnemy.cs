using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
    public abstract class ConstantVelocityEnemy : Enemy
    {
        public override void RunFrame(float deltaTime)
        {
            base.RunFrame(deltaTime);
            transform.Translate(deltaTime * Velocity);
        }
    }
}