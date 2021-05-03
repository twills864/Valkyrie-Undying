using Assets.Constants;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityEnemy : BasicFireStrategyEnemy
    {
        #region Prefabs

        [SerializeField]
        private Vector2 _PermanentVelocity;

        #endregion Prefabs


        #region Prefab Properties

        protected Vector2 PermanentVelocity => _PermanentVelocity;

        #endregion Prefab Properties


        private Vector2 _velocity;
        public sealed override Vector2 Velocity => !IsVoidPaused ? _velocity : Vector2.zero;

        protected virtual void OnPermanentVelocityEnemyInit() { }
        protected sealed override void OnEnemyInit()
        {
            _velocity = PermanentVelocity;
            OnPermanentVelocityEnemyInit();
        }
    }
}