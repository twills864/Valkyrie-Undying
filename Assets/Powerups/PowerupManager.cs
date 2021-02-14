using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;
using Assets.Util;

namespace Assets.Powerups
{
    // Despite the name, more closely resembles PoolList than PoolManager
    // Or does it?
    public class PowerupManager
    {
        private OnFireList OnFire { get; set; }
        private OnGetHitList OnGetHit { get; set; }
        private OnHitList OnHit { get; set; }
        private OnLevelUpList OnLevelUp { get; set; }

        private IPowerupList[] AllLists { get; set; }

        public void Init()
        {
            AllLists = new IPowerupList[4];

            UniqueIdGenerator ids = new UniqueIdGenerator(0);

            void Init(IPowerupList list)
            {
                list.Init();
                AllLists[list.PowerupManagerIndex] = list;
            }

            OnFire = new OnFireList(ids);
            Init(OnFire);

            OnGetHit = new OnGetHitList(ids);
            Init(OnGetHit);

            OnHit = new OnHitList(ids);
            Init(OnHit);

            OnLevelUp = new OnLevelUpList(ids);
            Init(OnLevelUp);
        }

        public void OnEnemyHit(Enemy enemy)
        {
            foreach(var onHit in OnHit)
                onHit.OnHit(enemy);
        }
    }
}
