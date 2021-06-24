using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Sound;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Options
{
    public class PlaylistCheckbox : ValkyrieSprite
    {
        #region Property Fields
        private bool _valueOnInit;
        #endregion Property Fields

        public override TimeScaleType TimeScale => TimeScaleType.Default;
        protected override ColorHandler DefaultColorHandler()
        {
            var text = new TextMeshColorHandler(Title);
            var toggle = new ToggleColorHandler(Toggle);

            var ret = new CollectionColorHandler(text);
            return ret;
        }

        #region Prefabs

        [SerializeField]
        private TextMesh _Title = null;

        [SerializeField]
        private Toggle _Toggle = null;

        [SerializeField]
        private float _ToggleMargin = GameConstants.PrefabNumber;

        [SerializeField]
        private float _ScreenEdgeMargin = GameConstants.PrefabNumber;

        [SerializeField]
        private float _VerticalOffset = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private TextMesh Title => _Title;
        private Toggle Toggle => _Toggle;
        public float ToggleMargin => _ToggleMargin;
        private float ScreenEdgeMargin => _ScreenEdgeMargin;
        public float VerticalOffset => _VerticalOffset;

        #endregion Prefab Properties


        public bool Value
        {
            get => Toggle.isOn;
            set => Toggle.isOn = value;
        }
        public bool ValueOnInit
        {
            get => _valueOnInit;
            private set
            {
                _valueOnInit = value;
                Value = value;
            }
        }

        public bool IsChanged => Value != ValueOnInit;

        public float ToggleScale => Toggle.transform.localScale.x;
        public Vector2 ToggleSize => ToggleScale * SpaceUtil.ScreenSizeToWorldSize(Toggle.GetComponent<RectTransform>().sizeDelta);
        public float ToggleOffsetX => (ToggleSize.x * 0.5f) + ToggleMargin;


        public float Width { get; set; }
        public float WidthHalf => Width * 0.5f;

        public string PlaylistName => Playlist.Name;
        private Playlist Playlist { get; set; }

        public void Init(Playlist playlist, bool isOn, float positionY)
        {
            Playlist = playlist;
            ValueOnInit = isOn;
            transform.position = SpaceUtil.WorldMap.Left.AddX(ScreenEdgeMargin).WithY(positionY);

            name += (playlist.Name);
            Title.text = playlist.Name;

            Init();
        }

        protected override void OnInit()
        {
            // Initialize label locations
            SetPosition(transform.position);

            Bounds titleBounds = Title.GetComponent<Renderer>().bounds;

            Width = titleBounds.size.x + ToggleSize.x + ToggleMargin;
        }

        public void SetPosition(Vector3 worldPosition)
        {
            //Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            //Toggle.transform.position = screenPosition;

            Title.transform.position = worldPosition.AddX(-ToggleOffsetX);
        }
    }
}
