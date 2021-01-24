using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Bullet
{
    public abstract class Bullet : MonoBehaviour
    {
        protected abstract int BaseDamage { get; }
        public virtual int Damage => BaseDamage;

        protected abstract void _Update(float deltaTime);
        private void Update()
        {
            _Update(Time.deltaTime);
        }

        protected virtual void _Init() { }
        public void Init()
        {
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