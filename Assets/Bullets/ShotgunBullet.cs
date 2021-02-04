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
        public float BulletOffset;
        [SerializeField]
        public float BulletSpread;
    }
}