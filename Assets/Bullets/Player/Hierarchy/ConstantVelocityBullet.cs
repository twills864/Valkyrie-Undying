using Assets.Bullets.PlayerBullets;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    public abstract class ConstantVelocityBullet : PlayerBullet
    {
        protected virtual void OnConstantVelocityBulletInit() { }
        protected override void OnPlayerBulletInit()
        {
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