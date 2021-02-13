using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerup
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

            OnFire = new OnFireList(ids);
            OnFire.Init();
            AllLists[OnFire.PowerupManagerIndex] = OnFire;

            OnGetHit = new OnGetHitList(ids);
            OnGetHit.Init();
            AllLists[OnGetHit.PowerupManagerIndex] = OnGetHit;

            OnHit = new OnHitList(ids);
            OnHit.Init();
            AllLists[OnHit.PowerupManagerIndex] = OnHit;

            OnLevelUp = new OnLevelUpList(ids);
            OnLevelUp.Init();
            AllLists[OnLevelUp.PowerupManagerIndex] = OnLevelUp;
        }

        //public void LevelUp(Type type)
        //{
        //    type.get
        //}
    }
}
