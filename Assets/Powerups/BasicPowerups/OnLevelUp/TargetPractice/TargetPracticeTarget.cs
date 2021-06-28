using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.UnityPrefabStructs;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    public class TargetPracticeTarget : ValkyrieSprite
    {
        public override TimeScaleType TimeScale => TimeScaleType.Player;

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _TravelAlpha = GameConstants.PrefabNumber;

        [SerializeField]
        private float _TravelSizeScale = GameConstants.PrefabNumber;

        [SerializeField]
        private float _TravelSpeed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;

        private float FadeTime => _FadeTime;
        private float TravelAlpha => _TravelAlpha;
        private float TravelSizeScale => _TravelSizeScale;
        private float TravelSpeed => _TravelSpeed;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler() => new SpriteColorHandler(Sprite);


        private bool CanCollide { get; set; }
        private Vector2 SpawnMin { get; set; }
        private Vector2 SpawnMax { get; set; }

        private MoveTo TravelMove { get; set; }
        private EaseInOut TravelMoveEase { get; set; }
        private Sequence TravelSequence { get; set; }

        protected override void OnInit()
        {
            var initialSize = GetComponent<Renderer>().bounds.size;
            SpawnMin = new Vector2(SpaceUtil.WorldMap.Left.x + initialSize.x, SpaceUtil.WorldMap.Center.y);
            SpawnMax = new Vector2(SpaceUtil.WorldMap.Right.x - initialSize.x, SpaceUtil.WorldMap.Top.y - initialSize.y);

            var activateCanCollide = new GameTaskFunc(this, () => CanCollide = true);

            var entranceFadeIn = new FadeTo(this, 0f, 1f, FadeTime);
            var entranceScaleIn = new ScaleTo(this, float.Epsilon, LocalScale, FadeTime);
            var entranceSequence = new Sequence(entranceFadeIn, entranceScaleIn, activateCanCollide);

            const float FullBrightAlpha = 1.0f;
            var fadeOut = new FadeTo(this, FullBrightAlpha, TravelAlpha, FadeTime);
            var scaleOut = new ScaleTo(this, LocalScale, TravelSizeScale, FadeTime);
            var fadeOutConcurrence = new ConcurrentGameTask(fadeOut, scaleOut);

            TravelMove = MoveTo.Default(this, 1.0f);
            TravelMoveEase = new EaseInOut(TravelMove);

            var fadeIn = new FadeTo(this, TravelAlpha, FullBrightAlpha, FadeTime);
            var scaleIn = new ScaleTo(this, TravelSizeScale, LocalScale, FadeTime);
            var fadeInConcurrence = new ConcurrentGameTask(fadeIn, scaleIn);

            TravelSequence = new Sequence(fadeOutConcurrence, TravelMoveEase, fadeInConcurrence, activateCanCollide);

            CanCollide = false;
            LocalScale = float.Epsilon;
            transform.position = SpaceUtil.WorldMap.Center;
            RunTask(entranceSequence);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CanCollide && CollisionUtil.IsPlayerBullet(collision))
            {
                CanCollide = false;
                ExplodeBullets();
                ResetTravelSequence();
                RunTask(TravelSequence);
            }
        }

        private void ResetTravelSequence()
        {
            TravelSequence.ResetSelf();

            float newX = RandomUtil.Float(SpawnMin.x, SpawnMax.x);
            float newY = RandomUtil.Float(SpawnMin.y, SpawnMax.y);
            Vector3 newPosition = new Vector3(newX, newY, PositionZ);

            float distance = Vector2.Distance(transform.position, newPosition) + float.Epsilon;
            float duration = distance / TravelSpeed;
            TravelMoveEase.Duration = duration;
            TravelMove.ReinitializeMove(transform.position, newPosition);
            TravelSequence.RecalculateDuration();
        }

        private void ExplodeBullets()
        {
            // 4 diagonal bullets + 3 cardinal
            const int NumBullets = 7;
            var bullets = PoolManager.Instance.BulletPool.GetMany<TargetPracticeBullet>(NumBullets);

            float speed = bullets[0].Speed;
            float diagonalSpeed = speed / MathUtil.Sqrt2;

            int bulletsSpawned = 0;

            TargetPracticeBullet NextBullet()
            {
                TargetPracticeBullet bullet = bullets[bulletsSpawned];
                bulletsSpawned++;
                return bullet;
            }

            void SetVelocity(TargetPracticeBullet bullet, float velocityX, float velocityY)
            {
                Vector2 velocity = new Vector2(velocityX, velocityY);
                bullet.Velocity = velocity;

                const float PositionClamp = 0.1f;
                Vector2 positionOffset = velocity * PositionClamp;
                bullet.transform.position = transform.position + (Vector3)positionOffset;
            }

            // Spawn diagonals
            for (int x = -1; x <= 1; x += 2)
            {
                for (int y = -1; y <= 1; y += 2)
                    SetVelocity(NextBullet(), diagonalSpeed * x, diagonalSpeed * y);
            }

            var left = NextBullet();
            SetVelocity(left, -speed, 0f);

            var right = NextBullet();
            SetVelocity(right, speed, 0f);

            var up = NextBullet();
            SetVelocity(up, 0f, speed);
        }
    }
}
