﻿using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets
{
    /// <inheritdoc/>
    public abstract class Bullet : PooledObject
    {
        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        #endregion Prefabs


        #region Prefab Properties

        protected SpriteRenderer Sprite => _Sprite;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        protected virtual bool ShouldDeactivateOnDestructor => true;

        public ColliderBoxMap ColliderMap { get; private set; }

        protected virtual void OnBulletInit() { }
        protected sealed override void OnInit()
        {
            ColliderMap = new ColliderBoxMap(this);
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

        public virtual AudioClip FireSound => SoundBank.Silence;
        public virtual float FireSoundVolume => 1.0f;

        public void PlayFireSound() => PlaySoundAtCenter(FireSound, FireSoundVolume);

    }
}