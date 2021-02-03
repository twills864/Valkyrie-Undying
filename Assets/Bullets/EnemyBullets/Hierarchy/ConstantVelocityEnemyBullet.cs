using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.EnemyBullets
{
    public abstract class ConstantVelocityEnemyBullet : EnemyBullet
    {
        protected virtual void OnConstantVelocityEnemyBulletInit() { }
        public override sealed void Init(Vector2 position)
        {
            base.Init(position);
            OnConstantVelocityEnemyBulletInit();
        }
        public override void RunFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
        }
    }
}