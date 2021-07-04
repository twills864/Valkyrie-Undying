using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Powerups;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Contains useful methods to save and load all information
    /// that either needs to be saved between game sessions,
    /// or should be saved.
    /// </summary>
    /// <inheritdoc />
    public static class SaveUtil
    {
        #region Property Fields
        private static List<Powerup> s_allPowerups;
        #endregion Property Fields

        private const string _SaveFileName = "Valkyrie.dat";

        private static SaveData Save = null;
        private static string SaveFilePath => $"{Application.persistentDataPath}/{_SaveFileName}";

        public static int HighScore
        {
            get => Save.HighScore;
            set
            {
                if(value > Save.HighScore)
                {
                    Save.HighScore = value;
                    Save.SaveGame();
                }
            }
        }

        public static List<Powerup> AllPowerups
        {
            get => s_allPowerups;
            set
            {
                s_allPowerups = value;
                PowerupNameMap = s_allPowerups.ToDictionary(x => x.GetType().Name);
            }
        }
        private static Dictionary<string, Powerup> PowerupNameMap { get; set; }

        public static void InitializeSave()
        {
            if(Save == null)
            {
                Save = new SaveData(SaveFilePath);
                Save.LoadGame();
            }
        }

        public static void InitializePowerups(List<Powerup> allPowerups)
        {
            // Create new List so that we can remove elements or otherwise modify this list as needed.
            AllPowerups = new List<Powerup>(allPowerups);
        }

        public static int LastWeapon
        {
            get => PlayerPrefs.GetInt(PlayerPrefsUtil.LastWeaponIndexKey, 0);
            set => PlayerPrefs.SetInt(PlayerPrefsUtil.LastWeaponIndexKey, value);
        }

        private static readonly string DefaultPowerupName = typeof(FireSpeedPowerup).Name;
        public static Powerup LastPowerup
        {
            get
            {
                try
                {
                    return PowerupNameMap[PlayerPrefs.GetString(PlayerPrefsUtil.LastPowerupNameKey, DefaultPowerupName)];
                }
                catch
                {
                    PlayerPrefs.SetString(PlayerPrefsUtil.LastPowerupNameKey, DefaultPowerupName);
                    return PowerupNameMap[DefaultPowerupName];
                }
            }
            set => PlayerPrefs.SetString(PlayerPrefsUtil.LastPowerupNameKey, value.GetType().Name);
        }
    }
}
