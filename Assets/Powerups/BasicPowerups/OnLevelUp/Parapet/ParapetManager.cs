using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    public class ParapetManager
    {
        private ParapetBullet LeftParapet { get; set; }
        private ParapetBullet RightParapet { get; set; }

        public ParapetManager(ParapetBullet prefab)
        {
            LeftParapet = InitParapet(prefab);
            RightParapet = InitParapet(prefab);
        }

        private ParapetBullet InitParapet(ParapetBullet prefab)
        {
            ParapetBullet ret = GameObject.Instantiate(prefab);

            ret.Init();
            ret.transform.localScale = Vector3.zero;
            ret.Alpha = 0;
            ret.gameObject.SetActive(false);

            return ret;
        }

        public void ActivateParapets(float height, Vector3 scale)
        {
            LeftParapet.Activate(height, scale);
            RightParapet.Activate(height, scale);
        }

        public Vector3 Position
        {
            set
            {
                Vector3 parapetPos = value;
                parapetPos.y += LeftParapet.HeightOffset;

                float parapetOffset = SpaceUtil.WorldMapSize.x * 0.5f;
                LeftParapet.transform.position = parapetPos.AddX(-parapetOffset);
                RightParapet.transform.position = parapetPos.AddX(parapetOffset);
            }
        }

        public Color SpriteColor
        {
            set
            {
                LeftParapet.SpriteColor = value;
                RightParapet.SpriteColor = value;
            }
        }

        public void Kill()
        {
            LeftParapet.DeactivateSelf();
            RightParapet.DeactivateSelf();
        }
    }
}
