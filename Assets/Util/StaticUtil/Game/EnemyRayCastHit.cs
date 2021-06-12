using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;
using UnityEngine;

namespace Assets.Util
{
    public struct EnemyRaycastHit
    {
        public Enemy Enemy;
        public RaycastHit2D RayCastHit;

        public EnemyRaycastHit(Enemy enemy, RaycastHit2D raycastHit)
        {
            Enemy = enemy;
            RayCastHit = raycastHit;
        }
    }
}
