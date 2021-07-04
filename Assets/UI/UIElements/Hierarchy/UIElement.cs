using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameTasks;

namespace Assets.UI
{
    /// <summary>
    /// Represents an element of the game that's visible to the player, but
    /// doesn't impact the game functionally.
    /// </summary>
    /// <inheritdoc/>
    public abstract class UIElement : PooledObject
    {
        public override TimeScaleType TimeScale => TimeScaleType.UIElement;

        protected virtual void OnUIElementInit() { }
        protected sealed override void OnInit()
        {
            OnUIElementInit();
        }
    }
}
