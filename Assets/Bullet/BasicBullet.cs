using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Bullet
{
    public class BasicBullet : Bullet
    {
        override protected int BaseDamage => 20;
        private const float BulletSpeed = 10f;

        protected override void _Update(float deltaTime)
        {
            transform.Translate(new Vector3(0, deltaTime * BulletSpeed));
        }
    }
}