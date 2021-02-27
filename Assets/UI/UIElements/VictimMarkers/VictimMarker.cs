using Assets.Enemies;
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

        private Vector3 StartPosition;
        private Vector3 EndPosition;

        [SerializeField]
        private VictimMarkerCorner TopLeft;
        [SerializeField]
        private VictimMarkerCorner TopRight;
        [SerializeField]
        private VictimMarkerCorner BottomRight;
        [SerializeField]
        private VictimMarkerCorner BottomLeft;

        private VictimMarkerCorner[] AllCorners { get; set; }

        public MonoBehaviour Host { get; set; }
        public Vector3 HostPosition => Host.transform.position;

        private FrameTimer FadeTimer { get; set; }

        private bool Deactivating { get; set; }

        protected sealed override void OnUIElementInit()
        {
            FadeTimer = new FrameTimer(FadeTime);

            StartPosition = new Vector3(StartDistance, StartDistance, 0);
            EndPosition = new Vector3(EndDistance, EndDistance, 0);

            AllCorners = new VictimMarkerCorner[]
            {
                TopLeft,
                TopRight,
                BottomRight,
                BottomLeft
            };

            const float rotationAngle = 45f;
            Vector3 rotation = new Vector3(0, 0, rotationAngle);
            TopLeft.transform.Rotate(rotation);
            TopRight.transform.Rotate(-rotation);
            BottomRight.transform.Rotate(rotation);
            BottomLeft.transform.Rotate(-rotation);
        }

        protected override void OnActivate()
        {
            Deactivating = false;
            foreach (var corner in AllCorners)
                corner.Alpha = 0;

            FadeTimer.Reset();
        }

        public void StartDeactivation()
        {
            Deactivating = true;
            FadeTimer.Reset();
        }

        protected override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            if (!Deactivating)
            {
                string message = $"{TopLeft.Alpha} {TopLeft.transform.position}";
                DebugUI.SetDebugLabel("TopLeftStart", message);
            }

            if (!Deactivating)
                transform.position = HostPosition;

            if(!FadeTimer.Activated)
            {
                if(FadeTimer.UpdateActivates(deltaTime) && Deactivating)
                {
                    DeactivateSelf();
                    return;
                }

                float ratio = !Deactivating ? FadeTimer.RatioComplete : FadeTimer.RatioRemaining;

                var pos = MathUtil.ScaledPositionBetween(StartPosition, EndPosition, ratio);

                void SetCorner(VictimMarkerCorner corner, float scaleX, float scaleY)
                {
                    corner.Alpha = ratio;

                    Vector3 newPos = new Vector3(pos.x * scaleX, pos.y * scaleY, 0);
                    corner.transform.localPosition = newPos;
                }

                SetCorner(TopLeft, -1f, 1f);
                SetCorner(TopRight, 1f, 1f);
                SetCorner(BottomRight, 1f, -1f);
                SetCorner(BottomLeft, -1f, -1f);

                if (!Deactivating)
                {
                    string message = $"{TopLeft.Alpha} {TopLeft.transform.position}";
                    DebugUI.SetDebugLabel("TopLeftEnd", message);
                }

            }
        }
    }
}