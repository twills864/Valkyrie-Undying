using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.Powerups;
using Assets.UI.PowerupMenu;
using Assets.Util.AssetsDebug;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Util
{
    public class DebugUI : MonoBehaviour
    {
        public const int DebugLabelFontSize = 20;
        public const float DebugBorderOffset = 20;

        private GameManager _GameManager { get; set; }
        private PowerupMenu _PowerupMenu { get; set; }

        [SerializeField]
        public DebugTextBox DebugTextBox { get; set; }

        [SerializeField]
        public InputField InputField;

        [SerializeField]
        public Button Button;

        [SerializeField]
        public Button ButtonShowPowerupMenu;

        [SerializeField]
        public Dropdown DropdownFireType;

        [SerializeField]
        public Slider SliderFireLevel;

        [SerializeField]
        public Text TextFireLevel;

        [SerializeField]
        public GameSceneDebugPowerupRow PowerupRow;

        [SerializeField]
        public Slider SliderGameSpeed;

        [SerializeField]
        public Text TextGameSpeed;

        private Type GameRowPowerupType => GameManager.GameRowPowerupType;
        private Powerup CurrentDebugPowerup => _GameManager._PowerupManager.AllPowerups[GameRowPowerupType];

        public void Init(CircularSelector<PlayerFireStrategy> fireStrategies, PowerupMenu powerupMenu)
        {
            _GameManager = GameManager.Instance;
            _PowerupMenu = powerupMenu;

            DebugTextBox = new DebugTextBox(InputField);

            var inputPos = SpaceUtil.ScreenMap.BottomLeft + new Vector3(DebugBorderOffset, DebugBorderOffset);
            SpaceUtil.SetLeftToPosition(InputField, inputPos);

            var buttonPos = SpaceUtil.ScreenMap.BottomRight + new Vector3(-DebugBorderOffset, DebugBorderOffset);
            SpaceUtil.SetRightToPosition(Button, buttonPos);

            var dropdownFireTypePos = SpaceUtil.ScreenMap.Right + new Vector3(-DebugBorderOffset, 0);
            SpaceUtil.SetRightToPosition(DropdownFireType, dropdownFireTypePos);

            const int showPowerupMenuButtonOffset = -50;
            var showPowerupMenuButtonPos = SpaceUtil.ScreenMap.Right + new Vector3(-DebugBorderOffset, showPowerupMenuButtonOffset);
            SpaceUtil.SetRightToPosition(ButtonShowPowerupMenu, showPowerupMenuButtonPos);

            const int powerupRowOffset = -120;
            var powerupRowPos = SpaceUtil.ScreenMap.Right + new Vector3(-DebugBorderOffset, powerupRowOffset);
            SpaceUtil.SetRightToPosition(PowerupRow, powerupRowPos);



            var strategiesToAdd = fireStrategies.Select(x => x.GetType().Name).ToList();
            DropdownFireType.AddOptions(strategiesToAdd);
            DropdownFireType.SetValueWithoutNotify(GameManager.Instance.DefaultFireTypeIndex);
            DropdownFireType.onValueChanged.AddListener(delegate
            {
                _GameManager.SetFireType(DropdownFireType.value, true);
            });

            const int sliderFireLevelYOffset = 15;
            var sliderFireLevelPos = SpaceUtil.ScreenMap.Right + new Vector3(-DebugBorderOffset, DebugBorderOffset + sliderFireLevelYOffset);
            SpaceUtil.SetRightToPosition(SliderFireLevel, sliderFireLevelPos);

            const int sliderFireLevelTextYOffset = 35;
            var sliderFireLevelTextPos = SpaceUtil.ScreenMap.Right + new Vector3(-DebugBorderOffset, DebugBorderOffset + sliderFireLevelTextYOffset);
            SpaceUtil.SetRightToPosition(TextFireLevel, sliderFireLevelTextPos);


            const int sliderGameSpeedYOffset = 180;
            var sliderGameSpeedPos = SpaceUtil.ScreenMap.Right + new Vector3(-DebugBorderOffset, DebugBorderOffset + sliderGameSpeedYOffset);
            SpaceUtil.SetRightToPosition(SliderGameSpeed, sliderGameSpeedPos);

            const int sliderGameSpeedTextYOffset = 200;
            var sliderGameSpeedTextPos = SpaceUtil.ScreenMap.Right + new Vector3(-DebugBorderOffset, DebugBorderOffset + sliderGameSpeedTextYOffset);
            SpaceUtil.SetRightToPosition(TextGameSpeed, sliderGameSpeedTextPos);

            //SliderFireLevel.value = 0;
            //DebugSliderFireLevelChanged(SliderFireLevel);

            //TimeSpan frameTime = TimeSpan.FromSeconds((double)1 / (double)Application.targetFrameRate);
            //SetDebugLabel("FRAMETIME", frameTime);

            PowerupRow.Init(CurrentDebugPowerup);
        }

        #region Debug Input

        public void ButtonPressed(Button button)
        {
            Color newRando = DebugUtil.GetRandomPlayerColor();
            _GameManager.RecolorPlayerActivity(newRando);
        }

        public void ShowPowerupMenuButtonPressed(Button button)
        {
            _GameManager.TogglePowerupMenuVisibility();
        }

        public void DebugSliderFireLevelChanged(Slider slider)
        {
            int level = (int) slider.value;
            TextFireLevel.text = level.ToString();
            _GameManager.WeaponLevel = level;
        }

        public void DebugSliderGameSpeedChanged(Slider slider)
        {
            float scale = slider.value * 2;
            scale = (float)Math.Round(scale, 0);
            scale *= 0.5f;

            TextGameSpeed.text = scale.ToString();
            Time.timeScale = scale;
        }

        public void AddGameSpeed(float scale)
        {
            SliderGameSpeed.value += scale;
            DebugSliderGameSpeedChanged(SliderGameSpeed);
        }


        public void PowerupMenuPowerLevelRowSet(Powerup powerup, int level)
        {
            var type = powerup.GetType();
            if (type == GameRowPowerupType)
                PowerupRow.PowerLevel = level;
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


        private static DebugMulti MouseLabelValue
            => new DebugMulti(
                () => SpaceUtil.WorldPositionUnderMouse(),
                () => (Vector2)Input.mousePosition);

        private static KeyValuePair<string, DebugValue> MouseLabelKvp
            = new KeyValuePair<string, DebugValue>("Mouse", MouseLabelValue);

        private static DebugFunc ObjectUnderMouseValue
            => new DebugFunc(() => SpaceUtil.TryGetGameObjectUnderMouse(out GameObject gameObject)
                ? gameObject.name : "-");

        private static KeyValuePair<string, DebugValue> ObjectUnderMouseKvp
            = new KeyValuePair<string, DebugValue>("Hover", ObjectUnderMouseValue);

        private static void DrawDebugLabels()
        {
            DrawDebugLabel(MouseLabelKvp);
            DrawDebugLabel(ObjectUnderMouseKvp);
            foreach (var kvp in DebugLabelValues)
                DrawDebugLabel(kvp);
        }
        private static void DrawDebugLabel(KeyValuePair<string, DebugValue> kvp)
        {
            string key = kvp.Key;
            string value = kvp.Value.Value;
            GUILayout.Label($"[{key}] {value}");
        }
        #endregion Debug Labels

        #region Debug Timers

        private static Dictionary<string, Stopwatch> DebugTimers { get; set; } = new Dictionary<string, Stopwatch>();

        private static string MissingTimerMessage(string timerName)
            => $"No such timer {timerName}!";
        private static void SetMissingTimerLabel(string timerName)
            => SetDebugLabel(timerName, MissingTimerMessage(timerName));

        public static Stopwatch StartTimer(string timerName)
        {
            Stopwatch stopwatch = CollectionUtil.GetOrAddNew(DebugTimers, timerName);
            stopwatch.Restart();

            return stopwatch;
        }

        public static void StopTimer(string timerName)
        {
            if (DebugTimers.TryGetValue(timerName, out Stopwatch stopwatch))
            {
                stopwatch.Stop();
                SetDebugLabel(timerName, stopwatch.Elapsed);
            }
            else
                SetMissingTimerLabel(timerName);
        }


        #endregion Debug Timers

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

            var frameRate = (int)(1f / (Time.unscaledDeltaTime));
            var fpsLabelPos = new Rect(Screen.width - fpsLabelWidth - fpsLabelOffset - guiOffset.x, guiOffset.y, fpsLabelWidth, fpsLabelHeight);
            string fpsLabelMessage = $"{frameRate} fps";

            GUILayout.BeginArea(fpsLabelPos, leftStyle);
            GUILayout.Label(fpsLabelMessage);
            GUILayout.EndArea();
        }

        #endregion Draw GUI


    }
}
