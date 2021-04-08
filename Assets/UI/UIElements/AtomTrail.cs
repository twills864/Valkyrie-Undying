using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <inheritdoc/>
    public class AtomTrail : UIElement
    {
        #region Prefabs

        [SerializeField]
        public float _TrailTime;

        [SerializeField]
        private TrailRenderer _Trail;

        #endregion Prefabs

        #region Prefab Properties

        public float TrailTime => _TrailTime;

        private TrailRenderer Trail => _Trail;

        #endregion Prefab Properties

        protected override ColorHandler DefaultColorHandler()
            => new TrailColorHandler(Trail);

        private FrameTimer DeactivateTimer { get; set; }

        protected sealed override void OnUIElementInit()
        {
            DeactivateTimer = new FrameTimer(TrailTime);
            DeactivateTimer.ActivateSelf();
        }

        public void StartDeactivation()
        {
            Velocity = Vector2.zero;
            DeactivateTimer.Reset();
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            if(!DeactivateTimer.Activated)
            {
                if(DeactivateTimer.UpdateActivates(deltaTime))
                {
                    DeactivateSelf();
                }
            }
        }

        public override void OnSpawn()
        {
            Trail.Clear();
        }
    }
}