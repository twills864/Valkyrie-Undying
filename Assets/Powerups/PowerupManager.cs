using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets;
using Assets.Bullets.PlayerBullets;
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
        private OnKillList OnKillList { get; set; }
        private OnGetHitList OnGetHitList { get; set; }
        private OnLevelUpList OnLevelUpList { get; set; }

        private IPowerupList[] AllLists { get; set; }

        public Dictionary<Type, Powerup> AllPowerups { get; set; }

        public void Init()
        {
            AllLists = new IPowerupList[5];
            AllPowerups = new Dictionary<Type, Powerup>();

            UniqueIdGenerator ids = new UniqueIdGenerator(0);

            void Init(IPowerupList list)
            {
                list.Init(AllPowerups);
                AllLists[list.PowerupManagerIndex] = list;
            }

            OnFireList = new OnFireList(ids);
            Init(OnFireList);

            OnHitList = new OnHitList(ids);
            Init(OnHitList);

            OnKillList = new OnKillList(ids);
            Init(OnKillList);

            OnGetHitList = new OnGetHitList(ids);
            Init(OnGetHitList);

            OnLevelUpList = new OnLevelUpList(ids);
            Init(OnLevelUpList);
        }

        public void OnFire(Vector2 firePosition, PlayerBullet[] bullets)
        {
            foreach (var powerup in OnFireList.Where(x => x.IsActive))
                powerup.OnFire(firePosition, bullets);
        }

        public void OnHit(Enemy enemy, PlayerBullet bullet)
        {
            foreach (var powerup in OnHitList.Where(x => x.IsActive))
                powerup.OnHit(enemy, bullet);
        }

        public void OnKill(Enemy enemy, PlayerBullet bullet)
        {
            foreach (var powerup in OnKillList.Where(x => x.IsActive))
                powerup.OnKill(enemy, bullet);
        }

        public void OnGetHit()
        {
            foreach (var powerup in OnGetHitList.Where(x => x.IsActive))
                powerup.OnGetHit();
        }

        public TPowerup GetFirePowerup<TPowerup>() where TPowerup : OnFirePowerup
        {
            var ret = OnFireList.Get<TPowerup>();
            return ret;
        }

        public TPowerup GetOnHitPowerup<TPowerup>() where TPowerup : OnHitPowerup
        {
            var ret = OnHitList.Get<TPowerup>();
            return ret;
        }

        public TPowerup GetOnKillPowerup<TPowerup>() where TPowerup : OnKillPowerup
        {
            var ret = OnKillList.Get<TPowerup>();
            return ret;
        }

        public TPowerup GetOnGetHitPowerup<TPowerup>() where TPowerup : OnGetHitPowerup
        {
            var ret = OnGetHitList.Get<TPowerup>();
            return ret;
        }

        public TPowerup GetOnLevelUpPowerup<TPowerup>() where TPowerup : OnLevelUpPowerup
        {
            var ret = OnLevelUpList.Get<TPowerup>();
            return ret;
        }
    }
}
