using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Increases asymptotically from initialValue to 1.0f using the following equation:
    /// y = (1 - initialValue) * -(exponentBase ^ (x - 1)) + 1
    ///
    /// exponentBase values close to 1 increase imperceptibly slowly.
    /// exponentBase values close to 0 approach maxValue nearly instantly.
    /// "Balanced" values are usually between 0.6 and 0.8.
    /// </summary>
    public class AsymptoteRatioLevelValueCalculator : LevelValueCalculator
    {
        private float InitialValueScale { get; set; }
        private float FinalValueScale { get; set; }

        public AsymptoteRatioLevelValueCalculator(float baseValue, float exponentBase, float scale = 1f)
            : base(baseValue, exponentBase)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(baseValue >= 0.0f && baseValue < 1.0f);
#endif
            InitialValueScale = (1 - BaseValue);
            FinalValueScale = scale;
        }

        protected override float CalculateValue()
        {
            var pow = -Mathf.Pow(IncreasePerLevel, (Level - 1));
            var exponent = (InitialValueScale * pow);

            var ret = (exponent + 1) * FinalValueScale;
            return ret;
        }
    }
}
