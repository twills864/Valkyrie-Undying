using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public abstract class LevelValueCalculator
    {
        private int _level = 0;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                Value = CalculateValue();
            }
        }
        protected abstract float CalculateValue();

        protected float BaseValue { get; }
        protected float IncreasePerLevel { get; }

        public LevelValueCalculator(float baseValue, float increasePerLevel)
        {
            BaseValue = baseValue;
            IncreasePerLevel = increasePerLevel;
        }

        public float Value { get; private set; }

        public static LevelValueCalculator Default() => new SumLevelValueCalculator(1, 1);
    }
}
