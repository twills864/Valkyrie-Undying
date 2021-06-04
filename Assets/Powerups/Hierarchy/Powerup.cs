using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that can be collected by the player.
    /// </summary>
    /// <inheritdoc/>
    public abstract class Powerup
    {
        #region Property Fields

        private int _level;

        #endregion Property Fields

        public Powerup()
        {
            PowerupName = CalculatePowerupName();
        }

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
                LevelUp();
            }
        }

        /// <summary>
        /// Whether or not this powerup should be considered
        /// when applying powerups of this type.
        /// </summary>
        public bool IsActive => Level != 0;

        /// <summary>
        /// Whether or not this powerup is a direct modification
        /// to the player's default weapon.
        /// </summary>
        public virtual bool IsDefaultWeaponPowerup => false;

        /// <summary>
        /// The maximum level that can be achieved by the player.
        /// Some powerups may need a hard upper limit.
        /// </summary>
        public virtual int MaxLevel => int.MaxValue;

        /// <summary>
        /// The level achieved by the player, plus the number of pickups
        /// containing this powerup currently in play.
        /// Needed to prevent multiple powerup drops from spawning
        /// when the player is nearing MaxLevel, which could potentially
        /// result in the player going over MaxLevel.
        /// </summary>
        private int NumberCheckedOut { get; set; }

        public void CheckOut() => NumberCheckedOut++;
        public void CheckIn()
        {
            PoolManager.Instance.PickupPool.BeforePowerupCheckIn(this);
            NumberCheckedOut--;
        }

        public bool AreAllPowerupsCheckedOut => NumberCheckedOut >= MaxLevel;

        /// <summary>
        /// The Powerup Manager has an array of powerup lists
        /// where each list is separated by type of powerup.
        /// This index represents the array index
        /// of the related powerup list/
        /// </summary>
        public int PowerupManagerIndex { get; set; }

        /// <summary>
        /// Initializes the balance of this powerup from a set of values
        /// managed from the GameScene GameObject in the Unity editor.
        /// </summary>
        /// <param name="balance">The balance values.</param>
        protected abstract void InitBalance(in PowerupBalanceManager balance);

        public void Init(in PowerupBalanceManager balance)
        {
            InitBalance(in balance);

            LevelValueCalculators = ReflectionUtil
                .GetPropertiesSubclassableFrom<LevelValueCalculator>(this)
                .ToList();
        }

        /// <summary>
        /// Functionality that will be applied after the level of this powerup is changed.
        /// Useful if anything extra needs to be calculated beyond the new ValueCalculator value.
        /// </summary>
        public abstract void OnLevelUp();

        /// <summary>
        /// Functionality that handles the leveling up of this powerup.
        /// </summary>
        public void LevelUp()
        {
            foreach(var calculator in LevelValueCalculators)
                calculator.Level = Level;

            OnLevelUp();
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

        public virtual string NotificationName
        {
            get
            {
                string ret;
                if(Level == 1)
                     ret = $"{PowerupName}!";
                else
                    ret = $"{PowerupName}!\r\nMk. {Level}";
                return ret;
            }
        }



        /// <summary>
        /// The ValueCalculators that are used to calculate the relative power
        /// of this powerup.
        /// </summary>
        public List<LevelValueCalculator> LevelValueCalculators { get; set; }
    }
}
