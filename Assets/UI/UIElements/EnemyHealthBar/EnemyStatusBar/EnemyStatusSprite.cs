using System.Collections;
using System.Collections.Generic;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    public class EnemyStatusSprite : UIElement
    {
        protected override ColorHandler DefaultColorHandler() => new SpriteColorHandler(Sprite);

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;

        #endregion Prefab Properties
    }
}