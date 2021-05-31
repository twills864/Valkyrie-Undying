using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Assets.Particles
{
    public class ParticleManager : ValkyrieSprite
    {
        public static ParticleManager Instance { get; private set; }

        public override TimeScaleType TimeScale => TimeScaleType.Default;
        protected override ColorHandler DefaultColorHandler() => new NullColorHandler();

        public ParticleSystem Particle { get; private set; }

        protected override void OnInit()
        {
            Instance = this;

            Particle = GetComponent<ParticleSystem>();
        }

        public void Emit(Vector3 position, Vector3 velocity, Color32 color, int count)
        {

            var emit = new EmitParams()
            {
                startColor = color,
                velocity = velocity * 0.5f,
                position = position,
            };

            Particle.Emit(emit, count);
        }


        public void EmitTest(int count = 5)
        {
            Particle.Emit(count);
        }
    }
}
