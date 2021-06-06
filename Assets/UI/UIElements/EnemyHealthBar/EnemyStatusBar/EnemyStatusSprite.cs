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
        protected override ColorHandler DefaultColorHandler() => new TextMeshColorHandler(TextMesh);

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        [SerializeField]
        private TextMesh _TextMesh = null;

        [SerializeField]
        private float _TextOffset = GameConstants.PrefabNumber;

        [SerializeField]
        private bool _LeftAligned = false;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;
        private TextMesh TextMesh => _TextMesh;
        public float TextOffset => _TextOffset;
        private bool LeftAligned => _LeftAligned;

        #endregion Prefab Properties

        protected override void OnUIElementInit()
        {
            SpriteColor = Sprite.color;

            if(!LeftAligned)
            {
                TextMesh.transform.position = VectorUtil.WithX3(TextMesh.transform.position, -TextOffset);
                TextMesh.anchor = TextAnchor.MiddleRight;
                TextMesh.alignment = TextAlignment.Right;
            }
            else
            {
                TextMesh.transform.position = VectorUtil.WithX3(TextMesh.transform.position, TextOffset);
            }
        }
    }
}