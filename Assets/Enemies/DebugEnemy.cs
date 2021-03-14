﻿using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.UI;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class DebugEnemy : FireStrategyEnemy
    {
        protected override bool ShouldDeactivateOnDestructor => false;

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

        private Vector3 InfernoDamageAngle { get; set; }
        private CircularSelector<Vector3> AngleLanes { get; set; }
        private int LastDamage { get; set; }

        protected override EnemyFireStrategy InitialFireStrategy()
            => new VariantLoopingEnemyFireStrategy<BasicEnemyBullet>(FireSpeed, FireSpeedVariance);

        protected override void OnEnemyInit()
        {
            DamageTextAngleStep *= Mathf.Deg2Rad;
            InfernoDamageTextAngle *= Mathf.Deg2Rad;

            AngleLanes = new CircularSelector<Vector3>();

            for (float f = 0; f < MathUtil.Pi2f - 0.01f; f += DamageTextAngleStep)
                AngleLanes.Add(MathUtil.VectorAtRadianAngle(f, DamageTextDistance));

            InfernoDamageAngle = MathUtil.VectorAtRadianAngle(InfernoDamageTextAngle, InfernoDamageTextDistance);

            base.OnActivate();
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
            Vector3 moveDistance = new Vector3(moveX, moveY);
            return FleetingDamageText(damage, moveDistance, color);
        }

        private FleetingText FleetingDamageText(int damage, Vector3 moveDistance, Color color)
        {
            var text = CreateFleetingTextAtCenter(damage);
            text.SpriteColor = color;

            //var duration = text.TotalTextTime * 0.95f;
            //var move = new MoveBy(text, moveDistance, duration);
            //var ease = new EaseIn3(move);
            //text.RunTask(ease);

            text.MoveDistance = moveDistance;

            return text;
        }

        private void RefreshHealthBarText()
        {
            HealthBar.SetText(CurrentHealth);
            LastestDamageHealthBar.SetText(LastDamage);
        }
    }
}