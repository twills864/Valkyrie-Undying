using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets
{
    /// <inheritdoc/>
    public abstract class Bullet : PooledObject
    {
        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite;

        #endregion Prefabs


        #region Prefab Properties

        protected SpriteRenderer Sprite => _Sprite;

        #endregion Prefab Properties


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