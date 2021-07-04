using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups;
using UnityEngine;

namespace Assets.UI.PowerupMenu
{
    /// <summary>
    /// A debug GUI that displays all powerups in the game, and allows
    /// the developer to arbitrarily set their levels.
    /// </summary>
    /// <inheritdoc/>
    public class PowerupMenu : MonoBehaviour
    {
        public static PowerupMenu Instance { get; private set; }

//#if !UNITY_EDITOR
        private const float UIScale = 2.5f;
        private Vector3 UIScaleVector => new Vector3(UIScale, UIScale, 1.0f);
        private Vector3 UIScaleVectorRow => new Vector3(1.0f, 1.0f, 1.0f);
        //#endif

#pragma warning disable 0414

        [SerializeField]
        private GameObject PowerupPanelContent = null;

        [SerializeField]
        private PowerupMenuTitleRow PowerupMenuTitleRowPrefab = null;

        [SerializeField]
        private PowerupMenuPowerupRow PowerupMenuPowerupRowPrefab = null;

#pragma warning restore 0414

        private Dictionary<Type, PowerupMenuPowerupRow> AllPowerupRows = new Dictionary<Type, PowerupMenuPowerupRow>();

        public void Init()
        {
            Instance = this;

#if !UNITY_EDITOR
            transform.localScale = UIScaleVector;
#endif
        }

        public void AddTitleRow(string title)
        {
            var newTitle = Instantiate(PowerupMenuTitleRowPrefab);
            newTitle.Text = title;

            newTitle.transform.parent = PowerupPanelContent.transform;

#if !UNITY_EDITOR
            newTitle.transform.localScale = UIScaleVectorRow;
#endif
        }

        public void AddPowerupRow(Powerup powerup)
        {
            var newRow = Instantiate(PowerupMenuPowerupRowPrefab);
            newRow.Init(powerup);

            newRow.transform.parent = PowerupPanelContent.transform;

            var powerupType = newRow.Powerup.GetType();
            AllPowerupRows[powerupType] = newRow;

#if !UNITY_EDITOR
            newRow.transform.localScale = UIScaleVectorRow;
#endif
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
