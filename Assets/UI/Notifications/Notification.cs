using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI
{
    /// <summary>
    /// A message that will be displayed after a significant game event,
    /// such as gaining a new powerup, collecting a one-up, etc.
    /// </summary>
    /// <inheritdoc/>
    public class Notification : ValkyrieSprite
    {
        #region Prefabs

        [SerializeField]
        private float _AnimationTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _InitialScale = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FinalScale = GameConstants.PrefabNumber;

        [SerializeField]
        private float _InitialAlpha = GameConstants.PrefabNumber;

        [SerializeField]
        private TextMesh _Label = null;

        #endregion Prefabs


        #region Prefab Properties

        public float AnimationTime => _AnimationTime;
        public float FadeTime => _FadeTime;
        public float InitialScale => _InitialScale;
        public float FinalScale => _FinalScale;
        public float InitialAlpha => _InitialAlpha;
        private TextMesh Label => _Label;

        #endregion Prefab Properties


        public override TimeScaleType TimeScale => TimeScaleType.UIElement;

        protected override ColorHandler DefaultColorHandler()
            => new TextMeshColorHandler(Label);

        private ConcurrentGameTask Animation { get; set; }
        public bool AnimationFinished => Animation.IsFinished;

        protected override void OnInit()
        {
            ResetSelf();
            InitAnimation();
        }

        private void InitAnimation()
        {
            const float FinalAlpha = 1.0f;

            ScaleTo grow = new ScaleTo(this, InitialScale, FinalScale, AnimationTime);

            FadeTo fadeIn = new FadeTo(this, InitialAlpha, FinalAlpha, FadeTime);
            Delay fadeDelay = new Delay(this, AnimationTime - (2 * FadeTime));
            FadeTo fadeOut = new FadeTo(this, FinalAlpha, 0, FadeTime);
            Sequence fadeSequence = new Sequence(fadeIn, fadeDelay, fadeOut);

            Animation = new ConcurrentGameTask(grow, fadeSequence);
            Animation.FinishSelf();
        }

        public void Activate(string message)
        {
            Label.text = message;
            Animation.ResetSelf();
            gameObject.SetActive(true);
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            if(Animation.FrameRunFinishes(deltaTime))
                ResetSelf();
        }

        private void ResetSelf()
        {
            transform.localScale = new Vector3(InitialScale, InitialScale, 1.0f);
            Alpha = InitialAlpha;
            gameObject.SetActive(false);
        }
    }
}
