using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;

namespace Assets
{
    /// <summary>
    /// Represents a ValkyrieSprite that will only allow a single represented instance,
    /// allowing them to easily persist between game scenes.
    /// </summary>
    /// <inheritdoc/>
    public abstract class SingletonValkyrieSpriteBase : ValkyrieSprite
    {
        public override TimeScaleType TimeScale => TimeScaleType.Default;

        protected abstract SingletonValkyrieSpriteBase InstanceToCompare { get; set; }

        public bool InitCalled { get; private set; }

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

                if (!InitCalled)
                    Init();
            }
        }

        protected abstract void OnSingletonInit();
        protected sealed override void OnInit()
        {
            InitCalled = true;
            OnSceneChange();
            OnSingletonInit();
        }

        protected virtual void OnSceneChange() { }
    }
}
