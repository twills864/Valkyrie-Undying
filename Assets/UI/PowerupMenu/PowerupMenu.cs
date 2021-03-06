﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups;
using UnityEngine;

namespace Assets.UI.PowerupMenu
{
    public class PowerupMenu : MonoBehaviour
    {
#pragma warning disable 0414

        [SerializeField]
        private GameObject PowerupPanelContent = null;

        [SerializeField]
        private PowerupMenuTitleRow PowerupMenuTitleRowPrefab = null;

        [SerializeField]
        private PowerupMenuPowerupRow PowerupMenuPowerupRowPrefab = null;

#pragma warning restore 0414

        private Dictionary<Type, PowerupMenuPowerupRow> AllPowerupRows = new Dictionary<Type, PowerupMenuPowerupRow>();

        public void AddTitleRow(string title)
        {
            var newTitle = Instantiate(PowerupMenuTitleRowPrefab);
            newTitle.Text = title;

            newTitle.transform.parent = PowerupPanelContent.transform;
        }

        public void AddPowerupRow(Powerup powerup)
        {
            var newRow = Instantiate(PowerupMenuPowerupRowPrefab);
            newRow.Init(powerup);

            newRow.transform.parent = PowerupPanelContent.transform;

            var powerupType = newRow.Powerup.GetType();
            AllPowerupRows[powerupType] = newRow;
        }

        public void OnMinimizeClick()
        {
            this.gameObject.SetActive(false);
        }

        public void OnPowerupAllClick()
        {
            foreach (var row in AllPowerupRows.Values)
                row.IncrementPowerLevel();
        }

        public void SetLevel<TPowerup>(int level) where TPowerup : Powerup
        {
            var type = typeof(TPowerup);
            SetLevel(type, level);
        }
        public void SetLevel(Type powerupType, int level)
        {
            var row = AllPowerupRows[powerupType];
            row.PowerLevel = level;
        }
    }
}
