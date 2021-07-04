using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.UI;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Enemies
{
    /// <summary>
    /// An invincible enemy used to test new features
    /// that only appears while developing from the editor.
    /// </summary>
    /// <inheritdoc/>
    public class DebugEnemy : Enemy
    {
        protected override bool ShouldDeactivateOnDestructor => false;
        public override AudioClip FireSound => SoundBank.GunPistol;

        #region Prefabs

        [SerializeField]
        private EnemyHealthBar _LastestDamageHealthBar = null;

        [SerializeField]
        private float _DamageTextDistance = GameConstants.PrefabNumber;

        [SerializeField]
        private float _DamageTextAngleStep = GameConstants.PrefabNumber;

        [SerializeField]
        private float _InfernoDamageTextAngle = GameConstants.PrefabNumber;

        [SerializeField]
        private float _InfernoDamageTextDistance = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private EnemyHealthBar LastestDamageHealthBar => _LastestDamageHealthBar;

        private float DamageTextDistance => _DamageTextDistance;
        private float DamageTextAngleStep => _DamageTextAngleStep;

        private float InfernoDamageTextAngle => _InfernoDamageTextAngle;
        private float InfernoDamageTextDistance => _InfernoDamageTextDistance;

        #endregion Prefab Properties


        private Vector3 InfernoDamageAngle { get; set; }
        private CircularSelector<Vector3> AngleLanes { get; set; }
        private int LastDamage { get; set; }

        protected override EnemyFireStrategy InitialFireStrategy()
            => new InactiveEnemyStrategy();

        protected override void OnEnemyInit()
        {
            _DamageTextAngleStep *= Mathf.Deg2Rad;
            _InfernoDamageTextAngle *= Mathf.Deg2Rad;

            AngleLanes = new CircularSelector<Vector3>();

            for (float f = 0; f < MathUtil.Pi2f - 0.01f; f += DamageTextAngleStep)
                AngleLanes.Add(MathUtil.VectorAtRadianAngle(f, DamageTextDistance));

            InfernoDamageAngle = MathUtil.VectorAtRadianAngle(InfernoDamageTextAngle, InfernoDamageTextDistance);

            ParticleColor = Color.green;
            ParticleColorAlt = new Color32(255, 182, 193, 255);

            base.OnActivate();

            LastestDamageHealthBar.Init();
        }

        protected override void OnEnemySpawn()
        {
#if !UNITY_EDITOR
            // Despawn next frame
            var deactivate = GameTaskFunc.DeactivateSelf(this);
            RunTask(deactivate);
#endif
        }


        protected override bool DamageKills(int damage)
        {
            LastDamage = damage;
            CurrentHealth -= damage;

            var moveDistance = AngleLanes.GetAndIncrement();
            FleetingDamageText(damage, moveDistance, Color.white);

            RefreshHealthBarText();

            return false;
        }

        private Color FleetingTextIgniteColor => new Color(1f, 0.5f, 0);
        //protected override bool BurnKills()
        //{
        //    CurrentHealth -= InfernoDamage;
        //    FleetingDamageText(InfernoDamage, InfernoDamageAngle, FleetingTextIgniteColor);
        //    StatusBar.BurningDamage = InfernoDamage;

        //    InfernoDamage += InfernoDamageIncrease;

        //    RefreshHealthBarText();

        //    return false;
        //}

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

        public void DebugDeathEffect()
        {
            ParticleDeathEffect(null);
        }
    }
}