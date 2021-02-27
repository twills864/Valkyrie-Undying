using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.UI;
using UnityEngine;

namespace Assets.Powerups
{
    public interface IVictimHost
    {
        float VictimMarkerDistance { get; }
        VictimMarker VictimMarker { get; set; }

        Transform transform { get; }
    }
}
