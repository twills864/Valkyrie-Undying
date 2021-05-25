using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;

namespace Assets
{
    public abstract class SingletonValkyrieSpriteBase : ValkyrieSprite
    {
        public override TimeScaleType TimeScale => TimeScaleType.Default;

        protected abstract SingletonValkyrieSpriteBase InstanceToCompare { get; set; }

        private void Awake()
        {
            if (InstanceToCompare != null && InstanceToCompare != this)
            {
                Destroy(gameObject);
                InstanceToCompare.OnSceneChange();
                return;
            }
            else
            {
                InstanceToCompare = this;
                DontDestroyOnLoad(gameObject);
                Init();
            }
        }

        protected abstract void OnSingletonInit();
        protected sealed override void OnInit()
        {
            OnSceneChange();
            OnSingletonInit();
        }

        protected virtual void OnSceneChange() { }
    }
}
