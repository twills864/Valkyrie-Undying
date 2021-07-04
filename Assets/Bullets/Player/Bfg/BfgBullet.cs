using System;
using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet fired after the Battle-Frenzied Guillotine is done charging its pre-fire shot.
    /// It collides with an unlimited number of enemies for a brief period of time
    /// after its activation, before fading out and eventually despawning.
    /// </summary>
    /// <inheritdoc/>
    public class BfgBullet : PlayerBullet
    {
        private const float FullBright = 1.0f;

        public override int Damage => base.Damage + (BulletLevel * DamagePerLevel);
        protected override bool AutomaticallyDeactivate => false;

        public override Vector3 GetHitPosition(Enemy enemy) => base.GetClosestPointOnBullet(enemy);

        public override AudioClip FireSound =>
            //SoundBank.LaserDramatic;
            //SoundBank.Cannon2;
            SoundBank.ExplosionLongDeep;

        public override Vector2 RepresentedVelocity => new Vector2(0, RepresentedVelocityY);

        #region Prefabs

        [SerializeField]
        private int _DamagePerLevel = GameConstants.PrefabNumber;

        #region FireDamage

        [Serializable]
        private struct BfgFireDamage
        {
            public int CollisionDamage;
            public int MaxDamageBase;
            public int MaxDamageIncrease;

            public int CalculateMaxDamage(int level)
            {
                int damage = MaxDamageBase + (level * MaxDamageIncrease);
                return damage;
            }
        }

        #endregion FireDamage

        [SerializeField]
        private BfgFireDamage _FireDamage = default;


        [SerializeField]
        private float _InitialScaleX = GameConstants.PrefabNumber;

        [SerializeField]
        private float _ScaleXPerLevel = GameConstants.PrefabNumber;


        [SerializeField]
        private float _FullBrightTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _RepresentedVelocityY = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private int DamagePerLevel => _DamagePerLevel;
        private int FireCollisionDamage => _FireDamage.CollisionDamage;
        private int MaxFireDamage => _FireDamage.CalculateMaxDamage(BulletLevel);

        private float InitialScaleX => _InitialScaleX;
        private float ScaleXPerLevel => _ScaleXPerLevel;

        private float FullBrightTime => _FullBrightTime;
        private float FadeTime => _FadeTime;

        public float RepresentedVelocityY => _RepresentedVelocityY;

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

        protected override void OnBulletSpawn()
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

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            const int FireDamageIncrease = 1;
            enemy.Ignite(FireCollisionDamage, FireDamageIncrease, MaxFireDamage);
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