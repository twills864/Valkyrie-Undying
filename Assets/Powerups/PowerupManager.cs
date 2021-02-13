using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
{
    // Despite the name, more closely resembles PoolList than PoolManager
    // Or does it?
    public class PowerupManager
    {
        public OnFireList OnFire { get; set; }
        public OnGetHitList OnGetHit { get; set; }
        public OnHitList OnHit { get; set; }
        public OnLevelUpList OnLevelUp { get; set; }

        public IPowerupList[] AllLists { get; set; }

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

        //public void LevelUp(Type type)
        //{
        //    type.get
        //}
    }
}
