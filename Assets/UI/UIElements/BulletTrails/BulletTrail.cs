using Assets.Bullets;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <inheritdoc/>
    public class BulletTrail : UIElement
    {
        #region Prefabs

        [SerializeField]
        private TrailRenderer _Trail = null;

        #endregion Prefabs


        #region Prefab Properties

        public TrailRenderer Trail => _Trail;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new TrailColorHandler(Trail);

        public Bullet TargetBullet
        {
            get => TargetBulletTracker.Target;
            set => TargetBulletTracker.Target = value;
        }

        private float TrailTime
        {
            get => Trail.time;
            set
            {
                Trail.time = value;
                DeactivateTimer.ActivationInterval = value;
                DeactivateTimer.Reset();
            }
        }

        private FrameTimer DeactivateTimer { get; set; }
        private PooledObjectTracker<Bullet> TargetBulletTracker { get; set; }

        private bool CurrentlyDeactivating => DeactivationForced || !TargetBulletTracker.IsActive;
        private bool HandledDeactivation { get; set; }
        private bool DeactivationForced { get; set; }

        protected sealed override void OnUIElementInit()
        {
            DeactivateTimer = FrameTimer.Default();
            //DeactivateTimer.ActivateSelf();

            TargetBulletTracker = new PooledObjectTracker<Bullet>();
        }

        protected override void OnActivate()
        {
            HandledDeactivation = false;
            DeactivationForced = false;
        }

        public void OnSpawn(Bullet bullet)
        {
            BulletTrailInfo trailInfo = bullet.BulletTrailInfo;

            TargetBullet = bullet;

            TrailTime = trailInfo.TrailTime;
            SpriteColor = bullet.SpriteColor;
            SetStartWidth(bullet.BulletTrailWidth, trailInfo.TangentCurveScale);

            OnSpawn();
        }
        public override void OnSpawn()
        {
            transform.position = TargetBullet.transform.position;
            Trail.Clear();
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            DeltaTime = deltaTime;
            RealDeltaTime = realDeltaTime;
        }

        private float DeltaTime;
        private float RealDeltaTime;
        private void LateUpdate()
        {
            if (!CurrentlyDeactivating)
            {
                transform.position = TargetBullet.transform.position;
            }
            else
            {
                if(!HandledDeactivation)
                {
                    HandledDeactivation = true;
                    transform.position = TargetBullet.LastActivePosition;
                }

                if (DeactivateTimer.UpdateActivates(DeltaTime))
                    DeactivateSelf();
            }
        }

        public void ForceDeactivation()
        {
            DeactivationForced = true;
            HandledDeactivation = true;
            transform.position = TargetBulletTracker.IsActive ? TargetBullet.transform.position : TargetBullet.LastActivePosition;
        }


        public static BulletTrail SpawnBulletTrail(Bullet bullet)
        {
            Vector3 spawnPos = bullet.transform.position;
            var trail = PoolManager.Instance.UIElementPool.Get<BulletTrail>(spawnPos);
            trail.OnSpawn(bullet);

            return trail;
        }

        private void SetStartWidth(float width, float curveTangentScale)
        {
            Trail.startWidth = width;

            // Unity bug prevents us from using Trail.widthCurve.moveKey

            var keys = Trail.widthCurve.keys;
            keys[0].outTangent = -width * curveTangentScale; // NEED A PER-BULLET SCALE
            AnimationCurve curve = new AnimationCurve(keys);
            Trail.widthCurve = curve;
        }
    }
}