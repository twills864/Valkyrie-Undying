using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;
using Assets.Util;

namespace Assets.DirectorHelpers
{
    [DebuggerDisplay("{DebugLabel}")]
    public struct ExperienceManager
    {
        public int CurrentLevel { get; private set; }
        public float CurrentExp { get; private set; }
        public float ExpToNextLevel { get; private set; }
        public float ExpToNextLevelIncrease { get; private set; }
        public float BaseEnemyExpRate { get; private set; }

        public ExperienceManager(DirectorBalance balance)
        {
            CurrentLevel = 0;
            CurrentExp = 0;
            ExpToNextLevel = balance.Experience.ExpToFirstLevel;
            ExpToNextLevelIncrease = balance.Experience.ExtraExpPerLevel;
            BaseEnemyExpRate = balance.Experience.BaseEnemyExpRate;
        }

        public bool KilledEnemyLevelsUp(Enemy enemy)
        {
            float expGain = enemy.ExpMultiplier * BaseEnemyExpRate;
            CurrentExp += expGain;


            bool levelUp = CurrentExp >= ExpToNextLevel;

            if(levelUp)
            {
                CurrentLevel++;
                CurrentExp -= ExpToNextLevel;
                ExpToNextLevel += ExpToNextLevelIncrease;
            }

            return levelUp;
        }

        public string DebugLabel => $"{CurrentLevel} {CurrentExp.ToString("0.00")} / {ExpToNextLevel.ToString("0.00")} (+{ExpToNextLevelIncrease})";
    }
}
