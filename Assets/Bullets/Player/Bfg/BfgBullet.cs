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

        [SerializeField]
        private int DamagePerLevel = GameConstants.PrefabNumber;

        [SerializeField]
        private float InitialScaleX = GameConstants.PrefabNumber;

        [SerializeField]
        private float ScaleXPerLevel = GameConstants.PrefabNumber;

        [SerializeField]
        private float FullBrightTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float FadeTime = GameConstants.PrefabNumber;

        private Sequence Sequence { get; set; }

        private bool HitBoxActive;

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

            Sequence = new Sequence(delay, removeActive, fadeTo, deactivate);
        }

        protected override void OnActivate()
        {
            HitBoxActive = true;
            Alpha = FullBright;
            Sequence.ResetSelf();

            BfgBulletFallout.ActivateInstance();
        }

        public override void OnSpawn()
        {
            float y = transform.position.y + SpaceUtil.WorldMap.HeightHalf;
            PositionY = y;

            float x = InitialScaleX + (BulletLevel * ScaleXPerLevel);
            LocalScaleX = x;
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            Sequence.RunFrame(deltaTime);
        }

        public override bool CollidesWithEnemy(Enemy enemy) => HitBoxActive;

        // Disable deactivating
        public override void OnCollideWithEnemy(Enemy enemy)
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