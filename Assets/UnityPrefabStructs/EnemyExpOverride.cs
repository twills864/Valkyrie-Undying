using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets
{
    /// <summary>
    /// Allows a given enemy's experience reward to be fine-tuned from the Unity editor.
    /// </summary>
    /// <inheritdoc/>
    [Serializable]
    public struct EnemyExpOverride
    {
        public bool ShouldOverride;
        public float Multiplier;

        public float? Value => ShouldOverride ? (float?)Multiplier : null;
    }
}
