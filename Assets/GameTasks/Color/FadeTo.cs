using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    public class FadeTo : FiniteTimeGameTask
    {
        private SpriteRenderer Sprite { get; set; }

        private float StartAlpha { get; set; }
        private float EndAlpha { get; set; }
        private float AlphaDifference { get; set; }

        public float Alpha
        {
            get => Sprite.color.a;
            set
            {
                var color = Sprite.color;
                color.a = value;
                Sprite.color = color;
            }
        }

        public FadeTo(GameTaskRunner target, float alpha, float duration) : base(target, duration)
        {
            Sprite = target.GetComponent<SpriteRenderer>();
            StartAlpha = Sprite.color.a;
            EndAlpha = alpha;
            AlphaDifference = EndAlpha - StartAlpha;

            Alpha = alpha;
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            Alpha = StartAlpha + (Timer.RatioComplete * AlphaDifference);
        }
    }
}
