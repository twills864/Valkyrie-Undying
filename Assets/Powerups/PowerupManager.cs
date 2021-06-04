using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Powerups.Balance;
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
        private PassivePowerupList PassivePowerupList { get; set; }

        private OnDefaultWeaponFireList OnDefaultWeaponFireList { get; set; }
        private OnDefaultWeaponHitList OnDefaultWeaponHitList { get; set; }
        private OnDefaultWeaponKillList OnDefaultWeaponKillList { get; set; }
        private OnDefaultWeaponLevelUpList OnDefaultWeaponLevelUpList { get; set; }

        private IPowerupList[] AllLists { get; set; }

        public List<Powerup> AllPowerups { get; private set; }
        public Dictionary<Type, Powerup> AllPowerupsMap { get; private set; }

        public void Init(in PowerupBalanceManager balance, Destructor destructor)
        {
            const int NumLists = 10;
            AllLists = new IPowerupList[NumLists];
            AllPowerupsMap = new Dictionary<Type, Powerup>();

            UniqueIdGenerator ids = new UniqueIdGenerator(0);

            InitBasicPowerups(in balance, ref ids);
            InitDefaultWeaponPowerups(in balance, ref ids);

            AllPowerups = AllPowerupsMap.Values.ToList();

            //OnHitList.Get<ShrapnelPowerup>().MaxY = destructor.SizeHalf.y;
            OnFireList.Get<PestControlPowerup>().Init();
        }

        private void InitBasicPowerups(in PowerupBalanceManager balance, ref UniqueIdGenerator ids)
        {
            OnFireList = new OnFireList(ids);
            InitList(OnFireList, in balance);

            OnHitList = new OnHitList(ids);
            InitList(OnHitList, in balance);

            OnKillList = new OnKillList(ids);
            InitList(OnKillList, in balance);

            OnGetHitList = new OnGetHitList(ids);
            InitList(OnGetHitList, in balance);

            OnLevelUpList = new OnLevelUpList(ids);
            InitList(OnLevelUpList, in balance);

            PassivePowerupList = new PassivePowerupList(ids);
            InitList(PassivePowerupList, in balance);
        }

        private void InitDefaultWeaponPowerups(in PowerupBalanceManager balance, ref UniqueIdGenerator ids)
        {
            OnDefaultWeaponFireList = new OnDefaultWeaponFireList(ids);
            InitList(OnDefaultWeaponFireList, in balance);

            OnDefaultWeaponHitList = new OnDefaultWeaponHitList(ids);
            InitList(OnDefaultWeaponHitList, in balance);

            OnDefaultWeaponKillList = new OnDefaultWeaponKillList(ids);
            InitList(OnDefaultWeaponKillList, in balance);

            OnDefaultWeaponLevelUpList = new OnDefaultWeaponLevelUpList(ids);
            InitList(OnDefaultWeaponLevelUpList, in balance);
        }

        private void InitList(IPowerupList list, in PowerupBalanceManager balance)
        {
            list.Init(AllPowerupsMap, in balance);
            AllLists[list.PowerupManagerIndex] = list;
        }

        #region Basic Powerups

        public void OnFire(Vector3 firePosition, PlayerBullet[] bullets)
        {
            foreach (var powerup in OnFireList.Where(x => x.IsActive))
                powerup.OnFire(firePosition, bullets);
        }

        public void OnHit(Enemy enemy, PlayerBullet bullet, Vector3 hitPosition)
        {
            foreach (var powerup in OnHitList.Where(x => x.IsActive))
                powerup.OnHit(enemy, bullet, hitPosition);
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

        public void PassiveUpdate(float deltaTime, float realDeltaTime)
        {
            foreach (var powerup in PassivePowerupList.Where(x => x.IsActive))
                powerup.RunFrame(deltaTime, realDeltaTime);
        }

        #endregion Basic Powerups


        #region Default Weapon Powerups

        public void OnDefaultWeaponFire(Vector3 firePosition, DefaultBullet[] bullets)
        {
            foreach (var powerup in OnDefaultWeaponFireList.Where(x => x.IsActive))
                powerup.OnFire(firePosition, bullets);
        }

        public void OnDefaultWeaponHit(Enemy enemy, DefaultBullet bullet, Vector3 hitPosition)
        {
            foreach (var powerup in OnDefaultWeaponHitList.Where(x => x.IsActive))
                powerup.OnHit(enemy, bullet, hitPosition);
        }

        public void OnDefaultWeaponKill(Enemy enemy, DefaultBullet bullet)
        {
            foreach (var powerup in OnDefaultWeaponKillList.Where(x => x.IsActive))
                powerup.OnKill(enemy, bullet);
        }

        #endregion Default Weapon Powerups


        #region Get Specific Powerups

        #region Get Specific Basic Powerup

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

        public TPowerup GetPassivePowerup<TPowerup>() where TPowerup : PassivePowerup
        {
            var ret = PassivePowerupList.Get<TPowerup>();
            return ret;
        }

        #endregion Get Specific Basic Powerup


        #region Get Specific Default Weapon Powerup

        public TPowerup GetDefaulWeaponFirePowerup<TPowerup>() where TPowerup : OnDefaultWeaponFirePowerup
        {
            var ret = OnDefaultWeaponFireList.Get<TPowerup>();
            return ret;
        }

        public TPowerup GetOnDefaultWeaponHitPowerup<TPowerup>() where TPowerup : OnDefaultWeaponHitPowerup
        {
            var ret = OnDefaultWeaponHitList.Get<TPowerup>();
            return ret;
        }

        public TPowerup GetOnDefaultWeaponKillPowerup<TPowerup>() where TPowerup : OnDefaultWeaponKillPowerup
        {
            var ret = OnDefaultWeaponKillList.Get<TPowerup>();
            return ret;
        }

        public TPowerup GetOnDefaultWeaponLevelUpPowerup<TPowerup>() where TPowerup : OnDefaultWeaponLevelUpPowerup
        {
            var ret = OnDefaultWeaponLevelUpList.Get<TPowerup>();
            return ret;
        }

        #endregion Get Specific Default Weapon Powerup

        #endregion GetSpecificPowerups
    }
}
