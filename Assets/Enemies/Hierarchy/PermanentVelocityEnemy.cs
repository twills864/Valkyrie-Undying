using Assets.Constants;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityEnemy : FireStrategyEnemy
    {
        #region Prefabs

        [SerializeField]
        private float _PermanentVelocityX = GameConstants.PrefabNumber;
        [SerializeField]
        private float _PermanentVelocityY = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        protected float PermanentVelocityX => _PermanentVelocityX;
        protected float PermanentVelocityY => _PermanentVelocityY;

        #endregion Prefab Properties


        private Vector2 _velocity;
        public sealed override Vector2 Velocity => !IsVoidPaused ? _velocity : Vector2.zero;

        protected virtual void OnPermanentVelocityEnemyInit() { }
        protected sealed override void OnEnemyInit()
        {
            _velocity = new Vector2(PermanentVelocityX, PermanentVelocityY);
            OnPermanentVelocityEnemyInit();
        }
    }
}