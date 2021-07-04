using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy
{
    /// <summary>
    /// Represents a ValkyrieSprite that can be triggered by a racyasting bullet,
    /// or anything else that emits a raycast.
    /// </summary>
    interface IRaycastTrigger
    {
        void ActivateTrigger(Vector2 hitPoint);
    }
}
