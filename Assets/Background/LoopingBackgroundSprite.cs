using Assets.Constants;
using Assets.Enemies;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Background
{
    /// <summary>
    /// Represents a sprite that will repeat indefinitely,
    /// designed to be used as a decoration for the background.
    /// </summary>
    /// <inheritdoc/>
    public class LoopingBackgroundSprite : TaggedSingletonValkyrieSprite
    {
        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;
        private SpriteRenderer Sprite => _Sprite;

        #endregion Prefab Properties

        public override TimeScaleType TimeScale => TimeScaleType.Default;

        protected override ColorHandler DefaultColorHandler()
        {
            return new NullColorHandler();
        }

        protected override void OnSingletonInit()
        {
            Sprite.size = SpaceUtil.WorldMapSize;
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            float scaledTime = deltaTime * Director.RetributionTimeScale;

            float increase = (Speed * scaledTime);

            Sprite.size = Sprite.size.AddY(increase);
        }
    }
}