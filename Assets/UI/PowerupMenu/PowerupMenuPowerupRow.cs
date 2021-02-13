using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.PowerupMenu
{
    public class PowerupMenuPowerupRow : MonoBehaviour
    {
        [SerializeField]
        private Text PowerupNameField;

        [SerializeField]
        private Button ButtonMinus;

        [SerializeField]
        private InputField InputPowerLevel;

        [SerializeField]
        private Button ButtonPlus;

        public string PowerupName
        {
            get => PowerupNameField.text;
            set => PowerupNameField.text = value;
        }

        public int PowerLevel
        {
            get
            {
                if (!int.TryParse(InputPowerLevel.text, out int ret))
                    ret = 0;
                return ret;
            }
            set => InputPowerLevel.text = value.ToString();
        }

        public void IncrementPowerLevel()
        {
            PowerLevel++;
        }
        public void DecrementPowerLevel()
        {
            PowerLevel--;
        }
    }
}
