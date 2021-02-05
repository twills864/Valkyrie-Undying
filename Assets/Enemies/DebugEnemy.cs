using Assets.Bullets;
using Assets.FireStrategies;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
    public class DebugEnemy : Enemy
    {
        public override int BaseSpawnHealth => 100000;
        public override float SpawnHealthScaleRate => 1.0f;

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new DebugEnemyStrategy();
    }
}