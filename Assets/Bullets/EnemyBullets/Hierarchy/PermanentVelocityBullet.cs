using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EnemyBullets
{
    public abstract class PermanentVelocityEnemyBullet : ConstantVelocityEnemyBullet
    {
        [SerializeField]
        protected float PermanentVelocityX;
        [SerializeField]
        protected float PermanentVelocityY;

        public sealed override Vector2 Velocity => new Vector2(PermanentVelocityX, PermanentVelocityY);

        protected void OnPermanentVelocityBulletInit() { }
        protected override sealed void OnConstantVelocityEnemyBulletInit()
        {
            base.OnConstantVelocityEnemyBulletInit();
            OnPermanentVelocityBulletInit();
        }
    }
}