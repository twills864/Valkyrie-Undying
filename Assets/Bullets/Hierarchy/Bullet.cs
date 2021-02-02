using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Bullets
{
    public abstract class Bullet : PooledObject
    {
        public override string LogTagColor => "#B381FE";

        [SerializeField]
        protected int BaseDamage;
        public virtual int Damage => BaseDamage;



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