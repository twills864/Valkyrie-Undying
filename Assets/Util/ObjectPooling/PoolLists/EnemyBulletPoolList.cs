using Assets.Bullets.EnemyBullets;
using Assets.EnemyBullets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    public class EnemyBulletPoolList : PoolList<EnemyBullet>
    {
        [SerializeField]
        private BasicEnemyBullet BasicPrefab;
        [SerializeField]
        private TankEnemyBullet TankPrefab;
    }
}
