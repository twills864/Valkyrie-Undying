using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.UnityPrefabStructs;
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
        private float _VelocityScaleMax = default;

        [SerializeField]
        private float _VelocityNoiseScale = GameConstants.PrefabNumber;

        [SerializeField]
        private float _VelocityNoiseOffset = GameConstants.PrefabNumber;

        [SerializeField]
        private float _PositionNoiseOffset = GameConstants.PrefabNumber;


        #endregion Prefabs

        public float VelocityNoiseScale => _VelocityNoiseScale;
        public float VelocityNoiseOffset => _VelocityNoiseOffset;
        public float PositionNoiseOffset => _PositionNoiseOffset;
        private float VelocityScaleMax => _VelocityScaleMax;

        #region Prefab Properties



        #endregion Prefab Properties

        public ParticleSystem Particle { get; private set; }

        protected override void OnInit()
        {
            Instance = this;

            Particle = GetComponent<ParticleSystem>();
        }

        public void Emit(Vector3 position, Vector3 velocity, int count, Color32 color)
        {
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

        public void Emit(Vector3 position, Vector3 velocity, int count, params Color32[] color)
        {
            var emit = new EmitParams();

            for (int i = 0; i < count; i++)
            {
                emit.startColor = RandomUtil.RandomElement(color);
                emit.velocity = AddVelocityNoise(velocity);
                emit.position = AddPositionNoise(position);

                Particle.Emit(emit, 1);
            }
        }

        public void EmitInColliderBounds(Collider2D collider, Vector3 velocity, int count, params Color32[] color)
        {
            //const float VelocityClamp = 0.5f;
            //velocity *= VelocityClamp;

            Vector2[] points = RandomUtil.RandomsPointsInsiderCollider(collider, count);

            var emit = new EmitParams();

            for (int i = 0; i < count; i++)
            {
                emit.startColor = RandomUtil.RandomElement(color);
                emit.velocity = AddVelocityNoise(velocity);
                emit.position = AddPositionNoise(points[i]);

                Particle.Emit(emit, 1);
            }
        }



        private Vector3 AddVelocityNoise(Vector3 velocity)
        {
            float scaleMin = 1f - VelocityNoiseScale;
            const float ScaleMax = 1f;
            //float scaleMax = 1f + VelocityNoiseScale;

            Vector2 normalVelocity = RandomUtil.Bool()
                ? new Vector2(-velocity.y, velocity.x)
                : new Vector2(velocity.y, -velocity.x);

            //Debug.DrawRay(Player.Position, velocity, Color.red, 1.0f);
            //Debug.DrawRay(Player.Position, normalVelocity, Color.cyan, 1.0f);

            float normalScale = RandomUtil.Float(0f, VelocityScaleMax);
            velocity.x += (normalScale * normalVelocity.x);
            velocity.y += (normalScale * normalVelocity.y);

            velocity *= RandomUtil.Float(scaleMin, ScaleMax);

            return velocity;

            //float averageOfValues = (velocity.x + velocity.y) * 0.5f;

            //velocity.x += RandomUtil.Float(-VelocityNoiseOffset, VelocityNoiseOffset);
            //velocity.x *= RandomUtil.Float(scaleMin, ScaleMax);

            //velocity.y += RandomUtil.Float(-VelocityNoiseOffset, VelocityNoiseOffset);
            //velocity.y *= RandomUtil.Float(scaleMin, ScaleMax);

            //return velocity;
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
