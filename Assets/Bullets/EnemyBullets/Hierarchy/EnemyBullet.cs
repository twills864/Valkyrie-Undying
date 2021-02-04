using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EnemyBullets
{
    public abstract class EnemyBullet : Bullets.Bullet
    {
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
