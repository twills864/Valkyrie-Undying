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

            DebugUI.SetDebugLabel("LEFT", () => LeftParapet.HeightOffset);
        }

        private ParapetBullet InitParapet(ParapetBullet prefab)
        {
            ParapetBullet ret = GameObject.Instantiate(prefab);

            ret.Init();
            ret.Alpha = 0;
            ret.gameObject.SetActive(false);

            return ret;
        }

        public void ActivateParapets(float height)
        {
            Activate(LeftParapet, height);
            Activate(RightParapet, height);
        }

        private void Activate(ParapetBullet parapet, float height)
        {
            parapet.gameObject.SetActive(true);

            const float FadeInTime = 1.0f;
            var fadeIn = new FadeTo(parapet, 0f, 1.0f, FadeInTime);
            parapet.RunTask(fadeIn);

            parapet.StartRise(height);
        }

        public Vector3 Position
        {
            set
            {
                Vector3 parapetPos = value;
                parapetPos.y += LeftParapet.HeightOffset;

                float parapetOffset = SpaceUtil.WorldMapSize.x * 0.5f;
                LeftParapet.transform.position = VectorUtil.AddX(parapetPos, -parapetOffset);
                RightParapet.transform.position = VectorUtil.AddX(parapetPos, parapetOffset);
            }
        }
    }
}
