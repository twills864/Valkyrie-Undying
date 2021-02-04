using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Represents an object with a velocity that will be manually managed.
    /// </summary>
    public abstract class ManagedVelocityObject : FrameRunner
    {
        public virtual Vector2 Velocity { get; set; }
        public virtual float VelocityX
        {
            get => Velocity.x;
            set => Velocity = new Vector2(value, Velocity.y);
        }
        public virtual float VelocityY
        {
            get => Velocity.y;
            set => Velocity = new Vector2(Velocity.x, value);
        }

        public abstract void OnInit();
        public void Init() => OnInit();
        public void Init(Vector2 position)
        {
            transform.position = position;
            Init();
        }
    }
}
