using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;

namespace Assets.Powerups
{
    public abstract class OnHitPowerup : Powerup
    {
        public override void OnLevelUp() { }
        public abstract void OnHit(Enemy enemy);
    }
}
