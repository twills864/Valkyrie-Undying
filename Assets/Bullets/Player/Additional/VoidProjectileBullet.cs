using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.Enemies;
using Assets.Hierarchy.ColorHandlers;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet fired from the center of a Void bullet while it's active.
    /// </summary>
    /// <inheritdoc/>
    public class VoidProjectileBullet : PlayerBullet
    {
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


        protected override ColorHandler DefaultColorHandler() => new SpriteColorHandler(Sprite);

        public override float BulletTrailWidth => SpriteMap.Width;

        private CircleCollider2D Collider { get; set; }
        private FuncAfterDelay ActivateCollider { get; set; }
        private VelocityChange VelocityChange { get; set; }
        private ConcurrentGameTask EntranceAnimation { get; set; }

        protected override void OnPlayerBulletInit()
        {
            Collider = GetComponent<CircleCollider2D>();

            ActivateCollider = new FuncAfterDelay(this, () => Collider.enabled = true, EntranceAnimationTime);
            var fadeIn = new FadeTo(this, 0, Alpha, EntranceAnimationTime);
            VelocityChange = new VelocityChange(this, Vector2.zero, Vector2.zero, EntranceAnimationTime);
            var easeVelocityOut = new EaseOut(VelocityChange);
            EntranceAnimation = new ConcurrentGameTask(fadeIn, easeVelocityOut, ActivateCollider);
        }

        protected override void OnActivate()
        {
            Collider.enabled = false;
            EntranceAnimation.ResetSelf();

            Alpha = 0;

            VelocityChange.EndVelocity = RandomUtil.RandomDirectionVectorTopThreeQuarters(Speed);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            EntranceAnimation.RunFrame(deltaTime);
        }
    }
}