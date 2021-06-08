using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Enemies;
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
        public const float MobileGuiScale = 3.0f;

        public static DebugUI Instance { get; private set; }
        public static Vector3 MobileGuiScaleVector => new Vector3(MobileGuiScale, MobileGuiScale, 1.0f);

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
        private Dropdown DropdownFireType;

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

        [SerializeField]
        public Toggle ToggleCanGameOver;

        [SerializeField]
        public Toggle ToggleGodMode;

        private Type GameRowPowerupType => SaveUtil.LastPowerup.GetType();
        private Powerup CurrentDebugPowerup => SaveUtil.LastPowerup;

        public void Init(CircularSelector<PlayerFireStrategy> fireStrategies, PowerupMenu powerupMenu)
        {
            Instance = this;

            _GameManager = GameManager.Instance;
            _PowerupMenu = powerupMenu;

            DebugTextBox = new DebugTextBox(InputField);

#if !UNITY_EDITOR
            Vector3 newScale = MobileGuiScaleVector;
            InputField.transform.localScale = newScale;

            var setAll = new Component[]
            {
                InputField,
                Button,
                DropdownFireType,
                ButtonShowPowerupMenu,
                PowerupRow,
                SliderFireLevel,
                TextFireLevel,
                SliderGameSpeed,
                TextGameSpeed,
                ToggleCanGameOver,
                ToggleGodMode,
            };
            foreach (Component component in setAll)
                component.transform.localScale = newScale;

            ToggleCanGameOver.isOn = true;
            ToggleGodMode.isOn = false;
#else
            ToggleCanGameOver.isOn = false;
            ToggleGodMode.isOn = true;
            Player.Instance.GodMode = true;
#endif

            var inputPos = SpaceUtil.ScreenMap.BottomLeft + new Vector3(DebugBorderOffset, DebugBorderOffset);
            SpaceUtil.SetLeftToPosition(InputField, inputPos);

            var buttonPos = SpaceUtil.ScreenMap.BottomRight + new Vector3(-DebugBorderOffset, DebugBorderOffset);
            SpaceUtil.SetRightToPosition(Button, buttonPos);

            var dropdownFireTypePos = SpaceUtil.ScreenMap.Right + new Vector3(-DebugBorderOffset, 0);
            SpaceUtil.SetRightToPosition(DropdownFireType, dropdownFireTypePos);

#if UNITY_EDITOR
            const int OffsetScale = 1;
#else
            const int OffsetScale = 5;
#endif

            void SetRight(MonoBehaviour target, float offsetScaleMultiplier)
            {
                float offsetY = offsetScaleMultiplier * OffsetScale;
                Vector3 pos = SpaceUtil.ScreenMap.Right + new Vector3(-DebugBorderOffset, offsetY);
                SpaceUtil.SetRightToPosition(target, pos);
            }

            SetRight(ButtonShowPowerupMenu, -50);
            SetRight(PowerupRow, -120);
            SetRight(ToggleCanGameOver, -180);

            ToggleCanGameOver.onValueChanged.AddListener(isOn => GameManager.Instance.CanGameOver = isOn);

            SetRight(ToggleGodMode, -210);
            ToggleGodMode.onValueChanged.AddListener(isOn => Player.Instance.GodMode = isOn);

            var strategiesToAdd = fireStrategies.Select(x => x.GetType().Name).ToList();
            DropdownFireType.AddOptions(strategiesToAdd);
            DropdownFireType.SetValueWithoutNotify(GameManager.Instance.DefaultFireTypeIndex);
            DropdownFireType.onValueChanged.AddListener(delegate
            {
                if (!ShouldSkipSettingFireTimeInGameManager)
                    _GameManager.SetFireType(DropdownFireType.value, skipDropDown: true, skipMessage: true, endlessTime: ShouldSetEndlessWeaponTime);
            });

            SetRight(SliderFireLevel, 35);
            SetRight(TextFireLevel, 55);
            SetRight(SliderGameSpeed, 180);
            SetRight(TextGameSpeed, 200);

            //SliderFireLevel.value = 0;
            //DebugSliderFireLevelChanged(SliderFireLevel);

            //TimeSpan frameTime = TimeSpan.FromSeconds((double)1 / (double)Application.targetFrameRate);
            //SetDebugLabel("FRAMETIME", frameTime);

            DebugSliderGameSpeedChanged(SliderGameSpeed);

            PowerupRow.Init(CurrentDebugPowerup);

#if !UNITY_EDITOR
            ToggleUI();
#endif
        }

        private bool ShouldSkipSettingFireTimeInGameManager = false;
        private bool ShouldSetEndlessWeaponTime = true;
        public void SetDropdownFiretype(int index, bool skipSetting, bool infiniteTime)
        {
            ShouldSkipSettingFireTimeInGameManager = skipSetting;
            ShouldSetEndlessWeaponTime = infiniteTime;

            DropdownFireType.value = index;

            ShouldSkipSettingFireTimeInGameManager = false;
            ShouldSetEndlessWeaponTime = true;
        }

#region Debug Input

        public void ButtonPressed(Button button)
        {
            //GameManager.ResetScene();
            //GameManager.Instance.LivesLeft = 0;
            //GameManager.Instance.TakeDamage();

            //Color newRando = DebugUtil.GetRandomPlayerColor();
            //_GameManager.RecolorPlayerActivity(newRando);

            //DebugUtil.SpawnSpecificEnemy<TankEnemy>();
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

        public void ToggleUI()
        {
            bool active = !InputField.gameObject.activeSelf;

            InputField.gameObject.SetActive(active);
            Button.gameObject.SetActive(active);
            ButtonShowPowerupMenu.gameObject.SetActive(active);
            DropdownFireType.gameObject.SetActive(active);
            SliderFireLevel.gameObject.SetActive(active);
            TextFireLevel.gameObject.SetActive(active);
            PowerupRow.gameObject.SetActive(active);
            SliderGameSpeed.gameObject.SetActive(active);
            TextGameSpeed.gameObject.SetActive(active);
            ToggleCanGameOver.gameObject.SetActive(active);
            ToggleGodMode.gameObject.SetActive(active);
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
