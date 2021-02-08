using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.UI
{
    public abstract class UIElement : PooledObject
    {
        protected virtual void OnUIElementInit() { }
        public sealed override void OnInit()
        {
            OnUIElementInit();
        }
    }
}
