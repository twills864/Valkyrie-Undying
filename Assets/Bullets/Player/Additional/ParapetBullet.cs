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

        public void StartRise(float height)
        {
            const float RiseTime = 1.0f;

            Action<float> setHeight = x => HeightOffset = x;

            var rise = new ApplyFloatValueOverTime(this, setHeight, HeightOffset, height, RiseTime);
            RunTask(rise);

            // Manually move since position is normally recalculated when the player is moved.
            Vector3 riseDistance = new Vector3(0, height - HeightOffset);
            var move = new MoveBy(this, riseDistance, RiseTime);
            RunTask(move);
        }
    }
}