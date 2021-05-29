using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class SentinelProjectileBullet : PlayerBullet
    {
        public override int Damage => SentinelProjectileDamage;

        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;

        #endregion Prefab Properties


        public int SentinelProjectileDamage { get; set; }

        public void CloneFromSentinel(SentinelBullet sentinel)
        {
            SpriteColor = sentinel.SpriteColor;
            transform.localScale = sentinel.transform.localScale;
        }

        protected override void OnPlayerBulletInit()
        {

        }

        protected override void OnActivate()
        {

        }

        public override void OnSpawn()
        {

        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {

        }
    }
}