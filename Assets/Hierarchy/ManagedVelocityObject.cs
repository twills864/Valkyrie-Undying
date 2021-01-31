using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
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

        public virtual void Init() { }
        public virtual void Init(Vector2 position)
        {
            transform.position = position;
            Init();
        }
        public virtual void Init(Vector2 position, Vector2 velocity)
        {
            transform.position = position;
            Velocity = velocity;
            Init();
        }
    }
}
