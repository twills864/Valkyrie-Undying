using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <summary>
    /// Manages each PowerupGuiIcon that appears on the bottom of the screen
    /// after collecting a new powerup.
    /// </summary>
    /// <inheritdoc/>
    public static class PowerupGui
    {
        private const float ScreenMargin = 0.25f;
        private const float TargetSpriteMargin = 0.25f;

        private const float SpriteMarginY = TargetSpriteMargin;
        private static float SpriteMarginX { get; set; }

        private static PowerupGuiIcon PowerupGuiIconTemplate { get; set; }
        private static Dictionary<Powerup, PowerupGuiIcon> AllPowerups { get; set; }

        private static Vector2 SpriteSize { get; set; }

        private static float MinX { get; set; }
        private static float MaxX { get; set; }
        private static Vector3 NextPosition;


        public static void Init(PowerupGuiIcon powerupGuiIcon)
        {
            PowerupGuiIconTemplate = powerupGuiIcon;
            AllPowerups = new Dictionary<Powerup, PowerupGuiIcon>();

            SpriteSize = PowerupGuiIconTemplate.SpriteSize;

            MinX = SpaceUtil.WorldMap.Left.x + (SpriteSize.x * 0.5f) + ScreenMargin;
            MaxX = SpaceUtil.WorldMap.Right.x - (SpriteSize.x * 0.5f) - ScreenMargin;

            float spawnY = SpaceUtil.WorldMap.Bottom.y + (SpriteSize.y * 0.5f) + ScreenMargin;
            NextPosition = new Vector3(MinX, spawnY);

            CalculateSpriteMarginX();
        }

        private static void CalculateSpriteMarginX()
        {
            float usableWidth = SpaceUtil.WorldMapSize.x - (2 * ScreenMargin);

            int maxSprites = (int)(usableWidth / (SpriteSize.x + TargetSpriteMargin));

            float remainingWidth = usableWidth - (maxSprites * SpriteSize.x);
            SpriteMarginX = remainingWidth / (maxSprites - 1);
        }

        public static void UpdatePowerup(Powerup powerup)
        {
            if (!AllPowerups.TryGetValue(powerup, out PowerupGuiIcon icon))
            {
                icon = AddNewIcon();
                icon.Init(powerup);

                AllPowerups[powerup] = icon;
            }

            icon.UpdateLabel();
        }

        private static PowerupGuiIcon AddNewIcon()
        {
            var icon = GameObject.Instantiate(PowerupGuiIconTemplate);
            icon.transform.position = NextPosition;
            RecalculateNextPosition();

            return icon;
        }

        private static void RecalculateNextPosition()
        {
            float nextX = NextPosition.x + SpriteSize.x + SpriteMarginX;

            if (nextX <= MaxX)
                NextPosition.x = nextX;
            else
            {
                NextPosition.x = MinX;
                NextPosition.y += SpriteSize.y + SpriteMarginY;
            }
        }
    }
}
