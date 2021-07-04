using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet fired as part of the Victim powerup.
    /// Victim bullets fire in a straight line directly toward the targeted enemy.
    /// </summary>
    /// <inheritdoc/>
    public class VictimBullet : PlayerBullet
    {
        public override int Damage => VictimDamage;
        public int VictimDamage { get; set; }
        public override AudioClip FireSound => SoundBank.GunShort;

        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;

        #endregion Prefab Properties

    }
}