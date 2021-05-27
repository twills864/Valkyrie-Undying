using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.MenuElements
{
    public class TextMeshHolder : ValkyrieSprite
    {
        public override TimeScaleType TimeScale => TimeScaleType.Default;
        protected override ColorHandler DefaultColorHandler() => new TextMeshColorHandler(Text);

        #region Prefabs

        [SerializeField]
        private TextMesh _Text = null;

        #endregion Prefabs


        #region Prefab Properties

        public TextMesh Text => _Text;

        #endregion Prefab Properties

        public SpriteBoxMap BoxMap { get; private set; }

        protected override void OnInit()
        {
            BoxMap = new SpriteBoxMap(this);
        }
    }
}
