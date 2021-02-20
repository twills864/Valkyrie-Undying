using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Increases exponentially using the following formula:
    ///
    /// y = (BaseValue) * (IncreasePerLevel ^ (x - 1))
    /// </summary>
    public class ProductLevelValueCalculator : LevelValueCalculator
    {
        public ProductLevelValueCalculator(float baseValue, float increasePerLevel)
            : base(baseValue, increasePerLevel)
        {
        }

        protected override float CalculateValue()
        {
            var x = Level - 1;
            var ret = BaseValue * Mathf.Pow(IncreasePerLevel, x);
            return ret;
        }
    }
}
