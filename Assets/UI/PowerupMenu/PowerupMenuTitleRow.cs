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
        [SerializeField]
        private Text TextField = null;

        public string Text
        {
            get => TextField.text;
            set => TextField.text = value;
        }
    }
}
