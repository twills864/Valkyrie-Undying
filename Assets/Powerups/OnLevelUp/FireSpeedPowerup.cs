using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
{
    public class FireSpeedPowerup : OnLevelUpPowerup
    {
        private const float FireDeltaBase = 1.2f;
        private const float FireDeltaIncrease = 0.15f;

        protected override LeveledValueCalculator DefaultValueCalculator
            => new LeveledValueCalculator(FireDeltaBase, FireDeltaIncrease);

        public override void OnLevelUp()
        {
            float playerFireDeltaTimeScale = ValueCalculator.Value;
            GameManager.Instance.PlayerFireDeltaTimeScale = playerFireDeltaTimeScale;
        }
    }
}
