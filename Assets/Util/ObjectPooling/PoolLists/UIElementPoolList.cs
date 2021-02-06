using Assets.Bullets;
using Assets.Enemies;
using Assets.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    // Will expand to other UI elements in the future
    public class UIElementPoolList : PoolList<FleetingText>
    {
        [SerializeField]
        private FleetingText FleetingTextPrefab;
    }
}
