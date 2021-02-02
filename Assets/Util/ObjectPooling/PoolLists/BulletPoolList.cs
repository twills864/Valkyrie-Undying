using Assets.Bullets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    public class BulletPoolList : PoolList<Bullet>
    {
        [SerializeField]
        private BasicBullet BasicPrefab;
        //[SerializeField]
        //private ShotgunBullet ShotgunPrefab;
    }
}
