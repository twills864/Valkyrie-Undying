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
        public LineRenderer Line;

        [SerializeField]
        public float AlphaDecayTime;

        /// <summary>
        /// Whether or not this bullet can currently damage enemies.
        /// </summary>
        public bool CollisionActive { get; protected set; }

        public Vector3 StartPoint
        {
            get => Line.GetPosition(0);
            set
            {
                value.z = 10;
                Line.SetPosition(0, value);
            }
        }
        public Vector3 EndPoint
        {
            get => Line.GetPosition(1);
            set
            {
                value.z = 10;
                Line.SetPosition(1, value);
            }
        }

        public virtual float MaxAlpha => 1.0f;
        public virtual float RaycastDistance => SpaceUtil.WorldMapSize.y;

        private FloatValueOverTime AlphaValue { get; set; }

        protected override ColorHandler DefaultColorHandler()
            => new LineRendererColorHandler(Line);

        // Collisions will be handled manually.
        public sealed override bool CollidesWithEnemy(Enemy enemy) => false;

        protected sealed override void OnPlayerBulletInit()
        {
            AlphaValue = new FloatValueOverTime(MaxAlpha, 0.0f, AlphaDecayTime);
        }

        protected virtual void OnPlayerRaycastBulletActivate() { }
        protected sealed override void OnActivate()
        {
            CollisionActive = true;
            AlphaValue.Reset();
        }

        public sealed override void OnSpawn()
        {
            StartPoint = transform.position;
            Alpha = 1.0f;
        }

        protected virtual void OnRaycastPlayerBulletFrameRun(float deltaTime) { }
        protected sealed override void OnPlayerBulletFrameRun(float deltaTime)
        {
            AlphaValue.Increment(deltaTime);
            if (!AlphaValue.IsFinished)
            {
                Alpha = AlphaValue.Value;
                OnRaycastPlayerBulletFrameRun(deltaTime);
            }
            else
                DeactivateSelf();
        }




        public void RayCastUp(float angleOffset = 0)
        {
            const float angleUp = 90f * Mathf.Deg2Rad;
            RayCast(angleUp + angleOffset);
        }

        public void RayCast(float angle)
        {
            var endPoint = MathUtil.VectorAtRadianAngle(angle, RaycastDistance);
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