using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
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
            PowerupListName = CalculatePowerupName();
        }

        public void Init()
        {
            GameManager.Instance.AddPowerupMenuTitleRow(PowerupListName);

            var types = ReflectionUtil.GetTypesSubclassableFrom<T>();

            foreach(var type in types)
            {
                var newPowerup = (T) ReflectionUtil.CreateNew(type);
                newPowerup.PowerupManagerIndex = PowerupManagerIndex;
                this.Add(newPowerup);

                GameManager.Instance.AddPowerupMenuPowerupRow(newPowerup);
            }
        }

        /// <summary>
        /// The name used to represent this powerup list.
        /// </summary>
        public string PowerupListName { get; }

        protected virtual string CalculatePowerupName()
        {
            string name = this.GetType().Name;
            //name = name.Replace("Powerup", "");

            string ret = StringUtil.AddSpacesBeforeCapitals(name);
            return ret;
        }

        /// <summary>
        /// Returns the instance of a given type of powerup within this list.
        /// </summary>
        /// <typeparam name="TPowerup">The type of powerup to get.</typeparam>
        /// <returns>The instance of the given type of powerup.</returns>
        public TPowerup Get<TPowerup>() where TPowerup : T
        {
            TPowerup ret = this.Where(x => x.GetType() == typeof(TPowerup))
                .First() as TPowerup;
            return ret;
        }
    }
}
