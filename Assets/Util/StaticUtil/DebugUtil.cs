using System;
using System.Collections.Generic;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.Powerups;
using LogUtilAssets;
using UnityEngine;

namespace Assets.Util
{
    public static class DebugUtil
    {
        public static string DebugEnemyName = "DebugEnemy";

        public static Color ColorRed => new Color(1.0f, 0.5f, 0.5f);

        public static DebugUI DebugUi { get; set; }
        private static GameManager GameManager { get; set; }

        public static void Init(DebugUI debugUi, GameManager gameManager)
        {
            DebugUi = debugUi;
            GameManager = gameManager;
        }

        public static Color GetRandomPlayerColor()
        {
            const float fixedColor = 0.5f;
            var fixedColorSelector = RandomUtil.Int(3);

            Color ret;
            switch (fixedColorSelector)
            {
                case 0:
                    ret = new Color(fixedColor, RandomUtil.Float(), RandomUtil.Float());
                    break;
                case 1:
                    ret = new Color(RandomUtil.Float(), fixedColor, RandomUtil.Float());
                    break;
                case 2:
                    ret = new Color(RandomUtil.Float(), RandomUtil.Float(), fixedColor);
                    break;
                default:
                    throw new Exception($"Unknown color selector {nameof(fixedColorSelector)}");
            }

            return ret;
        }


        #region Input Methods

        private static void InputSpace(KeyCode keyCode)
        {
            //var position = SpaceUtil.WorldPositionUnderMouse();
            //float scale = 10.0f;
            //float duration = DebugUi.DebugTextBox.GetFloat(2.0f);
            //int level = DebugUi.DebugTextBox.GetInt(1);
            //VoidBullet.StartVoid(position, level, scale, duration);
            //RetributionBullet.StartRetribution(position, level, scale, duration);
        }

        private static void InputEnter(KeyCode keyCode)
        {
        }

        private static void InputPageUp(KeyCode keyCode)
        {
            DebugUi.AddGameSpeed(0.5f);
        }

        private static void InputPageDown(KeyCode keyCode)
        {
            DebugUi.AddGameSpeed(-0.5f);
        }

        private static void InputW(KeyCode keyCode)
        {
        }

        private static void InputA(KeyCode keyCode)
        {

        }

        private static void InputS(KeyCode keyCode)
        {

        }

        private static void InputD(KeyCode keyCode)
        {

        }

        private static void InputUp(KeyCode keyCode)
        {
            GameManager.DebugDecrementFireType();
        }

        private static void InputLeft(KeyCode keyCode)
        {

        }

        private static void InputDown(KeyCode keyCode)
        {
            GameManager.DebugIncrementFireType();
        }

        private static void InputRight(KeyCode keyCode)
        {

        }

        private static void InputMouseBack(KeyCode keyCode)
        {
            //var moveTo = new MoveTo(GameManager._DebugEnemy, SpaceUtil.WorldMap.Center, 1f);
            var moveBy = new MoveBy(GameManager._DebugEnemy, new Vector3(1f, 1f), 1f);
            var ease = new EaseIn(moveBy);

            GameManager._DebugEnemy.RunTask(ease);
        }

        private static void InputMouseForward(KeyCode keyCode)
        {
            if (SpaceUtil.TryGetGameObjectUnderMouse(out GameObject gameObject))
            {
                if (gameObject.name != "Destructor")
                {
                    LogUtil.Log(gameObject);

                    if(gameObject.TryGetComponent<Enemy>(out Enemy enemy) && enemy.name != "DebugEnemy")
                        enemy.DebugKill();
                }
            }

            SmiteBullet.DebugTestSmite();

            //var moveTo = new MoveTo(GameManager._DebugEnemy, SpaceUtil.WorldMap.Center, 1f);
            //var ease = new EaseOut(moveTo);

            //GameManager._DebugEnemy.RunTask(ease);
        }

        private static void InputKeypadPlus(KeyCode keyCode)
        {

        }

        private static void InputKeypadMinus(KeyCode keyCode)
        {

        }

        #endregion Input Methods

        #region Input Engine

        private class KeyCodeAction
        {
            public KeyCode KeyCode { get; set; }
            public Action<KeyCode> Action { get; set; }

            public KeyCodeAction(KeyCode keyCode, Action<KeyCode> action)
            {
                KeyCode = keyCode;
                Action = action;
            }

            public void Run()
            {
                Action(KeyCode);
            }
        }

        private static readonly List<KeyCodeAction> KeyCodeActions = new List<KeyCodeAction>()
        {
            new KeyCodeAction(KeyCode.W, InputW),
            new KeyCodeAction(KeyCode.A, InputA),
            new KeyCodeAction(KeyCode.S, InputS),
            new KeyCodeAction(KeyCode.D, InputD),
            new KeyCodeAction(KeyCode.UpArrow, InputUp),
            new KeyCodeAction(KeyCode.LeftArrow, InputLeft),
            new KeyCodeAction(KeyCode.DownArrow, InputDown),
            new KeyCodeAction(KeyCode.RightArrow, InputRight),
            new KeyCodeAction(KeyCode.PageUp, InputPageUp),
            new KeyCodeAction(KeyCode.PageDown, InputPageDown),
            new KeyCodeAction(KeyCode.Return, InputEnter),
            new KeyCodeAction(KeyCode.Space, InputSpace),
            new KeyCodeAction(KeyCode.Mouse3, InputMouseBack),
            new KeyCodeAction(KeyCode.Mouse4, InputMouseForward),
            new KeyCodeAction(KeyCode.KeypadPlus, InputKeypadPlus),
            new KeyCodeAction(KeyCode.KeypadMinus, InputKeypadMinus),
        };

        public static void HandleInput()
        {
            if (Input.anyKeyDown)
            {
                foreach (var pair in KeyCodeActions)
                {
                    if (Input.GetKeyDown(pair.KeyCode))
                    {
                        pair.Run();
                    }
                }
            }

            if (Input.GetMouseButton(2))
                GameManager._DebugEnemy.transform.position = SpaceUtil.WorldPositionUnderMouse();
        }

        #endregion Input Engine

        /// <summary>
        /// Draws a red X for a brief time at the specified position.
        /// </summary>
        /// <param name="position">The position to draw the X.</param>
        public static void RedX(Vector3 position, float rayTime = 1.5f)
        {
            const float XRadius = 0.3f;

            Vector3 topLeft = position + new Vector3(-XRadius, XRadius);
            Vector3 topRight = position + new Vector3(XRadius, XRadius);
            Vector3 bottomRight = position + new Vector3(XRadius, -XRadius);
            Vector3 bottomLeft = position + new Vector3(-XRadius, -XRadius);

            Debug.DrawLine(topLeft, bottomRight, ColorRed, rayTime);
            Debug.DrawLine(bottomLeft, topRight, ColorRed, rayTime);
        }

        /// <summary>
        /// Draws a red X for a brief time at the specified position, and
        /// displays a red Fleeting Text at this position.
        /// </summary>
        /// <param name="position">The position to draw the X.</param>
        /// <param name="message">The message to display.</param>
        public static void RedX(Vector3 position, string message)
        {
            RedX(position);

            const float YOffset = 0.5f;

            position.y += YOffset;
            var fleetingText = GameManager.Instance.CreateFleetingText(message, position);
            var text = fleetingText.GetComponent<TextMesh>();
            text.color = ColorRed;
        }

        /// <summary>
        /// Draws a red X for a brief time at the specified position, and
        /// displays a red Fleeting Text at this position.
        /// </summary>
        /// <param name="position">The position to draw the X.</param>
        /// <param name="message">The message to display.</param>
        public static void RedX(Vector3 position, object message)
        {
            RedX(position, message.ToString());
        }

        #region Get Types

        public static Type GetPowerupType<TPowerup>() where TPowerup : Powerup => typeof(TPowerup);
        public static Type GetOverrideFireStrategyType<TFireStrategy>() where TFireStrategy : PlayerFireStrategy => typeof(TFireStrategy);
        public static Type GetOverrideEnemyType<TEnemy>() where TEnemy : Enemy => typeof(TEnemy);

        #endregion Get Types
    }
}