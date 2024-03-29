﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// Represents a bullet that can bounce a finite number of times
    /// before deactivating.
    /// </summary>
    /// <inheritdoc/>
    public abstract class BouncingBullet : PlayerBullet
    {
        public override int Damage => CurrentDamage;
        protected override bool AutomaticallyDeactivate => false;
        protected override bool ShouldMarkSelfCollision => false;

        #region Prefabs

        [SerializeField]
        private int _DamageAfterBounce = GameConstants.PrefabNumber;

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private int DamageAfterBounce => _DamageAfterBounce;
        public float Speed => _Speed;

        #endregion Prefab Properties


        protected virtual Vector2 InitialVelocity => new Vector2(0, Speed);

        public int BouncesLeft { get; set; }
        protected int CurrentDamage { get; set; }

        protected virtual void OnBouncingBulletInit() { }
        protected sealed override void OnPlayerBulletInit()
        {
            OnBouncingBulletInit();
        }

        protected virtual void OnBouncingBulletActivate() { }
        protected sealed override void OnActivate()
        {
            CurrentDamage = BaseDamage;
            Velocity = InitialVelocity;
            OnBouncingBulletActivate();
        }

        protected virtual void OnBouncingBulletSpawn() { }
        protected override void OnBulletSpawn()
        {
            InitializeNumberBounces();
            OnBouncingBulletSpawn();
        }

        protected virtual void OnBouncingBulletDeactivate() { }
        protected sealed override void OnBulletDeactivate()
        {
            OnBouncingBulletDeactivate();
        }

        protected abstract void OnBounce(Enemy enemy);
        protected sealed override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            if (BouncesLeft > 0)
            {
                ApplyBounceDebuff();
                OnBounce(enemy);
            }
            else
                DeactivateSelf();
        }

        protected virtual void ApplyBounceDebuff()
        {
            BouncesLeft--;
            CurrentDamage = DamageAfterBounce;
        }

        protected virtual void InitializeNumberBounces()
        {
            BouncesLeft = 1 + GameUtil.PlusOneIfMaxWeaponLevel(BulletLevel);
        }
    }
}
