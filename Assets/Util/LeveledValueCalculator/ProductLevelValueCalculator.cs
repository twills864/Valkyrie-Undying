using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    public class ProductLevelValueCalculator : LevelValueCalculator
    {
        public ProductLevelValueCalculator(float baseValue, float increasePerLevel)
            : base(baseValue, increasePerLevel)
        {
        }

        protected override float CalculateValue()
        {
            var ret = BaseValue * Mathf.Pow(IncreasePerLevel, Level);
            return ret;
        }
    }
}
