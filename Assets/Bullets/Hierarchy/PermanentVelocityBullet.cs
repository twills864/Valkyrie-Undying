using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Bullets
{
    public abstract class PermanentVelocityBullet : ConstantVelocityBullet
    {
        [SerializeField]
        protected float PermanentVelocityX;
        [SerializeField]
        protected float PermanentVelocityY;

        public sealed override Vector2 Velocity => new Vector2(PermanentVelocityX, PermanentVelocityY);

        protected virtual void OnPermanentVelocityBulletInit() { }
        protected override sealed void OnConstantVelocityBulletInit()
        {
            base.OnConstantVelocityBulletInit();
            OnPermanentVelocityBulletInit();
        }

        protected virtual void RunPermanentVelocityBulletFrame(float deltaTime) { }
        protected sealed override void RunConstantVelocityBulletFrame(float deltaTime)
        {
            base.RunConstantVelocityBulletFrame(deltaTime);
            RunPermanentVelocityBulletFrame(deltaTime);
        }
    }
}