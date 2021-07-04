using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    /// <summary>
    /// Represents a status bar that will display an icon for each status effect
    /// an enemy has, and optionally a number describing the power of the effect.
    /// </summary>
    /// <inheritdoc/>
    public abstract class EnemyStatusBarBase : UIElement
    {
        protected sealed override ColorHandler DefaultColorHandler() => new NullColorHandler();

        #region Prefabs



        #endregion Prefabs


        #region Prefab Properties



        #endregion Prefab Properties

        protected EnemyStatusSpriteManager SpriteManager { get; set; }

        protected abstract void OnEnemyStatusBarInit();
        protected sealed override void OnUIElementInit()
        {
            List<EnemyStatusSprite> statuses = InitialStatusSprites();
            SpriteManager = new EnemyStatusSpriteManager(statuses);

            OnEnemyStatusBarInit();
        }

        protected abstract List<EnemyStatusSprite> InitialStatusSprites();

        protected abstract void OnEnemyStatusBarSpawn();
        public sealed override void OnSpawn()
        {
            SpriteManager.OnSpawn();
        }

        protected void RecalculateStatusBar() => SpriteManager.RecalculateStatusBar();

        protected void SetAndRecalculate(EnemyStatusSprite sprite, int newValue)
        {
            int oldValue = sprite.Value;
            bool previouslyActive = sprite.IsActive;

            if (oldValue != newValue)
            {
                bool nowActive = newValue > 0;
                if (nowActive && !previouslyActive)
                {
                    sprite.Value = newValue;
                    RecalculateStatusBar();
                }
                else
                    sprite.Value = newValue;
            }
        }
    }
}