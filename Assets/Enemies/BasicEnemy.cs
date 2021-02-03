
using Assets.EnemyFireStrategies;
using Assets.FireStrategies;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
    public class BasicEnemy : PermanentVelocityEnemy
    {
        public override int BaseSpawnHealth => 100;
        public override float SpawnHealthScaleRate => 1.0f;

        protected override EnemyFireStrategy DefaultEnemyFireStrategy
            => new BasicEnemyStrategy();
    }
}