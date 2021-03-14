using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <inheritdoc/>
    public class AtomTrail : UIElement
    {
        protected override ColorHandler DefaultColorHandler()
            => new TrailColorHandler(Trail);

        [SerializeField]
        public float TrailTime;

        [SerializeField]
        private TrailRenderer Trail;
        private FrameTimer DeactivateTimer { get; set; }

        protected sealed override void OnUIElementInit()
        {
            Trail = GetComponent<TrailRenderer>();
            DeactivateTimer = new FrameTimer(TrailTime);
            DeactivateTimer.ActivateSelf();
        }

        public void StartDeactivation()
        {
            Velocity = Vector2.zero;
            DeactivateTimer.Reset();
        }

        protected override void OnFrameRun(float deltaTime)
        {
            if(!DeactivateTimer.Activated)
            {
                if(DeactivateTimer.UpdateActivates(deltaTime))
                {
                    DeactivateSelf();
                }
            }
        }

        protected override void OnDeactivate()
        {
            Trail.Clear();
        }
    }
}