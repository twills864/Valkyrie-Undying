using System;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Powerups;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <inheritdoc/>
    [Obsolete(MetronomePowerup.MetronomeObsolete)]
    public class MetronomeLabel : UIElement
    {
        private static float AlphaMax;

        public static void StaticInit()
        {
            var corner = PoolManager.Instance.UIElementPool.Get<MetronomeLabel>();
            AlphaMax = corner.Alpha;
            corner.DeactivateSelf();
        }

        #region Prefabs

        [SerializeField]
        private TextMesh _Mesh = null;

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        #endregion Prefabs

        #region Prefab Properties

        private TextMesh Mesh => _Mesh;

        private float FadeTime => _FadeTime;

        #endregion Prefab Properties

        protected override ColorHandler DefaultColorHandler()
            => new TextMeshColorHandler(Mesh);

        public float Text
        {
            get => throw new NotImplementedException();
            set => Mesh.text = value.ToString("0.0");
        }

        public Enemy Host { get; set; }
        public Vector3 HostPosition => Host.ColliderMap.Center;

        private FrameTimer FadeTimer { get; set; }

        // FadeRatio needs to be separate from FadeTimer, because the
        // marker may start fading out before it's done fading in.
        private FloatValueOverTime FadeRatio { get; set; }

        private bool Deactivating { get; set; }

        protected sealed override void OnUIElementInit()
        {
            FadeTimer = new FrameTimer(FadeTime);
        }

        protected override void OnActivate()
        {
            Deactivating = false;

            FadeTimer.Reset();
            FadeRatio = new FloatValueOverTime(0.0f, 1.0f, FadeTime);
        }

        public override void OnSpawn()
        {
            transform.position = HostPosition;
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!Deactivating)
                transform.position = HostPosition;

            FadeRatio.Increment(deltaTime);

            HandleFade(deltaTime);
        }

        private void HandleFade(float deltaTime)
        {
            if (!FadeTimer.Activated)
            {
                if (FadeTimer.UpdateActivates(deltaTime) && Deactivating)
                {
                    DeactivateSelf();
                    return;
                }

                Alpha = FadeRatio.Value * AlphaMax;
            }
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
    }
}