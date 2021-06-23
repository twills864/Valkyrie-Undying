using UnityEngine;

namespace Assets.Util
{
    public static class PlayerPrefsUtil
    {
        public const string LastWeaponIndexKey = "LastWeaponIndex";
        public const string LastPowerupNameKey = "LastPowerupName";

        public const string MusicVolumeKey = "MusicVolume";
        public const string SoundEffectVolumeKey = "SoundEffectVolume";
        public const string ActivePlaylistsKey = "ActivePlaylists";

        public const string ToggleGoreKey = "Gore";

        public const int TrueInt = 1;
        public const int FalseInt = 0;

        public static bool GetBoolFromPrefs(string key, bool defaultValue)
        {
            int intValue = PlayerPrefs.GetInt(key, BoolToInt(defaultValue));
            bool boolValue = IntToBool(intValue);

            return boolValue;
        }

        public static void SaveBoolToPrefs(string key, bool value)
        {
            int intValue = BoolToInt(value);
            PlayerPrefs.SetInt(key, intValue);
        }

        private static int BoolToInt(bool b)
        {
            return b ? TrueInt : FalseInt;
        }

        private static bool IntToBool(int i)
        {
            return i != FalseInt ? true : false;
        }
    }
}
