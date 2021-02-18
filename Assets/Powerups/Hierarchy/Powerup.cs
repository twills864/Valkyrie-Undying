using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that can be collected by the player.
    /// </summary>
    /// <inheritdoc/>
    public abstract class Powerup
    {
        private int _level;

        /// <summary>
        /// The current level of this powerup.
        /// Each powerup starts at 0, and generally increases by 1
        /// each time a new powerup of the same type is collected.
        /// </summary>
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

        /// <summary>
        /// Whether or not this powerup should be considered
        /// when applying powerups of this type.
        /// </summary>
        public bool IsActive => Level != 0;

        /// <summary>
        /// The Powerup Manager has an array of powerup lists
        /// where each list is seperated by type of powerup.
        /// This index represents the array index
        /// of the related powerup list/
        /// </summary>
        public int PowerupManagerIndex { get; set; }

        /// <summary>
        /// Functionality that will be applied after the level of this powerup is changed.
        /// Useful if anything extra needs to be calculated beyond the new ValueCalculator value.
        /// </summary>
        public abstract void OnLevelUp();

        /// <summary>
        /// The value set to ValueCalculator on load.
        /// </summary>
        protected abstract LevelValueCalculator InitialValueCalculator { get; }

        /// <summary>
        /// The ValueCalculator that is used to calculate the relative power
        /// of this powerup.
        /// </summary>
        protected LevelValueCalculator ValueCalculator { get; set; }

        public Powerup()
        {
            ValueCalculator = InitialValueCalculator;
            PowerupName = CalculatePowerupName();
        }

        /// <summary>
        /// The name used to represent this powerup.
        /// </summary>
        public string PowerupName { get; }

        protected virtual string CalculatePowerupName()
        {
            string name = this.GetType().Name;
            name = name.Replace("Powerup", "");

            string ret = StringUtil.AddSpacesBeforeCapitals(name);
            return ret;
        }


        /// <summary>
        /// The current relative value of this power.
        /// (Used for debug viewing)
        /// </summary>
        private float CalculatedValue => ValueCalculator.Value;
    }
}
