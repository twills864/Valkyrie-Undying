using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;
using Assets.Util;

namespace Assets.Powerups
{
    public interface IPowerupList
    {
        int PowerupManagerIndex { get; }
        void Init(Dictionary<Type, Powerup> allPowerups, in PowerupBalanceManager balance);
    }
    public abstract class PowerupList<TPowerUp> : List<TPowerUp>, IPowerupList where TPowerUp : Powerup
    {
        public int PowerupManagerIndex { get; set; }

        public PowerupList(int powerupManagerIndex)
        {
            PowerupManagerIndex = powerupManagerIndex;
            PowerupListName = CalculatePowerupName();
        }

        public void Init(Dictionary<Type, Powerup> allPowerups, in PowerupBalanceManager balance)
        {
            GameManager.Instance.AddPowerupMenuTitleRow(PowerupListName);

            var types = ReflectionUtil.GetTypesSubclassableFrom<TPowerUp>();

            foreach(var type in types)
            {
                var newPowerup = (TPowerUp) ReflectionUtil.CreateNew(type);
                newPowerup.PowerupManagerIndex = PowerupManagerIndex;
                newPowerup.Init(in balance);
                this.Add(newPowerup);
                allPowerups[type] = newPowerup;

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
        public TPowerup Get<TPowerup>() where TPowerup : TPowerUp
        {
            TPowerup ret = this.Where(x => x.GetType() == typeof(TPowerup))
                .First() as TPowerup;
            return ret;
        }
    }
}
