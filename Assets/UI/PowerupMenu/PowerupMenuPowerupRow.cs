﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Assets.Powerups;

namespace Assets.UI.PowerupMenu
{
    public class PowerupMenuPowerupRow : MonoBehaviour
    {
        [SerializeField]
        private Text PowerupNameField = null;

        [SerializeField]
        private Button ButtonMinus;

        [SerializeField]
        private InputField InputPowerLevel = null;

        [SerializeField]
        private Button ButtonPlus;

        public Powerup Powerup { get; set; }

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
            set
            {
                if(PowerLevel != value)
                    InputPowerLevel.text = value.ToString();

                // OnPowerLevelChanged() is triggered through Unity when
                // the InputPowerLevel text is changed.
                // The above line is enough to trigger this event.
                //OnPowerLevelChanged(value);
            }
        }

        public void IncrementPowerLevel()
        {
            PowerLevel++;
        }
        public void DecrementPowerLevel()
        {
            PowerLevel--;
        }

        private void OnPowerLevelChanged(int value)
        {
            if (Powerup.Level != value)
            {
                Powerup.Level = value;
                GameManager.Instance.PowerupMenuPowerLevelRowSet(Powerup, value);
            }
        }

        public void OnInputChanged()
        {
            OnPowerLevelChanged(PowerLevel);
        }

        public void Init(Powerup powerup)
        {
            Powerup = powerup;
            PowerupName = powerup.PowerupName;
        }
    }
}
