using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;

namespace Assets
{
    /// <summary>
    /// Which unique type of object best represents a given GameObject
    /// to use to calculate its current time scale.
    /// </summary>
    /// <inheritdoc/>
    public enum TimeScaleType
    {
        Default,
        Player,
        PlayerBullet,
        Enemy,
        EnemyBullet,
        UIElement
    }
}