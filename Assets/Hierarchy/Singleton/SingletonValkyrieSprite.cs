using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;

namespace Assets
{
    /// <summary>
    /// Variation of SingletonValkyrieSprite that allows a single object of each subclass.
    /// </summary>
    /// <inheritdoc/>
    public abstract class SingletonValkyrieSprite : SingletonValkyrieSpriteBase
    {
        public static SingletonValkyrieSpriteBase Instance { get; private set; }

        protected override SingletonValkyrieSpriteBase InstanceToCompare
        {
            get => Instance;
            set => Instance = value;
        }
    }
}
