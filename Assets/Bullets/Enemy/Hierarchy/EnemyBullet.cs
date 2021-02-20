using Assets.Constants;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public abstract class EnemyBullet : Bullet
    {
        public override string LogTagColor => "#FFA197";
        public override GameTaskType TaskType => GameTaskType.EnemyBullet;

        protected SpriteRenderer Sprite { get; private set; }
        public Color DefaultColor { get; protected set; }

        public abstract int ReflectedDamage { get; }
        protected virtual Color? ReflectedColor => null;

        protected virtual void OnEnemyBulletInit() { }
        protected sealed override void OnBulletInit()
        {
            Sprite = GetComponent<SpriteRenderer>();
            DefaultColor = Sprite.color;
            OnEnemyBulletInit();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsPlayer(collision))
            {
                if (Player.Instance.CollideWithBullet(this))
                    DeactivateSelf();
            }
        }
    }
}
