using System.Collections;
using System.Collections.Generic;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    /// <summary>
    /// The sprite rendered on the status bar when an enemy has a status effect.
    /// </summary>
    /// <inheritdoc/>
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
        private bool _LeftAligned = false;

        [SerializeField]
        private bool _HideValue = false;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;
        private TextMesh TextMesh => _TextMesh;
        private MeshRenderer TextMeshRenderer => _TextMeshRenderer;
        public float TextOffset => _TextOffset;
        private bool LeftAligned => _LeftAligned;
        private bool HideValue => _HideValue;

        #endregion Prefab Properties

        public bool IsActive => Value > 0;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;

                if(!HideValue)
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
                TextMesh.transform.position = TextMesh.transform.position.WithX(-TextOffset);
                TextMesh.anchor = TextAnchor.MiddleRight;
                TextMesh.alignment = TextAlignment.Right;
            }
            else
            {
                TextMesh.transform.position = TextMesh.transform.position.WithX(TextOffset);
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