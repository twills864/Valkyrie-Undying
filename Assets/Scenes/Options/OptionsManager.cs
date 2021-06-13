using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Background;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Scenes.MainMenu;
using Assets.Sound;
using Assets.UI.MenuElements;
using Assets.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scenes.Options
{
    public class OptionsManager : MonoBehaviour
    {


        #region Prefabs

        [SerializeField]
        private TextMeshHolder _Title = null;

        [SerializeField]
        private ButtonHolder _BackButton = null;

        [SerializeField]
        private PercentSlider _MusicVolumeSlider = null;

        [SerializeField]
        private PercentSlider _SoundEffectVolumeSlider = null;

        [SerializeField]
        public Toggle _ToggleGore = null;

        [SerializeField]
        private LoopingBackgroundSprite _SpaceLarge = null;

        [SerializeField]
        private LoopingBackgroundSprite _SpaceSmall = null;

        [SerializeField]
        private float _OptionMargin = GameConstants.PrefabNumber;

        [SerializeField]
        private Vector2 _BackButtonMargin = Vector2.zero;

        #endregion Prefabs


        #region Prefab Properties

        private TextMeshHolder Title => _Title;
        private ButtonHolder BackButton => _BackButton;
        private PercentSlider MusicVolumeSlider => _MusicVolumeSlider;
        private PercentSlider SoundEffectVolumeSlider => _SoundEffectVolumeSlider;
        public Toggle ToggleGore => _ToggleGore;
        private LoopingBackgroundSprite SpaceLarge => _SpaceLarge;
        private LoopingBackgroundSprite SpaceSmall => _SpaceSmall;
        private float OptionMargin => _OptionMargin;
        private Vector2 BackButtonMargin => _BackButtonMargin.ScaleX(-1f);

        #endregion Prefab Properties

        private bool SceneIsLoading => GameSceneLoad != null;
        private AsyncOperation GameSceneLoad { get; set; }

        private void Awake()
        {
#if !UNITY_EDITOR
            Camera.main.orthographicSize *= 2.0f;
#endif

            SpaceUtil.Init();

            SpaceLarge.Init();
            SpaceSmall.Init();

            //BackButton.Init();
            Title.Init();

            Title.PositionY = SpaceUtil.WorldMap.Top.y - Title.BoxMap.Height;
            //BackButton.PositionY = SpaceUtil.WorldMap.Center.y;

            MusicVolumeSlider.Init();
            SoundEffectVolumeSlider.Init();

            Vector3 sliderOffset = new Vector3(0, OptionMargin);

            float musicY = Title.transform.position.y - sliderOffset.y;
            float musicX = SpaceUtil.WorldMap.Right.x - MusicVolumeSlider.WidthHalf;
            Vector3 musicPosition = new Vector3(musicX, musicY); // Title.transform.position - sliderOffset;
            MusicVolumeSlider.SetPosition(musicPosition);

            float soundEffectY = Title.transform.position.y - (sliderOffset.y * 2);
            float soundEffectX = musicPosition.x;
            Vector3 soundEffectPosition = new Vector3(soundEffectX, soundEffectY);
            SoundEffectVolumeSlider.SetPosition(soundEffectPosition);

            var canvas = ToggleGore.GetComponentInParent<Canvas>();
            canvas.transform.position = Vector3.zero;

            float toggleGoreY = Title.transform.position.y - (sliderOffset.y * 3);
            float toggleGoreX = musicPosition.x;
            Vector3 toggleGorePosition = new Vector3(toggleGoreX, toggleGoreY);
            ToggleGore.transform.position = toggleGorePosition;

            ToggleGore.isOn = PlayerPrefsUtil.GetBoolFromPrefs(PlayerPrefsUtil.ToggleGoreKey, false);


            //ToggleGore.transform.position = Vector3.zero;

            Vector3 cornerOffset = BackButtonMargin + BackButton.ButtonSize.ScaleX(-1.0f);
            BackButton.transform.position = SpaceUtil.WorldMap.BottomRight + cornerOffset;

            MusicVolumeSlider.Value = PlayerPrefs.GetInt(PlayerPrefsUtil.MusicVolumeKey, 100);
            SoundEffectVolumeSlider.Value = PlayerPrefs.GetInt(PlayerPrefsUtil.SoundEffectVolumeKey, 100);
        }

        public void BackButtonPressed()
        {
            if (!SceneIsLoading)
                SceneManager.LoadSceneAsync(GameConstants.SceneNameMainMenu, LoadSceneMode.Single);
        }

        public void MusicVolumeChanged(float value)
        {
            MusicManager.SetMusicVolume(value);
        }

        public void SoundEffectVolumeChanged(float value)
        {
            SoundManager.SetSoundEffectVolume(value);
        }

        public void ToggleGoreChanged(bool value)
        {
            PlayerPrefsUtil.SaveBoolToPrefs(PlayerPrefsUtil.ToggleGoreKey, value);
        }
    }
}
