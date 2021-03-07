﻿using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <inheritdoc/>
    public class FleetingText : UIElement
    {
        protected override ColorHandler DefaultColorHandler()
            => new TextMeshColorHandler(TextField);

        [SerializeField]
        private TextMesh TextField = null;
        private FrameTimer DestroyTimer { get; set; }
        private FrameTimer StartFadeTimer { get; set; }
        private FrameTimer FadeTimer { get; set; }
        private bool CurrentyFading { get; set; }

        public string Text
        {
            get => TextField.text;
            set => TextField.text = value;
        }

        public Color DefaultColor => new Color(1, 1, 1, 1);

        [SerializeField]
        private const float OpaqueTextTime = 1f;
        [SerializeField]
        private const float FadeTime = 0.5f;

        [SerializeField]
        private const float Speed = 1f;

        protected override void OnUIElementInit()
        {
            DestroyTimer = new FrameTimer(OpaqueTextTime + FadeTime);
            StartFadeTimer = new FrameTimer(OpaqueTextTime);
            FadeTimer = new FrameTimer(FadeTime);
            Velocity = new Vector2(0, Speed);
        }

        protected override void OnActivate()
        {
            TextField.color = DefaultColor;
            DestroyTimer.Reset();
            StartFadeTimer.Reset();
            FadeTimer.Reset();
            CurrentyFading = false;
        }

        protected override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            // Check if timer is destroyed
            if (DestroyTimer.UpdateActivates(deltaTime))
            {
                DeactivateSelf();
            }

            // Calculate start of fade
            else if (!CurrentyFading)
            {
                CurrentyFading = StartFadeTimer.UpdateActivates(deltaTime);
            }

            // Calculate alpha value of fade
            else
            {
                FadeTimer.Increment(deltaTime);

                float alpha = FadeTimer.RatioRemaining;
                var color = TextField.color;
                color.a = alpha;
                TextField.color = color;
            }
        }
    }
}