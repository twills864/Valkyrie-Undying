using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class SmiteLightningBullet : SmiteBullet
    {

        public override float Scale
        {
            get => LocalScaleX;
            set => LocalScaleX = value;
        }

        //protected override void OnPlayerBulletInit()
        //{

        //}

        //protected override void OnActivate()
        //{

        //}

        //public override void OnSpawn()
        //{

        //}

        //protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        //{

        //}
    }
}