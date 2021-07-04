using UnityEngine;

namespace Assets.Constants
{
    /// <summary>
    /// Contains constant values specified to the flow of the game.
    /// </summary>
    public static class GameConstants
    {
        /// <summary>
        /// The maximum level a player's weapon can achieve.
        /// </summary>
        public const int MaxWeaponLevel = 5;

        /// <summary>
        /// A default value to assign to prefab numbers in order to
        /// prevent the unassigned field warning message.
        /// </summary>
        public const int PrefabNumber = 0;


        public const string SceneNameMainMenu = "MainMenuScene";
        public const string SceneNameGame = "GameScene";
        public const string SceneNameOptions = "OptionsScene";
    }
}
