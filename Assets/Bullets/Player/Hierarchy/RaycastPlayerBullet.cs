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
        protected override bool AutomaticallyDeactivate => false;

        private Vector3 _representedVelocity { get; set; }
        public override Vector2 RepresentedVelocity => _representedVelocity;

        #region Prefabs

        [SerializeField]
        public LineRenderer _Line = null;

        [SerializeField]
        private float _FadeInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FadeOutTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _StartWidth = GameConstants.PrefabNumber;

        [SerializeField]
        private float _EndWidth = GameConstants.PrefabNumber;

        [SerializeField]
        private float _RepresentedVelocitySpeed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public LineRenderer Line => _Line;

        public float FadeInTime => _FadeInTime;
        public float FadeOutTime => _FadeOutTime;

        private float StartWidth => _StartWidth;
        private float EndWidth => _EndWidth;

        public float RepresentedVelocitySpeed => _RepresentedVelocitySpeed;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new LineRendererColorHandler(Line);

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
        protected sealed override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
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
            var endDirection = MathUtil.VectorAtRadianAngle(angle, RaycastDistance * distanceScale);
            if (GameUtil.RaycastTryGetEnemy(transform.position, endDirection, out Enemy enemy, out RaycastHit2D? hit))
            {
                EndPoint = hit.Value.point;

                _representedVelocity = endDirection.normalized * RepresentedVelocitySpeed;
                enemy.CollideWithBullet(this);
            }
            else
                EndPoint = (Vector3)endDirection + transform.position;
        }
    }
}