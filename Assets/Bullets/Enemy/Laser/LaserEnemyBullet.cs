using System;
using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public class LaserEnemyBullet : EnemyBullet
    {
        public override bool DeactivateOnHit => false;
        public override bool CanReflect => false;
        protected override bool ShouldDeactivateOnDestructor => false;

        public override float TimeScaleModifier => Parent?.TimeScaleModifier ?? base.TimeScaleModifier;

        #region Prefabs

        [SerializeField]
        private float _FadeInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _PrefireFullBrightTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FullBrightTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        // Cannot have [NonSerialized] tag due to Unity needing to keep this value for
        // future instantiated copies.
        //
        // Unity instantiation mechanisms prevent us from turning this into a property
        // with a proper getter and setter.
        [HideInInspector]
        public Color PrefireColor;
        #endregion Prefabs


        #region Prefab Properties

        private float FadeInTime => _FadeInTime;
        private float PrefireFullBrightTime => _PrefireFullBrightTime;
        private float FullBrightTime => _FullBrightTime;
        private float FadeTime => _FadeTime;

        #endregion Prefab Properties

        public LaserEnemy Parent { get; set; }

        public Vector3 SpawnPoint { get; set; }
        public float WidthHalf { get; private set; }

        private Color LaserColor { get; set; }
        public bool LaserActivated { get; private set; }

        private Sequence Behavior { get; set; }

        protected override void OnEnemyBulletInit()
        {
            LaserColor = SpriteColor;

            float worldHeight = SpaceUtil.WorldMap.Height;
            LocalScaleX = worldHeight * MathUtil.Sqrt2;

            var sprite = GetComponent<SpriteRenderer>();
            WidthHalf = sprite.size.x * 0.5f * transform.localScale.x;

            var fadeIn = new FadeTo(this, 0f, PrefireColor.a, FadeInTime);
            var prefireDelay = new Delay(this, PrefireFullBrightTime);
            var activateCollider = new GameTaskFunc(this, ActivateCollider);
            var postfireDelay = new Delay(this, FullBrightTime);
            var fadeTo = new FadeTo(this, 0.0f, FadeTime);
            var deactivate = GameTaskFunc.DeactivateSelf(this);

            Behavior = new Sequence(fadeIn, prefireDelay, activateCollider, postfireDelay, fadeTo, deactivate);
        }

        protected override void OnEnemyBulletActivate()
        {
            LaserActivated = false;

            Color spawnColor = PrefireColor;
            spawnColor.a = 0;
            SpriteColor = spawnColor;

            Parent = null;

            Behavior.ResetSelf();
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            Behavior.RunFrame(deltaTime);
        }

        protected override void OnDeactivate()
        {
            Parent = null;
        }

        private void ActivateCollider()
        {
            LaserActivated = true;
            SpriteColor = LaserColor;

            Vector2 direction = transform.position - SpawnPoint;
            if(GameUtil.RaycastHitsPlayer(SpawnPoint, direction, out RaycastHit2D hit))
                Player.Instance.CollidesWithBullet(this);
        }
    }
}