using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class LeveledValueCalculator
    {
        private int _level = 0;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                Value = BaseValue + (_level * IncreasePerLevel);
            }
        }

        public float Value { get; private set; }

        private float BaseValue;
        private float IncreasePerLevel;

        public LeveledValueCalculator(float baseValue, float increasePerLevel)
        {
            BaseValue = baseValue;
            IncreasePerLevel = increasePerLevel;
        }
    }
}
