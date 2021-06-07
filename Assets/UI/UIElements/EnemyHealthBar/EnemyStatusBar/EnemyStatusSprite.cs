using System.Collections;
using System.Collections.Generic;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    public class EnemyStatusSprite : UIElement
    {
        #region Property Fields
        private int _value;
        #endregion Property Fields

        protected override ColorHandler DefaultColorHandler() => new TextMeshColorHandler(TextMesh);

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        [SerializeField]
        private TextMesh _TextMesh = null;

        [SerializeField]
        private MeshRenderer _TextMeshRenderer = null;

        [SerializeField]
        private float _TextOffset = GameConstants.PrefabNumber;

        [SerializeField]
        private float _VerticalMargin = GameConstants.PrefabNumber;

        [SerializeField]
        private bool _LeftAligned = false;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;
        private TextMesh TextMesh => _TextMesh;
        private MeshRenderer TextMeshRenderer => _TextMeshRenderer;
        public float TextOffset => _TextOffset;
        public float VerticalMargin => _VerticalMargin;
        private bool LeftAligned => _LeftAligned;

        #endregion Prefab Properties

        public bool IsActive => Value > 0;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                TextMesh.text = value.ToString();

                if (value > 0)
                {
                    if (!IsEnabled)
                        IsEnabled = true;
                }
                else
                {
                    if (IsEnabled)
                        IsEnabled = false;
                }
            }
        }

        private bool IsEnabled
        {
            get => Sprite.enabled;
            set
            {
                Sprite.enabled = value;
                TextMeshRenderer.enabled = value;
            }
        }

        public float Height => BoxMap.Height;
        private SpriteBoxMap BoxMap { get; set; }

        protected override void OnUIElementInit()
        {
            SpriteColor = Sprite.color;
            BoxMap = new SpriteBoxMap(this);

            if (!LeftAligned)
            {
                TextMesh.transform.position = VectorUtil.WithX3(TextMesh.transform.position, -TextOffset);
                TextMesh.anchor = TextAnchor.MiddleRight;
                TextMesh.alignment = TextAlignment.Right;
            }
            else
            {
                TextMesh.transform.position = VectorUtil.WithX3(TextMesh.transform.position, TextOffset);
            }

            IsEnabled = false;
        }

        public override void OnSpawn()
        {
            Value = 0;
        }

        //protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //        Debug.Log($"{name}{RotationDegrees}");
        //}
    }
}