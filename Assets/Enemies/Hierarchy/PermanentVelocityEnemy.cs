using Assets.Constants;
using UnityEngine;

namespace Assets.Enemies
{
    /// <summary>
    /// Represents an enemy whose velocity will be assigned
    /// as soon as the instance is created, and will never change.
    /// </summary>
    /// <inheritdoc/>
    public abstract class PermanentVelocityEnemy : AutomaticFireStrategyEnemy
    {
        #region Prefabs

        [SerializeField]
        private Vector2 _PermanentVelocity = Vector2.zero;

        #endregion Prefabs


        #region Prefab Properties

        protected Vector2 PermanentVelocity => _PermanentVelocity;

        #endregion Prefab Properties


        private Vector2 _velocity;
        public sealed override Vector2 Velocity => !IsVoidPaused
            ? (!IsChilled ? _velocity : _velocity * ChillScale)
            : Vector2.zero;

        protected virtual void OnPermanentVelocityEnemyInit() { }
        protected sealed override void OnEnemyInit()
        {
            _velocity = PermanentVelocity;
            OnPermanentVelocityEnemyInit();
        }
    }
}