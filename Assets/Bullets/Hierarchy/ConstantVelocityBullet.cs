using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Bullets
{
    public abstract class ConstantVelocityBullet : Bullet
    {
        protected virtual void OnConstantVelocityBulletInit() { }
        protected override sealed void OnBulletInit()
        {
            base.OnBulletInit();
            OnConstantVelocityBulletInit();
        }

        protected virtual void RunConstantVelocityBulletFrame(float deltaTime) { }
        public sealed override void RunFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
            RunConstantVelocityBulletFrame(deltaTime);
        }
    }
}