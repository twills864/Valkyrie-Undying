using Assets.Constants;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <summary>
    /// A label that flies directly up from a given point,
    /// and fades out after a brief moment.
    /// </summary>
    /// <inheritdoc/>
    public class FleetingText : UIElement
    {
        public override TimeScaleType TimeScale => TimeScaleType.UIElement;

        #region Prefabs

        [SerializeField]
        private TextMesh _TextField = null;

        [SerializeField]
        private float _OpaqueTextTime = GameConstants.PrefabNumber;
        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;
        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private TextMesh TextField => _TextField;

        private float OpaqueTextTime => _OpaqueTextTime;
        private float FadeTime => _FadeTime;
        private float Speed => _Speed;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new TextMeshColorHandler(TextField);

        private MoveBy Move { get; set; }
        public Vector3 MoveDistance
        {
            get => Move.Distance;
            set => Move.Distance = value;
        }

        private EaseIn3 Ease { get; set; }
        private Sequence Sequence { get; set; }

        public string Text
        {
            get => TextField.text;
            set => TextField.text = value;
        }

        public Color DefaultColor => new Color(1, 1, 1, 1);

        public float TotalTextTime => OpaqueTextTime + FadeTime;
        public float TotalDistance => Speed * TotalTextTime;
        private Vector3 DefaultMoveDistance => new Vector3(0, TotalDistance);

        protected override void OnUIElementInit()
        {
            Move = new MoveBy(this, DefaultMoveDistance, TotalTextTime);
            Ease = new EaseIn3(Move);

            var delay = new Delay(this, OpaqueTextTime);
            var fade = new FadeTo(this, 0, FadeTime);

            Sequence = new Sequence(delay, fade);
        }

        protected override void OnActivate()
        {
            TextField.color = DefaultColor;
            Ease.ResetSelf();
            Sequence.ResetSelf();
            MoveDistance = DefaultMoveDistance;
            //Velocity = new Vector2(0, Speed);
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            if (Sequence.FrameRunFinishes(deltaTime))
                DeactivateSelf();

            Ease.RunFrame(deltaTime);
        }
    }
}