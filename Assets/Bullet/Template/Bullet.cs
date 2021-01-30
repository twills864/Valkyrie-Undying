using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Bullet
{
    public abstract class Bullet : PooledObject
    {
        protected abstract int BaseDamage { get; }
        public virtual int Damage => BaseDamage;

        public override string LogTagColor => "#B381FE";

        protected abstract void _Update(float deltaTime);
        private void Update()
        {
            _Update(Time.deltaTime);
        }

        protected abstract void _Init();
        public override void Init()
        {
            Debug.Log("");
            _Init();
        }

        protected virtual void _Start() { }
        private void Start()
        {
            _Start();
            Init();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (CollisionUtil.IsDestructor(collision))
            {
                Destroy(gameObject);
            }
        }
    }
}