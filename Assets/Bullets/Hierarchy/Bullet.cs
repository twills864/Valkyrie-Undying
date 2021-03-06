using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets
{
    /// <inheritdoc/>
    public abstract class Bullet : PooledObject
    {
        [SerializeField]
        protected SpriteRenderer Sprite;

        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        protected virtual bool ShouldDeactivateOnDestructor => true;

        protected virtual void OnBulletInit() { }
        protected sealed override void OnInit()
        {

            OnBulletInit();
        }

        protected virtual void OnBulletTriggerExit2D(Collider2D collision) { }
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (CollisionUtil.IsDestructor(collision))
            {
                if(ShouldDeactivateOnDestructor)
                    DeactivateSelf();
            }
        }
    }
}