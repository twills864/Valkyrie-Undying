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

        public Vector2 SliderSize => SpaceUtil.ScreenSizeToWorldSize(Slider.GetComponent<RectTransform>().sizeDelta);
        public float SliderOffsetX => (SliderSize.x * 0.5f) + SliderMargin;

        protected override void OnInit()
        {
            void ValueChanged(float value) => Percent.text = value.ToString();
            Slider.onValueChanged.AddListener(ValueChanged);
        }

        public void SetPosition(Vector3 worldPosition)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            Slider.transform.position = screenPosition;

            Title.transform.position = VectorUtil.AddX3(worldPosition, -SliderOffsetX);
            Percent.transform.position = VectorUtil.AddX3(worldPosition, SliderOffsetX);
        }
    }
}
