using Assets.Hierarchy.ColorHandlers;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets
{
    /// <summary>
    /// Represents a bullet either fired by the player, or by an enemy.
    /// </summary>
    /// <inheritdoc/>
    public abstract class Bullet : PooledObject
    {
        protected virtual bool ShouldDeactivateOnDestructor => true;

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        [SerializeField]
        private BulletTrailInfo _BulletTrailInfo = default;

        #endregion Prefabs


        #region Prefab Properties

        protected SpriteRenderer Sprite => _Sprite;
        public BulletTrailInfo BulletTrailInfo => _BulletTrailInfo;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        protected sealed override void OnSpriteColorSet(Color color)
        {
            if(CurrentBulletTrail != null)
                CurrentBulletTrail.SpriteColor = color;
        }

        protected sealed override void OnAlphaSet(float alpha)
        {
            if (CurrentBulletTrail != null)
                CurrentBulletTrail.Alpha = alpha;
        }

        public ColliderBoxMap ColliderMap { get; private set; }
        public SpriteBoxMap SpriteMap { get; private set; }

        public BulletTrail CurrentBulletTrail { get; private set; }
        public virtual float BulletTrailWidth => ColliderMap.Width;

        protected virtual void OnBulletInit() { }
        protected sealed override void OnInit()
        {
            ColliderMap = new ColliderBoxMap(this);
            SpriteMap = new SpriteBoxMap(this);
            OnBulletInit();
        }

        protected virtual void OnBulletSpawn() { }
        public sealed override void OnSpawn()
        {
            if (BulletTrailInfo.UseTrail)
                CurrentBulletTrail = BulletTrail.SpawnBulletTrail(this);
            else
                CurrentBulletTrail = null;

            OnBulletSpawn();
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

        protected virtual void OnBulletDeactivate() { }
        protected sealed override void OnDeactivate()
        {
            CurrentBulletTrail = null;
            OnBulletDeactivate();
        }

        public virtual AudioClip FireSound => SoundBank.Silence;
        public virtual float FireSoundVolume => 1.0f;

        public void PlayFireSound() => PlaySoundAtCenter(FireSound, FireSoundVolume);

        protected void DetachBulletTrail()
        {
            CurrentBulletTrail.ForceDeactivation();
        }
        protected void AttachNewBulletTrail()
        {
            CurrentBulletTrail = BulletTrail.SpawnBulletTrail(this);
        }

    }
}