using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class SumLevelValueCalculator : LevelValueCalculator
    {
        public SumLevelValueCalculator(float baseValue, float increasePerLevel)
            : base(baseValue, increasePerLevel)
        {
        }

        protected override float CalculateValue()
        {
            var ret = BaseValue + (Level * IncreasePerLevel);
            return ret;
        }
    }
}
