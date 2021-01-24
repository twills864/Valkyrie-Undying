using Assets.Util.AssetsDebug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Util
{
    public static class DebugUtil
    {
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
            var fixedColorSelector = RandomUtil.General.Int(3);

            Color ret;
            switch (fixedColorSelector)
            {
                case 0:
                    ret = new Color(fixedColor, RandomUtil.General.Float(), RandomUtil.General.Float());
                    break;
                case 1:
                    ret = new Color(RandomUtil.General.Float(), fixedColor, RandomUtil.General.Float());
                    break;
                case 2:
                    ret = new Color(RandomUtil.General.Float(), RandomUtil.General.Float(), fixedColor);
                    break;
                default:
                    throw new Exception($"UNKNOWN FIXEDCOLOR {fixedColorSelector}");
            }

            return ret;
        }


        #region Input Methods

        private static void InputSpace(KeyCode keyCode)
        {
            Debug.Log(keyCode);
            GameManager.SpawnBasicBullet();
            //GameManager.CreateFleetingText("Test " + RandomUtil.General.Int(), SpaceUtil.WorldPositionUnderMouse());
        }

        private static void InputEnter(KeyCode keyCode)
        {
            Debug.Log(keyCode);
        }

        private static void InputW(KeyCode keyCode)
        {
            Debug.Log(keyCode);
        }

        private static void InputA(KeyCode keyCode)
        {
            Debug.Log(keyCode);
        }

        private static void InputS(KeyCode keyCode)
        {
            Debug.Log(keyCode);
        }

        private static void InputD(KeyCode keyCode)
        {
            Debug.Log(keyCode);
        }

        private static void InputUp(KeyCode keyCode)
        {
            Debug.Log(keyCode);
        }

        private static void InputLeft(KeyCode keyCode)
        {
            Debug.Log(keyCode);
        }

        private static void InputDown(KeyCode keyCode)
        {
            Debug.Log(keyCode);
        }

        private static void InputRight(KeyCode keyCode)
        {
            Debug.Log(keyCode);
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
    }

}