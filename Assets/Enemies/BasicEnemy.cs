using Assets.Bullets;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
    public class BasicEnemy : Enemy
    {
        public override int BaseSpawnHealth => 100;
        public override float SpawnHealthScaleRate => 1.0f;
    }
}