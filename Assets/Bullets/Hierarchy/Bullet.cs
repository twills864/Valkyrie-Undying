using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Bullets
{
    public abstract class Bullet : PooledObject
    {
        protected virtual void OnBulletInit() { }
        public override sealed void OnInit()
        {
            OnBulletInit();
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (CollisionUtil.IsDestructor(collision))
            {
                CollideWithDestructor();
            }
        }

        protected virtual void CollideWithDestructor()
        {
            DeactivateSelf();
        }
    }
}