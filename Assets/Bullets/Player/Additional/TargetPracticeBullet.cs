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
    public class TargetPracticeBullet : PlayerBullet
    {
        public override AudioClip FireSound => SoundBank.LaserGeneric;

        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;

        #endregion Prefab Properties


        public int TargetPracticeDamage { get; set; }

        protected override void OnPlayerBulletInit()
        {

        }

        protected override void OnActivate()
        {

        }

        protected override void OnBulletSpawn()
        {

        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {

        }
    }
}