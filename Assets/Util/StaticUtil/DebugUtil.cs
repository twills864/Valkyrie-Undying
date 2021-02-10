using System;
using System.Collections.Generic;
using Assets.GameTasks;
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

        }

        private static void InputMouseForward(KeyCode keyCode)
        {
            var _DebugEnemy = GameManager.Instance._DebugEnemy;

            var gameTask = new MoveSpeedForDuration(_DebugEnemy, new Vector2(5f, 5f), 1.5f);

            GameManager.Instance._DebugEnemy.StartTask(gameTask);
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
        }

        #endregion Input Engine

        /// <summary>
        /// Draws a red X for a brief time at the specified position.
        /// </summary>
        /// <param name="position">The position to draw the X.</param>
        public static void RedX(Vector2 position)
        {
            const float XRadius = 0.3f;

            Vector2 topLeft = position + new Vector2(-XRadius, XRadius);
            Vector2 topRight = position + new Vector2(XRadius, XRadius);
            Vector2 bottomRight = position + new Vector2(XRadius, -XRadius);
            Vector2 bottomLeft = position + new Vector2(-XRadius, -XRadius);

            const float RayTime = 1.5f;
            Debug.DrawLine(topLeft, bottomRight, ColorRed, RayTime);
            Debug.DrawLine(bottomLeft, topRight, ColorRed, RayTime);
        }

        /// <summary>
        /// Draws a red X for a brief time at the specified position, and
        /// displays a red Fleeting Text at this position.
        /// </summary>
        /// <param name="position">The position to draw the X.</param>
        /// <param name="message">The message to display.</param>
        public static void RedX(Vector2 position, object message)
        {
            RedX(position);

            const float YOffset = 0.5f;
            var fleetingText = GameManager.Instance.CreateFleetingText(message.ToString(), position + new Vector2(0, YOffset));
            var text = fleetingText.GetComponent<TextMesh>();
            text.color = ColorRed;
        }
    }

}