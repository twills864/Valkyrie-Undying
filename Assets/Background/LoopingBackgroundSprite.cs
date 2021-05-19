using Assets.Constants;
using Assets.Enemies;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Background
{
    /// <inheritdoc/>
    public class LoopingBackgroundSprite : ValkyrieSprite
    {
        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => gameObject.name.Contains("Large") ? DebugUI.Instance.DebugTextBox.GetFloat(_Speed) : _Speed;
        private SpriteRenderer Sprite => _Sprite;

        #endregion Prefab Properties

        public override TimeScaleType TimeScale => TimeScaleType.Default;

        protected override ColorHandler DefaultColorHandler()
        {
            return new NullColorHandler();
        }

        protected override void OnInit()
        {
            Sprite.size = SpaceUtil.WorldMapSize;
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            float offsetY = (Director.TotalTime * Speed);
            Vector2 size = SpaceUtil.WorldMapSize;
            size.y += offsetY;
            Sprite.size = size;
        }
    }
}