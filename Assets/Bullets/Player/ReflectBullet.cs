using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.Enemies;
using Assets.Bullets.EnemyBullets;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class ReflectBullet : PermanentVelocityPlayerBullet
    {
        protected override bool AutomaticallyDeactivate => false;
        public override AudioClip FireSound => SoundBank.RandomForceField;

        #region Prefabs

        [SerializeField]
        private float _DefaultScaleMultiplier = GameConstants.PrefabNumber;

        [SerializeField]
        private float _ScalePerLevel = GameConstants.PrefabNumber;

        [SerializeField]
        private float _ScaleTimePerLevel = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FadeOutTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float DefaultScaleMultiplier => _DefaultScaleMultiplier;
        private float ScalePerLevel => _ScalePerLevel;
        private float ScaleTimePerLevel => _ScaleTimePerLevel;
        private float FadeOutTime => _FadeOutTime;

        #endregion Prefab Properties


        private int PenetrationCount { get; set; }

        // Will reactivate Collider-disabling functionality after it's discovered
        // how to disable the Collider without disabling the entire gameObject.
        //private BoxCollider2D Collider { get; set; }

        private Vector3 InitialScale { get; set; }
        private ScaleTo ScaleIn { get; set; }
        private float ScaleInScaleX
        {
            get => ScaleIn.EndValue.x;
            set => ScaleIn.EndValue = VectorUtil.WithX3(ScaleIn.EndValue, value);
        }

        private Sequence FadeOutSequence { get; set; }
        private bool FadingOut { get; set; }

        protected override void OnPermanentVelocityBulletInit()
        {
            InitialScale = transform.localScale;
            Vector3 startScale = VectorUtil.WithX3(InitialScale, 0);
            ScaleIn = new ScaleTo(this, startScale, InitialScale, 1.0f);

            var fadeOut = new FadeTo(this, Alpha, 0, FadeOutTime);
            var deactivateSelf = GameTaskFunc.DeactivateSelf(this);
            FadeOutSequence = new Sequence(fadeOut, deactivateSelf);

            //Collider = GetComponent<BoxCollider2D>();
        }

        protected override void OnActivate()
        {
            Alpha = 1f;
            //Collider.enabled = true;
            FadingOut = false;

            ScaleIn.ResetSelf();
            FadeOutSequence.ResetSelf();
        }

        protected override void OnBulletSpawn()
        {
            PenetrationCount = BulletLevel;
            SetScaleLevel();
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!FadingOut)
                ScaleIn.RunFrame(deltaTime);
            else
                FadeOutSequence.RunFrame(deltaTime);
        }

        public override bool CollidesWithEnemy(Enemy enemy) => !FadingOut;

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            if (PenetrationCount == 0)
                BeginFadeOut();
            else
                PenetrationCount--;
        }

        protected sealed override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsEnemyBullet(collision))
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();
                GameManager.Instance.ReflectBullet(enemyBullet);
            }
        }

        protected override void OnBulletDeactivate()
        {
            LocalScaleX = 0;
        }

        private void SetScaleLevel()
        {
            const int PseudoInfiniteLevel = 20;
            int level = !IsMaxLevel ? BulletLevel : PseudoInfiniteLevel;

            float scaleScale = DefaultScaleMultiplier + (level * ScalePerLevel);
            ScaleInScaleX = InitialScale.x * scaleScale;
            ScaleIn.Duration = (level + 1) * ScaleTimePerLevel;
        }

        private void BeginFadeOut()
        {
            // Disabling Collider disables entire gameObject for unknown reasons.
            //Collider.enabled = false;
            FadingOut = true;
        }
    }
}