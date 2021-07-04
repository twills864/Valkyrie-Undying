using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.MenuElements
{
    /// <summary>
    /// A Unity component container that can apply Valkyrie Sprite effects
    /// to a Unity Slider that represents a percentage from 0 to 100.
    /// </summary>
    /// <inheritdoc/>
    public class PercentSlider : ValkyrieSprite
    {
        public override TimeScaleType TimeScale => TimeScaleType.Default;
        protected override ColorHandler DefaultColorHandler()
        {
            var text = new TextMeshColorHandler(Percent);
            var slider = new SliderColorHandler(Slider);

            var ret = new CollectionColorHandler(text, slider);
            return ret;
        }

        #region Prefabs

        [SerializeField]
        private TextMesh _Title = null;

        [SerializeField]
        private Slider _Slider = null;

        [SerializeField]
        private TextMesh _Percent = null;

        [SerializeField]
        private float _SliderMargin = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private TextMesh Title => _Title;
        private Slider Slider => _Slider;
        private TextMesh Percent => _Percent;
        private float SliderMargin => _SliderMargin;

        #endregion Prefab Properties


        public int Value
        {
            get => (int) Slider.value;
            set => Slider.value = value;
        }

        public float SliderScale => Slider.transform.localScale.x;
        public Vector2 SliderSize => SliderScale * SpaceUtil.ScreenSizeToWorldSize(Slider.GetComponent<RectTransform>().sizeDelta);
        public float SliderOffsetX => (SliderSize.x * 0.5f) + SliderMargin;


        public float Width { get; set; }
        public float WidthHalf => Width * 0.5f;

        protected override void OnInit()
        {
            void ValueChanged(float value) => Percent.text = value.ToString();
            Slider.onValueChanged.AddListener(ValueChanged);

            // Initialize label locations
            SetPosition(transform.position);

            Bounds titleBounds = Title.GetComponent<Renderer>().bounds;
            Bounds percentBounds = Percent.GetComponent<Renderer>().bounds;

            Width = titleBounds.size.x + percentBounds.size.x + SliderSize.x + (2 * SliderMargin);
        }

        public void SetPosition(Vector3 worldPosition)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            Slider.transform.position = screenPosition;

            Title.transform.position = worldPosition.AddX(-SliderOffsetX);
            Percent.transform.position = worldPosition.AddX(SliderOffsetX);
        }
    }
}
