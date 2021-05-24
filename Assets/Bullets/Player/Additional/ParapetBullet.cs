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
    public class ParapetBullet : PlayerBullet
    {
        private const float AnimationTime = 1.0f;

        protected override bool ShouldDeactivateOnDestructor => false;
        public override bool CollidesWithEnemy(Enemy enemy) => false;

        public float HeightOffset { get; set; }

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsEnemyBullet(collision))
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();
                GameManager.Instance.ReflectBullet(enemyBullet);
            }
        }

        public void Activate(float height, Vector3 scale)
        {
            ClearGameTasks();

            gameObject.SetActive(true);
            StartRise(height);
            StartFadeIn();
            ScaleUp(scale);
        }

        public void StartRise(float height)
        {
            Action<float> setHeight = x => HeightOffset = x;

            var rise = new ApplyFloatValueOverTime(this, setHeight, HeightOffset, height, AnimationTime);
            RunTask(rise);

            // Manually move since position is normally recalculated when the player is moved.
            Vector3 riseDistance = new Vector3(0, height - HeightOffset);
            var move = new MoveBy(this, riseDistance, AnimationTime);
            RunTask(move);
        }

        private void StartFadeIn()
        {
            FadeTo fadeIn = new FadeTo(this, Alpha, 1.0f, AnimationTime);
            RunTask(fadeIn);
        }

        private void ScaleUp(Vector3 scale)
        {
            Vector3 currentScale = transform.localScale;
            ScaleTo scaleTo = new ScaleTo(this, currentScale, scale, AnimationTime);

            RunTask(scaleTo);
        }
    }
}