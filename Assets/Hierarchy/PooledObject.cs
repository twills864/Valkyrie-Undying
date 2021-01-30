using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public abstract class PooledObject : FrameRunner
    {
        public List<PooledObject> Pool { get; set; }

        public new abstract void Init();
        public void Init(Vector2 position)
        {
            Debug.Log("");
            gameObject.transform.position = position;
            Init();
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            Pool.Add(this);
        }
    }
}
