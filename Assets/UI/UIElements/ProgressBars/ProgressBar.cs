using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    public class ProgressBar : UIElement
    {
        #region Property Fields

        private float _maxValue;
        private float _currentValue;

        #endregion

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Background = null;

        [SerializeField]
        private SpriteMask _FillMask = null;

        [SerializeField]
        private SpriteRenderer _FillSprite = null;


        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Background => _Background;
        private SpriteMask FillMask => _FillMask;
        private SpriteRenderer FillSprite => _FillSprite;

        #endregion Prefab Properties


        public override TimeScaleType TimeScale => TimeScaleType.Default;

        protected override ColorHandler DefaultColorHandler()
        {
            return new NullColorHandler();
            //throw new NotImplementedException();
        }

        public Vector2 InitialSize { get; private set; }
        public float InitialWidth => InitialSize.x;

        public SpriteBoxMap BackgroundMap { get; private set; }

        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                RecalculateBar();
            }
        }

        public float MaxValue
        {
            get => _maxValue;
            protected set
            {
                _maxValue = value;
                MaxValueInverse = 1 / _maxValue;
                RecalculateBar();
            }
        }

        private float MaxValueInverse { get; set; }


        protected override void OnUIElementInit()
        {
            BackgroundMap = new SpriteBoxMap(this, Background);

            InitialSize = BackgroundMap.Size;
        }

        protected override void OnActivate()
        {

        }

        public override void OnSpawn()
        {

        }

        public void SetValues(float current, float max)
        {
            _currentValue = current;
            MaxValue = max;
        }

        protected void RecalculateBar()
        {
            float ratioComplete = CurrentValue * MaxValueInverse;
            float ratioRemaining = 1f - ratioComplete;
            float offset = -1f * ratioRemaining * InitialWidth;

            FillMask.transform.position = new Vector3(offset, 0);
            FillMask.transform.localPosition = VectorUtil.WithY(FillMask.transform.localPosition, 0);
        }
    }
}
