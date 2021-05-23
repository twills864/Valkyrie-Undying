using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class OneManArmyBullet : PermanentVelocityPlayerBullet
    {
        #region Prefabs

        [SerializeField]
        private float _OffsetX = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float OffsetX => _OffsetX;

        #endregion Prefab Properties


        public override AudioClip FireSound => SoundBank.ShotgunStrong;
    }
}