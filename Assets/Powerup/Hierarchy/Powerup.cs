using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerup
{
    public abstract class Powerup
    {
        public int Level { get; set; }
        public bool IsActive => Level != 0;

        public int PowerupManagerIndex { get; set; }

        public abstract void OnLevelUp();
    }
}
