using Assets.Util.AssetsDebug;
using Assets.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Util.ObjectPooling;
using Assets.UI;

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

            //DebugUI.SetDebugLabel("Mouse", () => $"{SpaceUtil.WorldPositionUnderMouse()} {(Vector2)Input.mousePosition}");
            //DebugUI.SetDebugLabel("int", () => DebugUi.DebugTextBox.GetInt(-9));
            //DebugUI.SetDebugLabel("float", () => DebugUi.DebugTextBox.GetFloat(-8.1f));
            //DebugUI.SetDebugLabel("double", () => DebugUi.DebugTextBox.GetDouble(-7.21));
        }

        public static FleetingText CreateFleetingText(string text, MonoBehaviour target)
        {
            return CreateFleetingText(text, target.transform.position);
        }
        public static FleetingText CreateFleetingText(string text, Vector2 position)
        {
            return GameManager.CreateFleetingText(text, position);
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
                    throw new Exception($"UNKNOWN FIXEDCOLOR {fixedColorSelector}");
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
            GameManager.DebugIncrementFireType();
        }

        private static void InputLeft(KeyCode keyCode)
        {

        }

        private static void InputDown(KeyCode keyCode)
        {
            GameManager.DebugDecrementFireType();
        }

        private static void InputRight(KeyCode keyCode)
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
            new KeyCodeAction(KeyCode.Return, InputEnter),
            new KeyCodeAction(KeyCode.Space, InputSpace)
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
        public static void RedX(Vector2 position, object message)
        {
            RedX(position);

            const float YOffset = 0.5f;
            var fleetingText = GameManager.Instance.CreateFleetingText(message.ToString(), position + new Vector2(0, YOffset));
            var text = fleetingText.GetComponent<Text>();
            text.color = ColorRed;
        }
    }

}