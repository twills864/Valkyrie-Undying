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

        [SerializeField]
        private float FullBrightTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float FadeTime = GameConstants.PrefabNumber;

        private SequenceGameTask Sequence { get; set; }

        private bool HitBoxActive;


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

            Sequence = new SequenceGameTask(this, delay, removeActive, fadeTo, deactivate);
        }

        protected override void OnActivate()
        {
            HitBoxActive = true;
            Alpha = FullBright;
            Sequence.ResetSelf();
        }

        public override void OnSpawn()
        {
            float y = transform.position.y + SpaceUtil.WorldMap.HeightHalf;
            PositionY = y;

            float x = 0.1f + (BulletLevel * 0.1f);
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