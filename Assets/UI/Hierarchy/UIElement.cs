using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameTasks;

namespace Assets.UI
{
    public abstract class UIElement : PooledObject
    {
        protected override GameTaskType TaskType => GameTaskType.UIElement;

        protected virtual void OnUIElementInit() { }
        public sealed override void OnInit()
        {
            OnUIElementInit();
        }
    }
}
