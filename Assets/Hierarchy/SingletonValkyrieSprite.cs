using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;

namespace Assets
{
    public abstract class SingletonValkyrieSprite : ValkyrieSprite
    {
        public override TimeScaleType TimeScale => TimeScaleType.Default;

        public static SingletonValkyrieSprite Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                Instance.OnSceneChange();
                return;
            }
            else
            {
                Instance = this;
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
