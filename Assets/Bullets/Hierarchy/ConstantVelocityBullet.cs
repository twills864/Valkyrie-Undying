using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Bullets
{
    public abstract class ConstantVelocityBullet : Bullet
    {
        protected virtual void OnConstantVelocityBulletInit() { }
        public override sealed void Init(Vector2 position)
        {
            base.Init(position);
            OnConstantVelocityBulletInit();
        }
        public override void RunFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
        }
    }
}