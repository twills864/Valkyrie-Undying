using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <inheritdoc/>
    public class VictimMarker : UIElement
    {
        [SerializeField]
        private float FadeTime;

        [SerializeField]
        private float StartDistance;

        [SerializeField]
        private float EndDistance;

        private Vector3 StartOffset;
        private Vector3 EndOffset;

        private VictimMarkerCorner TopLeft;
        private VictimMarkerCorner TopRight;
        private VictimMarkerCorner BottomRight;
        private VictimMarkerCorner BottomLeft;

        public MonoBehaviour Host { get; set; }
        public Vector3 HostPosition => Host.transform.position;

        private FrameTimer FadeTimer { get; set; }

        // FadeRatio needs to be separate from FadeTimer, because the
        // marker may start fading out before it's done fading in.
        private FloatValueOverTime FadeRatio { get; set; }

        private bool Deactivating { get; set; }

        protected sealed override void OnUIElementInit()
        {
            FadeTimer = new FrameTimer(FadeTime);

            StartOffset = new Vector3(StartDistance, StartDistance, 0);
            EndOffset = new Vector3(EndDistance, EndDistance, 0);

        }

        protected override void OnActivate()
        {
            Deactivating = false;

            ActivateCorners();

            FadeTimer.Reset();

            FadeRatio = new FloatValueOverTime(0.0f, 1.0f, FadeTime);
        }

        public override void OnSpawn()
        {
            UpdateCorners(0f);
        }


        private void ActivateCorners()
        {
            var allConers = PoolManager.Instance.UIElementPool.GetMany<VictimMarkerCorner>(4);

            TopLeft = allConers[0];
            TopRight = allConers[1];
            BottomRight = allConers[2];
            BottomLeft = allConers[3];

            const float rotationAngle = 45f;
            Vector3 rotation = new Vector3(0, 0, rotationAngle);
            void SetCorner(VictimMarkerCorner corner, Vector3 newRotation)
            {
                Quaternion newQuaternion = MathUtil.RotationToQuaternion(newRotation);
                corner.transform.rotation = newQuaternion;
            }

            SetCorner(TopLeft, rotation);
            SetCorner(TopRight, -rotation);
            SetCorner(BottomRight, rotation);
            SetCorner(BottomLeft, -rotation);
        }

        public void StartDeactivation()
        {
            Deactivating = true;
            FadeTimer.Reset();

            float startAlpha = FadeRatio.Value;
            float newFadeTime = FadeRatio.Elapsed;
            FadeRatio = new FloatValueOverTime(startAlpha, 0.0f, newFadeTime);
        }

        protected override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            if (!Deactivating)
                transform.position = HostPosition;

            FadeRatio.Increment(deltaTime);

            UpdateCorners(deltaTime);
        }

        private void UpdateCorners(float deltaTime)
        {
            if (!FadeTimer.Activated)
            {
                if (FadeTimer.UpdateActivates(deltaTime) && Deactivating)
                {
                    DeactivateSelf();
                    return;
                }

                RecalculateCornerOffsets();
            }

            void SetPosition(VictimMarkerCorner corner)
            {
                corner.transform.position = transform.position + corner.PositionOffset;
            }

            SetPosition(TopLeft);
            SetPosition(TopRight);
            SetPosition(BottomRight);
            SetPosition(BottomLeft);

            if (!Deactivating)
            {
                string message = $"{TopLeft.Alpha} {TopLeft.transform.position}";
                DebugUI.SetDebugLabel("TopLeftEnd", message);
            }
        }

        private void RecalculateCornerOffsets()
        {
            float ratio = FadeRatio.Value;

            TopLeft.PositionOffset = MathUtil.ScaledPositionBetween(StartOffset, EndOffset, ratio);

            var pos = MathUtil.ScaledPositionBetween(StartOffset, EndOffset, ratio);

            void SetCorner(VictimMarkerCorner corner, float scaleX, float scaleY)
            {
                corner.PositionOffset = new Vector3(pos.x * scaleX, pos.y * scaleY, 0);
                corner.Alpha = ratio;
            }

            SetCorner(TopLeft, -1f, 1f);
            SetCorner(TopRight, 1f, 1f);
            SetCorner(BottomRight, 1f, -1f);
            SetCorner(BottomLeft, -1f, -1f);
        }

        protected override void OnDeactivate()
        {
            TopLeft.DeactivateSelf();
            TopRight.DeactivateSelf();
            BottomRight.DeactivateSelf();
            BottomLeft.DeactivateSelf();
        }
    }
}