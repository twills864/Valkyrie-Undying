﻿using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Powerups;
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
        private float AlphaMax;

        [SerializeField]
        private float StartDistance;

        private Vector3 StartOffset;
        private Vector3 EndOffset;

        private VictimMarkerCorner TopLeft;
        private VictimMarkerCorner TopRight;
        private VictimMarkerCorner BottomRight;
        private VictimMarkerCorner BottomLeft;

        private IVictimHost _host;
        public IVictimHost Host
        {
            get => _host;
            set
            {
                _host = value;
                float distance = Host.VictimMarkerDistance;
                EndOffset = new Vector3(distance, distance, 0);
            }
        }
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
        }

        protected override void OnActivate()
        {
            Deactivating = false;
            ActivateCorners();

            FadeTimer.Reset();
            FadeRatio = new FloatValueOverTime(0.0f, 1.0f, FadeTime);
        }

        private void ActivateCorners()
        {
            var allConers = PoolManager.Instance.UIElementPool.GetMany<VictimMarkerCorner>(4);

            TopLeft = allConers[0];
            TopRight = allConers[1];
            BottomRight = allConers[2];
            BottomLeft = allConers[3];

            const float rotationAngle = 45f;
            Vector3 rotationVector = new Vector3(0, 0, rotationAngle);

            // In this case, -45 degrees is simply found by negating rotation.z
            Quaternion rotation = MathUtil.RotationToQuaternion(rotationVector);
            Quaternion minusRotation = new Quaternion(rotation.x, rotation.y, -rotation.z, rotation.w);

            TopLeft.transform.rotation = rotation;
            TopRight.transform.rotation = minusRotation;
            BottomRight.transform.rotation = rotation;
            BottomLeft.transform.rotation = minusRotation;
        }

        public override void OnSpawn()
        {
            UpdateCorners(0f);
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
                => corner.transform.position = transform.position + corner.PositionOffset;

            SetPosition(TopLeft);
            SetPosition(TopRight);
            SetPosition(BottomRight);
            SetPosition(BottomLeft);
        }

        private void RecalculateCornerOffsets()
        {
            float ratio = FadeRatio.Value;
            float alpha = ratio * AlphaMax;

            var pos = MathUtil.ScaledPositionBetween(StartOffset, EndOffset, ratio);

            void SetCorner(VictimMarkerCorner corner, float scaleX, float scaleY)
            {
                corner.PositionOffset = new Vector3(pos.x * scaleX, pos.y * scaleY, -1);
                corner.Alpha = alpha;
            }

            SetCorner(TopLeft, -1f, 1f);
            SetCorner(TopRight, 1f, 1f);
            SetCorner(BottomRight, 1f, -1f);
            SetCorner(BottomLeft, -1f, -1f);
        }

        public void StartDeactivation()
        {
            if (!Deactivating)
            {
                Deactivating = true;
                FadeTimer.Reset();

                float startAlpha = FadeRatio.Value;
                float newFadeTime = FadeRatio.Timer.Elapsed + float.Epsilon;
                FadeRatio = new FloatValueOverTime(startAlpha, 0.0f, newFadeTime);
            }
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