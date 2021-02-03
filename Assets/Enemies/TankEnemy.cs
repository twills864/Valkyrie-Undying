using Assets.Bullets;
using Assets.EnemyFireStrategies;
using Assets.FireStrategies;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
    public class TankEnemy : PermanentVelocityEnemy
    {
        public override int BaseSpawnHealth => 250;
        public override float SpawnHealthScaleRate => 2.5f;

        protected override EnemyFireStrategy DefaultEnemyFireStrategy
            => new TankEnemyStrategy();
    }
}