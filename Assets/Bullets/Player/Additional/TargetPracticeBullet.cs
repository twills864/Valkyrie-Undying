using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.Powerups;
using Assets.Enemies;

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

        [SerializeField]
        private int _NumberParasites = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;
        private int NumberParasites => _NumberParasites;

        #endregion Prefab Properties


        public int TargetPracticeDamage { get; set; }

        protected override void OnBulletSpawn()
        {
            Vector3 positionOffset = (Velocity / Speed) * TargetPracticeTarget.Radius;
            transform.position += positionOffset;
        }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            enemy.AddParasites(NumberParasites);
        }
    }
}