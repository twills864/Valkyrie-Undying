using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI.PowerupMenu
{
    public class PowerupMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject PowerupPanelContent;

        [SerializeField]
        private PowerupMenuTitleRow PowerupMenuTitleRowPrefab;

        [SerializeField]
        private PowerupMenuPowerupRow PowerupMenuPowerupRowPrefab;

        public void AddTitlerow(string title)
        {
            var newTitle = Instantiate(PowerupMenuTitleRowPrefab);
            newTitle.Text = title;

            newTitle.transform.parent = PowerupPanelContent.transform;
        }
    }
}
