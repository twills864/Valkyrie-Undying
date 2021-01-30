using Assets.Util;
using LogUtilAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public override void RunFrame(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void log(params Expression<Func<BasicBullet, object>>[] expressions)
        {
            LogExpression(expressions);
        }
        public override void Init()
        {

        }

        protected override void _Init()
        {

        }
    }
}