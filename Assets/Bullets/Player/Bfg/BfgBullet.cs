using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class BfgBullet : PlayerBullet
    {
        private const float FullBright = 1.0f;

        public override int Damage => base.Damage + (BulletLevel * DamagePerLevel);

        public override Vector3 GetHitPosition(Enemy enemy) => GetClosestPoint(enemy);

        public override AudioClip FireSound =>
            //SoundBank.LaserDramatic;
            //SoundBank.Cannon2;
            SoundBank.ExplosionLongDeep;

        public override Vector2 RepresentedVelocity => new Vector2(0, 40f);

        #region Prefabs

        [SerializeField]
        private int _DamagePerLevel = GameConstants.PrefabNumber;


        [SerializeField]
        private float _InitialScaleX = GameConstants.PrefabNumber;

        [SerializeField]
        private float _ScaleXPerLevel = GameConstants.PrefabNumber;


        [SerializeField]
        private float _FullBrightTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private int DamagePerLevel => _DamagePerLevel;

        private float InitialScaleX => _InitialScaleX;
        private float ScaleXPerLevel => _ScaleXPerLevel;

        private float FullBrightTime => _FullBrightTime;
        private float FadeTime => _FadeTime;

        #endregion Prefab Properties


        private Sequence Behavior { get; set; }

        private bool HitBoxActive { get; set; }

        public void InitSpawner() => BfgBulletSpawner.StaticInitScale(InitialScaleX, ScaleXPerLevel);
        public void InitFallout() => BfgBulletFallout.StaticInitFadeInfo(FullBrightTime, FadeTime);

        protected override void OnPlayerBulletInit()
        {
            float worldHeight = SpaceUtil.WorldMap.Height;

            var spriteHeight = Sprite.size.y;
            var heightScale = worldHeight / spriteHeight;
            transform.localScale = new Vector3(1, heightScale, 1);

            var delay = new Delay(this, FullBrightTime);
            var removeActive = new GameTaskFunc(this, () => HitBoxActive = false);
            var fadeTo = new FadeTo(this, 0.0f, FadeTime);
            var deactivate = GameTaskFunc.DeactivateSelf(this);

            Behavior = new Sequence(delay, removeActive, fadeTo, deactivate);
        }

        protected override void OnActivate()
        {
            HitBoxActive = true;
            Alpha = FullBright;
            Behavior.ResetSelf();

            BfgBulletFallout.ActivateInstance();
        }

        public override void OnSpawn()
        {
            float y = transform.position.y + SpaceUtil.WorldMap.HeightHalf;
            PositionY = y;

            float x = InitialScaleX + (BulletLevel * ScaleXPerLevel);
            LocalScaleX = x;
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            Behavior.RunFrame(deltaTime);
        }

        public override bool CollidesWithEnemy(Enemy enemy) => HitBoxActive;

        // Disable deactivating
        public override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
        }

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if (IsMaxLevel)
            {
                if (CollisionUtil.IsEnemyBullet(collision))
                {
                    var bullet = collision.GetComponent<EnemyBullet>();
                    bullet.DeactivateSelf();
                }
            }
        }
    }
}