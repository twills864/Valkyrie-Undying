using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.UI;
using Assets.Util;
using UnityEngine;

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

        private ProgressBar ExpBar { get; set; }

        public ExperienceManager(DirectorBalance balance)
        {
            CurrentLevel = 0;
            CurrentExp = 0;
            ExpToNextLevel = balance.Experience.ExpToFirstLevel;
            ExpToNextLevelIncrease = balance.Experience.ExtraExpPerLevel;
            BaseEnemyExpRate = balance.Experience.BaseEnemyExpRate;

            ExpBar = PoolManager.Instance.UIElementPool.Get<ProgressBar>();
            InitExpBar();
        }

        private void InitExpBar()
        {
            Vector3 pos = SpaceUtil.WorldMap.Top - new Vector3(0, ExpBar.InitialSize.y * 1.5f);

            ExpBar.transform.position = pos;
            ExpBar.SetValues(0, ExpToNextLevel);
            ExpBar.OnSpawn();
        }

        public bool KilledEnemyLevelsUp(Enemy enemy)
        {
            float expGain = enemy.ExpMultiplier * BaseEnemyExpRate;
            CurrentExp += expGain;


            bool levelUp = CurrentExp >= ExpToNextLevel;

            if(!levelUp)
            {
                ExpBar.CurrentValue = CurrentExp;
            }
            else
            {
                CurrentLevel++;
                CurrentExp -= ExpToNextLevel;
                ExpToNextLevel += ExpToNextLevelIncrease;

                ExpBar.SetValues(CurrentExp, ExpToNextLevel);
            }

            return levelUp;
        }

        public string DebugLabel => $"{CurrentLevel} {CurrentExp.ToString("0.00")} / {ExpToNextLevel.ToString("0.00")} (+{ExpToNextLevelIncrease})";
    }
}
