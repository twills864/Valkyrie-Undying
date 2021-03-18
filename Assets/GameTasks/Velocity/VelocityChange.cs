﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
    public class VelocityChange : FiniteVelocityGameTask
    {
        private Vector2Range VelocityRange { get; set; }

        public VelocityChange(ValkyrieSprite target, Vector2 endVelocity, float duration)
            : this(target, target.Velocity, endVelocity, duration)
        {
        }

        public VelocityChange(ValkyrieSprite target, Vector2 startVelocity, Vector2 endVelocity, float duration) : base(target, duration)
        {
            VelocityRange = new Vector2Range(startVelocity, endVelocity);

            Velocity = startVelocity;
        }

        private VelocityChange(ValkyrieSprite target, float duration) : base(target, duration)
        {
            VelocityRange = new Vector2Range();

            Velocity = Vector2.zero;
        }

        public void Init(Vector2 startVelocity, Vector2 endVelocity)
        {
            VelocityRange = new Vector2Range(startVelocity, endVelocity);

            Velocity = startVelocity;

            Timer.Reset();
        }

        protected override void OnFiniteVelocityTaskFrameRun(float deltaTime)
        {
            Velocity = VelocityRange.ValueAtRatio(Timer.RatioComplete);
        }

        public static VelocityChange Default(ValkyrieSprite target, float duration)
        {
            var ret = new VelocityChange(target, duration);
            ret.FinishSelf();
            return ret;
        }

        protected override string ToFiniteTimeGameTaskString()
        {
            return $"{VelocityRange.StartValue} -> {Velocity} -> {VelocityRange.EndValue}";
        }
    }
}
