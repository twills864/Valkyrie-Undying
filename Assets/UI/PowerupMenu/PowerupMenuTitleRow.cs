using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.PowerupMenu
{
    public class PowerupMenuTitleRow : MonoBehaviour
    {
        #region Prefabs

        [SerializeField]
        private Text _TextField = null;

        #endregion Prefabs

        #region Prefab Properties

        private Text TextField => _TextField;

        #endregion Prefab Properties

        public string Text
        {
            get => TextField.text;
            set => TextField.text = value;
        }
    }
}
