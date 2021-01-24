﻿using Assets.Util.AssetsDebug;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Util
{
    public class DebugUI : MonoBehaviour
    {
        public const int DebugLabelFontSize = 20;
        public const float DebugBorderOffset = 20;

        private GameManager GameManager { get; set; }

        public DebugTextBox DebugTextBox { get; set; }

        public InputField InputField;
        public Button Button;
        public Dropdown DropdownFireType;
        public Slider SliderFireLevel;
        public Text TextFireLevel;


        public void Init(GameManager gameManager)
        {
            GameManager = gameManager;
            DebugTextBox = new DebugTextBox(InputField);

            var inputPos = SpaceUtil.ScreenMap.BottomLeft + new Vector2(DebugBorderOffset, DebugBorderOffset);
            SpaceUtil.SetLeftToPosition(InputField, inputPos);

            var buttonPos = SpaceUtil.ScreenMap.BottomRight + new Vector2(-DebugBorderOffset, DebugBorderOffset);
            SpaceUtil.SetRightToPosition(Button, buttonPos);

            var dropdownFireTypePos = SpaceUtil.ScreenMap.Right + new Vector2(-DebugBorderOffset, 0);
            SpaceUtil.SetRightToPosition(DropdownFireType, dropdownFireTypePos);

            const int sliderFireLevelYOffset = 15;
            var sliderFireLevelPos = SpaceUtil.ScreenMap.Right + new Vector2(-DebugBorderOffset, DebugBorderOffset + sliderFireLevelYOffset);
            SpaceUtil.SetRightToPosition(SliderFireLevel, sliderFireLevelPos);

            const int sliderFireLevelTextYOffset = 45;
            var sliderFireLevelTextPos = SpaceUtil.ScreenMap.Right + new Vector2(-DebugBorderOffset, DebugBorderOffset + sliderFireLevelTextYOffset);
            SpaceUtil.SetRightToPosition(TextFireLevel, sliderFireLevelTextPos);
        }

        #region Debug Input

        public void ButtonPressed(Button button)
        {
            Color newRando = DebugUtil.GetRandomPlayerColor();
            GameManager.RecolorPlayerActivity(newRando);
        }

        public void DebugSliderFireLevelChanged(Slider slider)
        {
            int level = (int) slider.value;
            TextFireLevel.text = level.ToString();
        }

        #endregion Debug Input


        #region Debug Labels

        private static Dictionary<string, DebugValue> DebugLabelValues { get; set; } = new Dictionary<string, DebugValue>();
        public static void SetDebugLabel(string key, object value)
        {
            DebugLabelValues[key] = new DebugConstant(value);
        }
        public static void SetDebugLabel(string key, Func<object> func)
        {
            DebugLabelValues[key] = new DebugFunc(func);
        }
        public static void SetDebugLabel(string key, params Func<object>[] funcs)
        {
            DebugLabelValues[key] = new DebugMulti(funcs);
        }


        private static DebugMulti MouseLabelValue => new DebugMulti(() => SpaceUtil.WorldPositionUnderMouse(),
            () => (Vector2)Input.mousePosition);
        private static KeyValuePair<string, DebugValue> MouseLabelKvp = new KeyValuePair<string, DebugValue>("Mouse", MouseLabelValue);
        private static void DrawDebugLabels()
        {
            DrawDebugLabel(MouseLabelKvp);
            foreach (var kvp in DebugLabelValues)
            {
                DrawDebugLabel(kvp);
            }
        }
        private static void DrawDebugLabel(KeyValuePair<string, DebugValue> kvp)
        {
            string key = kvp.Key;
            string value = kvp.Value.Value;
            GUILayout.Label($"[{key}] {value}");
        }
        #endregion Debug Labels


        #region Draw GUI

        private static GUIStyle GetLabelStyle(TextAnchor textAnchor)
        {
            var style = GUI.skin.GetStyle("Label");
            style.alignment = textAnchor;
            style.fontSize = DebugLabelFontSize;
            return style;
        }

        public void OnGUI()
        {
            Vector2 guiOffset = new Vector2(DebugBorderOffset, DebugBorderOffset);

            var leftStyle = GetLabelStyle(TextAnchor.MiddleLeft);
            //var rightStyle = GetLabelStyle(TextAnchor.MiddleRight);

            const int dbLabelSize = 600;
            GUILayout.BeginArea(new Rect(guiOffset.x, guiOffset.y, dbLabelSize, dbLabelSize), leftStyle);
            DrawDebugLabels();
            GUILayout.EndArea();

            const int fpsLabelWidth = 100;
            const int fpsLabelHeight = 250;
            const int fpsLabelOffset = -50;

            var frameRate = (int)(1f / Time.deltaTime); //Time.captureFramerate;
            var fpsLabelPos = new Rect(Screen.width - fpsLabelWidth - fpsLabelOffset - guiOffset.x, guiOffset.y, fpsLabelWidth, fpsLabelHeight);
            string fpsLabelMessage = $"{frameRate} fps";

            GUILayout.BeginArea(fpsLabelPos, leftStyle);
            GUILayout.Label(fpsLabelMessage);
            GUILayout.EndArea();
        }

        #endregion Draw GUI


    }
}