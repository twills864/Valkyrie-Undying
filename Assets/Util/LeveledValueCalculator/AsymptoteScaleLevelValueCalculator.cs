using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Increases asymptotically from 1.0f to maxValue using the following equation:
    /// y = -(maxValue - 1) * (exponentBase ^ (x-1)) + maxValue
    ///
    /// exponentBase values close to 1 increase imperceptibly slowly.
    /// exponentBase values close to 0 approach maxValue nearly instantly.
    /// "Balanced" values are usually between 0.6 and 0.8.
    /// </summary>
    public class AsymptoteScaleLevelValueCalculator : LevelValueCalculator
    {
        private float Scale { get; set; }

        public AsymptoteScaleLevelValueCalculator(float exponentBase, float maxValue = 2.0f)
            : base(maxValue, exponentBase)
        {
            Scale = -(BaseValue - 1);
        }

        protected override float CalculateValue()
        {
            var exponent = Scale * Mathf.Pow(IncreasePerLevel, (Level - 1));

            var ret = exponent + BaseValue;
            return ret;
        }
    }
}
