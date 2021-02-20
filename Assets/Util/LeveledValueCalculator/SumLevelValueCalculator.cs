using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Increases linearly using the following formula:
    ///
    /// y = BaseValue + ((x - 1) * IncreasePerLevel)
    /// </summary>
    public class SumLevelValueCalculator : LevelValueCalculator
    {
        public SumLevelValueCalculator(float baseValue, float increasePerLevel)
            : base(baseValue, increasePerLevel)
        {
        }

        protected override float CalculateValue()
        {
            var x = Level - 1;
            var ret = BaseValue + (x * IncreasePerLevel);
            return ret;
        }
    }
}
