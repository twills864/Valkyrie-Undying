using Assets.Util;
using Assets.Util.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Represents an object that can be stored inside of an Object Pool.
    /// </summary>
    public abstract class PooledObject : ManagedVelocityObject
    {
        /// <summary>
        /// An ID assigned to this object by its Object Pool when it's spawned.
        /// Guaranteed to be unique from any other active object spawned from the same pool.
        /// </summary>
        public int SpawnId;

        /// <summary>
        /// Subclass-specific functionality to happen when an object is activated.
        /// </summary>
        protected virtual void OnActivate() { }

        /// <summary>
        /// Activates this game object, and calls any subclass-specific implementation of OnActivate()
        /// </summary>
        public void ActivateSelf()
        {
            gameObject.SetActive(true);
            OnActivate();
        }

        /// <summary>
        /// Subclass-specific functionality to happen when an object is deactivated.
        /// </summary>
        protected virtual void OnDeactivate() { }

        /// <summary>
        /// Activates this game object, and calls any subclass-specific implementation of OnDeactivate()
        /// </summary>
        public void DeactivateSelf()
        {
            gameObject.SetActive(false);
            OnDeactivate();
        }
    }
}
