﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Assets.Powerups;
using Assets.Util;

namespace Assets.UI.PowerupMenu
{
    /// <summary>
    /// The debug powerup row that appears outside the powerup menu
    /// as part of the normal debug UI.
    /// </summary>
    /// <inheritdoc/>
    public class GameSceneDebugPowerupRow : MonoBehaviour
    {
        #region Prefabs

        [SerializeField]
        private Text _PowerupNameField = null;

        [SerializeField]
        private Button _ButtonMinus = null;

        [SerializeField]
        private InputField _InputPowerLevel = null;

        [SerializeField]
        private Button _ButtonPlus = null;

        #endregion Prefabs


        #region Prefab Properties

        private Text PowerupNameField => _PowerupNameField;
        private Button ButtonMinus => _ButtonMinus;
        private InputField InputPowerLevel => _InputPowerLevel;
        private Button ButtonPlus => _ButtonPlus;

        #endregion Prefab Properties


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
                if (PowerLevel != value)
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
                GameManager.Instance.PowerupRowPowerLevelChanged(value);
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

            string title = powerup.PowerupName;
            PowerupNameField.text = title;

            InputPowerLevel.text = powerup.Level.ToString();
        }
    }
}
