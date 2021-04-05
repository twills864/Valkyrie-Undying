using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public abstract class RaycastPlayerBullet : PlayerBullet
    {
        [SerializeField]
        public float FadeInTime = GameConstants.PrefabNumber;

        [SerializeField]
        public float FadeOutTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float StartWidth = GameConstants.PrefabNumber;

        [SerializeField]
        private float EndWidth = GameConstants.PrefabNumber;

        [SerializeField]
        public LineRenderer Line = null;

        protected Vector3 StartPoint
        {
            get => Line.GetPosition(0);
            private set
            {
                value.z = 10;
                Line.SetPosition(0, value);
            }
        }
        protected Vector3 EndPoint
        {
            get => Line.GetPosition(1);
            private set
            {
                value.z = 10;
                Line.SetPosition(1, value);
            }
        }

        public virtual float MaxAlpha => 1.0f;
        public virtual float RaycastDistance => SpaceUtil.WorldMapSize.y;

        private FloatValueOverTime FadeInAlphaValue { get; set; }
        private FloatValueOverTime FadeOutAlphaValue { get; set; }

        protected override ColorHandler DefaultColorHandler()
            => new LineRendererColorHandler(Line);

        // Collisions will be handled manually.
        public sealed override bool CollidesWithEnemy(Enemy enemy) => false;

        protected sealed override void OnPlayerBulletInit()
        {
            FadeInAlphaValue = new FloatValueOverTime(0.0f, MaxAlpha, FadeInTime);
            FadeOutAlphaValue = new FloatValueOverTime(MaxAlpha, 0.0f, FadeOutTime);

            Line.startWidth = StartWidth;
            Line.endWidth = EndWidth;
        }

        protected virtual void OnPlayerRaycastBulletActivate() { }
        protected sealed override void OnActivate()
        {
            FadeInAlphaValue.Timer.Reset();
            FadeOutAlphaValue.Timer.Reset();
            OnPlayerRaycastBulletActivate();
        }

        protected virtual void OnPlayerRaycastBulletSpawn() { }
        public sealed override void OnSpawn()
        {
            StartPoint = transform.position;
            Alpha = MaxAlpha;
            OnPlayerRaycastBulletSpawn();
        }

        protected virtual void OnRaycastPlayerBulletFrameRun(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if(FadeInAlphaValue.IsFinished)
                FadeOutFrameRun(deltaTime, realDeltaTime);
            else
            {
                FadeInAlphaValue.Increment(deltaTime);
                if(!FadeInAlphaValue.IsFinished)
                {
                    Alpha = FadeInAlphaValue.Value;
                    OnRaycastPlayerBulletFrameRun(deltaTime, realDeltaTime);
                }
                else
                {
                    float overflow = FadeInAlphaValue.Timer.OverflowDeltaTime;
                    FadeOutFrameRun(deltaTime, realDeltaTime);
                }
            }
        }

        private void FadeOutFrameRun(float deltaTime, float realDeltaTime)
        {
            FadeOutAlphaValue.Increment(deltaTime);
            if (!FadeOutAlphaValue.IsFinished)
            {
                Alpha = FadeOutAlphaValue.Value;
                OnRaycastPlayerBulletFrameRun(deltaTime, realDeltaTime);
            }
            else
                DeactivateSelf();
        }

        protected virtual void OnRaycastPlayerBulletCollideWithEnemy(Enemy enemy) { }
        // Don't deactivate self - fadeout begins automatically.
        public sealed override void OnCollideWithEnemy(Enemy enemy)
        {
            OnRaycastPlayerBulletCollideWithEnemy(enemy);
        }

        public void RayCastUp(float angleOffset = 0)
        {
            const float angleUp = 90f * Mathf.Deg2Rad;
            RayCast(angleUp + angleOffset);
        }

        public void RayCast(float angle, float distanceScale = 1.0f)
        {
            var endPoint = MathUtil.VectorAtRadianAngle(angle, RaycastDistance * distanceScale);
            if (GameUtil.RaycastTryGetEnemy(transform.position, endPoint, out Enemy enemy, out RaycastHit2D? hit))
            {
                EndPoint = hit.Value.point;
                enemy.CollideWithBullet(this);
            }
            else
                EndPoint = (Vector3)endPoint + transform.position;
        }
    }
}