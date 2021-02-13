using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerup
{
    public interface IPowerupList
    {
        int PowerupManagerIndex { get; }
        void Init();
    }
    public abstract class PowerupList<T> : List<T>, IPowerupList where T : Powerup
    {
        public int PowerupManagerIndex { get; set; }

        public PowerupList(int powerupManagerIndex)
        {
            PowerupManagerIndex = powerupManagerIndex;
        }

        public void Init()
        {
            var types = ReflectionUtil.GetTypesSubclassableFrom<T>();

            foreach(var type in types)
            {
                var newType = (T) ReflectionUtil.CreateNew(type);
                newType.PowerupManagerIndex = PowerupManagerIndex;
                this.Add(newType);
            }
        }
    }
}
