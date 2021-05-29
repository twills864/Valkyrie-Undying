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
    public class VoidProjectileBullet : PlayerBullet
    {
        //public override int Damage => VoidProjectileDamage;

        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _EntranceAnimationTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;
        public float EntranceAnimationTime => _EntranceAnimationTime;

        #endregion Prefab Properties


        private VelocityChange VelocityChange { get; set; }
        private ConcurrentGameTask EntranceAnimation { get; set; }


        //public int VoidProjectileDamage { get; set; }

        protected override void OnPlayerBulletInit()
        {
            var scaleIn = new ScaleTo(this, Vector3.zero, transform.localScale, EntranceAnimationTime);
            var fadeIn = new FadeTo(this, 0, Alpha, EntranceAnimationTime);
            VelocityChange = new VelocityChange(this, Vector2.zero, Vector2.zero, EntranceAnimationTime);
            var easeVelocityOut = new EaseOut(VelocityChange);
            EntranceAnimation = new ConcurrentGameTask(scaleIn, fadeIn, easeVelocityOut);
        }

        protected override void OnActivate()
        {
            EntranceAnimation.ResetSelf();

            transform.localScale = Vector3.zero;
            Alpha = 0;

            VelocityChange.EndVelocity = RandomUtil.RandomDirectionVectorTopThreeQuarters(Speed);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            EntranceAnimation.RunFrame(deltaTime);
        }
    }
}