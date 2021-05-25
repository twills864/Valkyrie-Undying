using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Variation of SingletonValkyrieSprite that allows multiple objects of the same class
    /// by assigning each one a unique tag.
    /// </summary>
    public abstract class TaggedSingletonValkyrieSprite : SingletonValkyrieSpriteBase
    {
        private static Dictionary<string, SingletonValkyrieSpriteBase> Instances
            = new Dictionary<string, SingletonValkyrieSpriteBase>();


        #region Prefabs

        [SerializeField]
        private string _Tag = null;

        #endregion Prefabs


        #region Prefab Properties

        private string Tag => _Tag;

        #endregion Prefab Properties


        protected override SingletonValkyrieSpriteBase InstanceToCompare
        {
            get => Instances.TryGetValue(Tag, out SingletonValkyrieSpriteBase instance) ? instance : null;
            set => Instances[Tag] = value;
        }
    }
}
