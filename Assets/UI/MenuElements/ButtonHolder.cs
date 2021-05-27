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
    public class ButtonHolder : ValkyrieSprite
    {
        public override TimeScaleType TimeScale => TimeScaleType.Default;
        protected override ColorHandler DefaultColorHandler()
        {
            var buttonHandler = new ButtonColorHandler(Button);

            var text = Button.GetComponentInChildren<Text>();
            var textHandler = new TextColorHandler(text);

            return new CollectionColorHandler(buttonHandler, textHandler);
        }

        #region Prefabs

        [SerializeField]
        private Button _Button = null;

        #endregion Prefabs


        #region Prefab Properties

        public Button Button => _Button;

        #endregion Prefab Properties

        public Vector2 ButtonSize => SpaceUtil.ScreenSizeToWorldSize(Button.GetComponent<RectTransform>().sizeDelta);
    }
}
