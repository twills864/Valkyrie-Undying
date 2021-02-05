using Assets.Bullets;
using Assets.FireStrategies;
using Assets.FireStrategies.EnemyFireStrategies;
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

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new TankEnemyStrategy();
    }
}