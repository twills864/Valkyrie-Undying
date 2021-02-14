﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
{
    public abstract class Powerup
    {
        private int _level;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                ValueCalculator.Level = value;
                OnLevelUp();
            }
        }
        public bool IsActive => Level != 0;

        public int PowerupManagerIndex { get; set; }

        public abstract void OnLevelUp();

        protected abstract LeveledValueCalculator DefaultValueCalculator { get; }
        protected LeveledValueCalculator ValueCalculator { get; set; }

        public Powerup()
        {
            ValueCalculator = DefaultValueCalculator;
        }

        private float CalculatedValue => ValueCalculator.Value;
    }
}
