using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Manages each Powerup List contained within the main game scene.
    /// </summary>
    public class PowerupManager
    {
        private OnFireList OnFireList { get; set; }
        private OnHitList OnHitList { get; set; }
        private OnGetHitList OnGetHitList { get; set; }
        private OnLevelUpList OnLevelUpList { get; set; }

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

            OnFireList = new OnFireList(ids);
            Init(OnFireList);

            OnHitList = new OnHitList(ids);
            Init(OnHitList);

            OnGetHitList = new OnGetHitList(ids);
            Init(OnGetHitList);

            OnLevelUpList = new OnLevelUpList(ids);
            Init(OnLevelUpList);
        }

        public void OnFire(Vector2 firePosition, Bullet[] bullets)
        {
            foreach (var powerup in OnFireList)
                powerup.OnFire(firePosition, bullets);
        }

        public void OnHit(Enemy enemy)
        {
            foreach (var powerup in OnHitList)
                powerup.OnHit(enemy);
        }

        public void OnGetHit()
        {
            foreach (var powerup in OnGetHitList)
                powerup.OnGetHit();
        }
    }
}
