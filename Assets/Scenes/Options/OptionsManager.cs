using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Background;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Scenes.MainMenu;
using Assets.UI.MenuElements;
using Assets.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private LoopingBackgroundSprite SpaceLarge => _SpaceLarge;
        private LoopingBackgroundSprite SpaceSmall => _SpaceSmall;
        private float OptionMargin => _OptionMargin;
        private Vector2 BackButtonMargin => VectorUtil.ScaleX2(_BackButtonMargin, -1f);

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

            Vector3 musicPosition = Title.transform.position - sliderOffset;
            MusicVolumeSlider.SetPosition(musicPosition);
            SoundEffectVolumeSlider.SetPosition(musicPosition - sliderOffset);

            Vector3 cornerOffset = BackButtonMargin + VectorUtil.ScaleX2(BackButton.ButtonSize, -1.0f);
            BackButton.transform.position = SpaceUtil.WorldMap.BottomRight + cornerOffset;
        }

        public void BackButtonPressed()
        {
            if (!SceneIsLoading)
                SceneManager.LoadSceneAsync(GameConstants.SceneNameMainMenu, LoadSceneMode.Single);
        }

        private void Update()
        {

        }

        public void MusicVolumeChanged(float value)
        {
            Debug.Log($"[MusicVolumeChanged] {value}");
        }

        public void SoundEffectVolumeChanged(float value)
        {
            Debug.Log($"[SoundEffectVolumeChanged] {value}");
        }
    }
}
