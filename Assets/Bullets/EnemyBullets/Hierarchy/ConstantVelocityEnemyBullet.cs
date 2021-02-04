using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.EnemyBullets
{
    public abstract class ConstantVelocityEnemyBullet : EnemyBullet
    {
        protected virtual void OnConstantVelocityEnemyBulletInit() { }
        protected override sealed void OnBulletInit()
        {
            base.OnBulletInit();
            OnConstantVelocityEnemyBulletInit();
        }
        public override void RunFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
        }
    }
}