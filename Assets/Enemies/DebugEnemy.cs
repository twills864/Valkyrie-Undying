﻿using Assets.Constants;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.UI;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class DebugEnemy : Enemy
    {
        public override int BaseSpawnHealth => 100000;
        public override float SpawnHealthScaleRate => 1.0f;
        protected override bool ShouldDeactivateOnDestructor => false;

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new DebugEnemyStrategy();

        [SerializeField]
        private EnemyHealthBar LastestDamageHealthBar = null;

        [SerializeField]
        private float DamageTextDistance = GameConstants.PrefabNumber;

        [SerializeField]
        private float DamageTextAngleStep = GameConstants.PrefabNumber;

        [SerializeField]
        private float InfernoDamageTextAngle = GameConstants.PrefabNumber;

        [SerializeField]
        private float InfernoDamageTextDistance = GameConstants.PrefabNumber;



        private Vector2 InfernoDamageAngle { get; set; }
        private CircularSelector<Vector2> AngleLanes { get; set; }
        private int LastDamage { get; set; }


        protected override void OnEnemyInit()
        {
            base.OnActivate();

            DamageTextAngleStep *= Mathf.Deg2Rad;
            InfernoDamageTextAngle *= Mathf.Deg2Rad;

            AngleLanes = new CircularSelector<Vector2>();

            for (float f = 0; f < MathUtil.Pi2f - 0.01f; f += DamageTextAngleStep)
                AngleLanes.Add(MathUtil.VectorAtRadianAngle(f, DamageTextDistance));

            InfernoDamageAngle = MathUtil.VectorAtRadianAngle(InfernoDamageTextAngle, InfernoDamageTextDistance);
        }


        public override bool DamageKills(int damage)
        {
            LastDamage = damage;
            CurrentHealth -= damage;

            var moveDistance = AngleLanes.GetAndIncrement();
            FleetingDamageText(damage, moveDistance, Color.white);

            RefreshHealthBarText();

            return false;
        }

        private Color FleetingTextIgniteColor => new Color(1f, 0.5f, 0);
        protected override bool BurnKills()
        {
            CurrentHealth -= InfernoDamage;
            FleetingDamageText(InfernoDamage, InfernoDamageAngle, FleetingTextIgniteColor);

            InfernoDamage += InfernoDamageIncrease;

            RefreshHealthBarText();

            return false;
        }

        private FleetingText FleetingDamageText(int damage, float moveX, float moveY, Color color)
        {
            Vector2 moveDistance = new Vector2(moveX, moveY);
            return FleetingDamageText(damage, moveDistance, color);
        }

        private FleetingText FleetingDamageText(int damage, Vector2 moveDistance, Color color)
        {
            var text = CreateFleetingTextAtCenter(damage);
            text.Velocity = Vector2.zero;
            text.SpriteColor = color;

            var duration = text.TotalTextTime * 0.95f;
            var move = new MoveBy(text, moveDistance, duration);
            var ease = new EaseIn3(move);
            text.RunTask(ease);

            return text;
        }

        private void RefreshHealthBarText()
        {
            HealthBar.SetText(CurrentHealth);
            LastestDamageHealthBar.SetText(LastDamage);
        }
    }
}