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
    public abstract class PooledObject : ManagedVelocityObject
    {
        //public List<PooledObject> Pool { get; set; }

        public virtual void DeactivateSelf()
        {
            gameObject.SetActive(false);
            PoolManager.SendHome(this);
            //Pool.Add(this);
        }

        public virtual void ActivateSelf()
        {
            gameObject.SetActive(true);
        }
    }
}
