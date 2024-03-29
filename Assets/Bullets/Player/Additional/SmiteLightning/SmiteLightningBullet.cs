using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// Represents the visible line between two joints of a Smite powerup lightning bolt.
    /// </summary>
    /// <inheritdoc/>
    public class SmiteLightningBullet : SmiteBullet
    {
        public override float Scale
        {
            get => LocalScaleX;
            set => LocalScaleX = value;
        }
    }
}