using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    public abstract class EnemyBullet : Bullet
    {
        public override string LogTagColor => "#FFA197";

        protected virtual void OnEnemyBulletInit() { }
        protected sealed override void OnBulletInit()
        {
            OnEnemyBulletInit();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsPlayer(collision))
            {
                if (Player.Instance.CollideWithBullet(this))
                    DeactivateSelf();
            }
        }
    }
}
