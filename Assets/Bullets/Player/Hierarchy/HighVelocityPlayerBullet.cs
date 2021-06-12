using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public abstract class HighVelocityPlayerBullet : PlayerBullet
    {
        protected override bool AutomaticallyDeactivate => false;

        private Vector3 _representedVelocity { get; set; }
        public override Vector2 RepresentedVelocity => _representedVelocity;

        // Velocity must present as 0 so the sprite doesn't automatically move.
        public sealed override Vector2 Velocity
        {
            get => Vector2.zero;
            set => _representedVelocity = value;
        }

        // HighVelocityPlayerBullets should have no Collider2D.
        public override float BulletTrailWidth => SpriteMap.Width;

        #region Prefabs

        //[SerializeField]
        //public SpriteRenderer _Sprite = null;

        #endregion Prefabs


        #region Prefab Properties

        //public SpriteRenderer Sprite => _Sprite;

        #endregion Prefab Properties


        #region Penetration

        protected int MaxPenetration { get; set; }
        private int NumberPenetrated { get; set; }

        #endregion Penetration


        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        private const int MaxRaycastHits = 8;
        protected RaycastHit2D[] RaycastHits { get; set; }

        // Collisions will be handled manually.
        public sealed override bool CollidesWithEnemy(Enemy enemy) => false;

        protected sealed override void OnPlayerBulletInit()
        {
            MaxPenetration = 1;
            RaycastHits = new RaycastHit2D[MaxRaycastHits];
        }

        protected virtual void OnPlayerRaycastBulletActivate() { }
        protected sealed override void OnActivate()
        {
            OnPlayerRaycastBulletActivate();
        }

        protected virtual void OnPlayerRaycastBulletSpawn() { }
        protected override void OnBulletSpawn()
        {
            NumberPenetrated = 0;
            OnPlayerRaycastBulletSpawn();
        }

        protected virtual void OnHighVelocityPlayerBulletFrameRun(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            UpdatePosition(deltaTime);
            OnHighVelocityPlayerBulletFrameRun(deltaTime, realDeltaTime);
        }

        private void UpdatePosition(float deltaTime)
        {
            Vector3 nextPosition = transform.position + (Vector3) (RepresentedVelocity * deltaTime);

            int numEnemies = GameUtil.LinecastGetAllEnemiesNonAlloc(transform.position, nextPosition, RaycastHits);

            for(int i = 0; i < numEnemies; i++)
            {
                var hit = RaycastHits[i];
                var enemy = hit.collider.GetComponent<Enemy>();

                transform.position = hit.point;
                enemy.CollideWithBullet(this);

                NumberPenetrated++;

                if (NumberPenetrated >= MaxPenetration)
                {
                    DeactivateSelf();
                    return;
                }
            }

            if (SpaceUtil.PointIsInDestructor(nextPosition))
                transform.position = nextPosition;
            else
                DeactivateSelf();
        }
    }
}