using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy
{
    interface IRaycastTrigger
    {
        void ActivateTrigger(Vector2 hitPoint);
    }
}
