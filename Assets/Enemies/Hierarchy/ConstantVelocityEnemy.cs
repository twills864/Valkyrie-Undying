using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
    public abstract class ConstantVelocityEnemy : Enemy
    {
        protected virtual void OnConstantVelocityEnemyFrame(float deltaTime) { }
        protected sealed override void OnEnemyFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
            base.OnEnemyFrame(deltaTime);
        }
    }
}