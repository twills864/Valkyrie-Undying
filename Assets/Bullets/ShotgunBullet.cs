using Assets.Util;
using LogUtilAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace Assets.Bullets
{
    public class ShotgunBullet : ConstantVelocityBullet
    {
        [SerializeField]
        public float BulletOffsetX;
        [SerializeField]
        public float BulletOffsetY;
        [SerializeField]
        public float BulletSpreadX;
        [SerializeField]
        public float BulletSpreadY;
    }
}