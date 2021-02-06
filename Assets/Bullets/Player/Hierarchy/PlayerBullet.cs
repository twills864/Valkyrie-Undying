using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    public abstract class PlayerBullet : Bullet
    {
        public override string LogTagColor => "#B381FE";

        [SerializeField]
        protected int BaseDamage;
        public virtual int Damage => BaseDamage;

        protected virtual void OnPlayerBulletInit() { }
        protected sealed override void OnBulletInit()
        {
            OnPlayerBulletInit();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayerBullet(collision))
            {
                Log("Bullet self-collision!");
                //GameManager.Instance.CreateFleetingText("X", this.transform.position);
                DebugUtil.RedX(transform.position, "Player bullet collide");
            }
        }
    }
}
