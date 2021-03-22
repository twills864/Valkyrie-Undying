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
        public override int ReflectedDamage => 0;
        public override bool CanReflect => false;
        protected override bool ShouldDeactivateOnDestructor => false;


        [SerializeField]
        private float FadeInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float PrefireFullBrightTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float FullBrightTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float FadeTime = GameConstants.PrefabNumber;

        [HideInInspector]
        public Color PrefireColor;
        private Color LaserColor;

        private Sequence Sequence { get; set; }

        public float WidthHalf { get; private set; }

        public bool LaserActivated { get; private set; }
        public Vector3 SpawnPoint;

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

            Sequence = new Sequence(fadeIn, prefireDelay, activateCollider, postfireDelay, fadeTo, deactivate);
        }

        protected override void OnActivate()
        {
            LaserActivated = false;

            Color spawnColor = PrefireColor;
            spawnColor.a = 0;
            SpriteColor = spawnColor;

            Sequence.ResetSelf();
        }

        protected override void OnFrameRun(float deltaTime)
        {
            Sequence.RunFrame(deltaTime);
        }

        private void ActivateCollider()
        {
            LaserActivated = true;
            SpriteColor = LaserColor;

            Vector2 direction = transform.position - SpawnPoint;
            if(GameUtil.RaycastHitsPlayer(SpawnPoint, direction, out RaycastHit2D hit))
                Player.Instance.CollideWithBullet(this);
        }
    }
}