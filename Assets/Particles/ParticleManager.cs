using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
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

        #region Prefabs

        [SerializeField]
        private float _VelocityNoiseScale = GameConstants.PrefabNumber;

        [SerializeField]
        private float _PositionNoiseOffset = GameConstants.PrefabNumber;


        #endregion Prefabs

        public float VelocityNoiseScale => _VelocityNoiseScale;
        public float PositionNoiseOffset => _PositionNoiseOffset;

        #region Prefab Properties



        #endregion Prefab Properties

        public ParticleSystem Particle { get; private set; }

        protected override void OnInit()
        {
            Instance = this;

            Particle = GetComponent<ParticleSystem>();
        }

        public void Emit(Vector3 position, Vector3 velocity, Color32 color, int count)
        {
            const float VelocityClamp = 0.5f;
            velocity *= VelocityClamp;

            var emit = new EmitParams()
            {
                startColor = color,
            };

            for (int i = 0; i < count; i++)
            {
                emit.velocity = AddVelocityNoise(velocity);
                emit.position = AddPositionNoise(position);

                Particle.Emit(emit, 1);
            }
        }

        private Vector3 AddVelocityNoise(Vector3 velocity)
        {
            float scaleMin = 1f - VelocityNoiseScale;
            float scaleMax = 1f + VelocityNoiseScale;

            velocity.x *= RandomUtil.Float(scaleMin, scaleMax);
            velocity.y *= RandomUtil.Float(scaleMin, scaleMax);

            return velocity;
        }

        private Vector3 AddPositionNoise(Vector3 position)
        {
            float noiseMin = -PositionNoiseOffset;
            float noiseMax = PositionNoiseOffset;

            position.x += RandomUtil.Float(noiseMin, noiseMax);
            position.y += RandomUtil.Float(noiseMin, noiseMax);

            return position;
        }


        public void EmitTest(int count = 5)
        {
            Particle.Emit(count);
        }
    }
}
