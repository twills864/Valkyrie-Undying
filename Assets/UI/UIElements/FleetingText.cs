using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <inheritdoc/>
    public class FleetingText : UIElement
    {
        public override GameTaskType TaskType => GameTaskType.UIElement;
        protected override ColorHandler DefaultColorHandler()
            => new TextMeshColorHandler(TextField);

        [SerializeField]
        private TextMesh TextField = null;

        private bool CurrentyFading;

        private MoveBy Move { get; set; }
        public Vector2 MoveDistance
        {
            get => Move.Distance;
            set => Move.Distance = value;
        }

        private ConcurrentGameTask Concurrence { get; set; }

        public string Text
        {
            get => TextField.text;
            set => TextField.text = value;
        }

        public Color DefaultColor => new Color(1, 1, 1, 1);

        [SerializeField]
        private float OpaqueTextTime = 1f;
        [SerializeField]
        private float FadeTime = 0.5f;
        [SerializeField]
        private float Speed = 1f;

        public float TotalTextTime => OpaqueTextTime + FadeTime;
        public float TotalDistance => Speed * TotalTextTime;
        private Vector2 DefaultMoveDistance => new Vector2(0, TotalDistance);

        protected override void OnUIElementInit()
        {
            Move = new MoveBy(this, DefaultMoveDistance, TotalTextTime);
            var ease = new EaseIn3(Move);

            var delay = new Delay(this, OpaqueTextTime);
            var fade = new FadeTo(this, 0, FadeTime);

            var sequence = new SequenceGameTask(this, delay, fade);

            Concurrence = new ConcurrentGameTask(this, sequence, ease);
        }

        protected override void OnActivate()
        {
            TextField.color = DefaultColor;
            CurrentyFading = false;
            Concurrence.ResetSelf();
            MoveDistance = DefaultMoveDistance;
            //Velocity = new Vector2(0, Speed);
        }


        protected override void OnFrameRun(float deltaTime)
        {
            if (Concurrence.FrameRunFinishes(deltaTime))
                DeactivateSelf();
        }
    }
}