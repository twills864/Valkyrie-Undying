using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerup
{
    public class FireSpeedPowerup : OnLevelUpPowerup
    {
        private const float FireDeltaBase = 1.2f;
        private const float FireDeltaIncrease = 0.15f;

        private float PlayerFireDeltaTimeScale { get; set; }

        public override void OnLevelUp()
        {
            PlayerFireDeltaTimeScale = FireDeltaBase + (FireDeltaIncrease * Level);
            GameManager.Instance.PlayerFireDeltaTimeScale = PlayerFireDeltaTimeScale;
        }
    }
}
