using Assets.Bullets;
using Assets.Util;
using Assets.Util.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FireStrategies
{
    public class ShotgunStrategy : FireStrategy<ShotgunBullet>
    {
        private const float ShotgunSpeedY = 8.0f;
        public override LoopingFrameTimer DefaultFireTimer => new LoopingFrameTimer(0.75f);

        public override Bullet[] GetBullets(Vector2 playerFirePos)
        {
            // Default value until weapon level is implemented
            const int numToGet = 5;
            ShotgunBullet[] ret = PoolManager.Instance.BulletPool.Get<ShotgunBullet>(numToGet);

            var first = ret[0];
            float width = first.GetComponent<Renderer>().bounds.size.x + first.BulletOffset;
            float spread = first.BulletSpread;

            for(int i = 0; i < numToGet; i++)
            {
                int offsetIndex = i - 2;
                float posX = offsetIndex * width;

                Vector2 newFirePos = playerFirePos;
                newFirePos.x += posX;

                var bullet = ret[i];
                bullet.Velocity = new Vector2(offsetIndex * spread, ShotgunSpeedY);
                bullet.transform.position = newFirePos;
            }
            return ret;
        }
    }
}
