using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Background;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.UI.MenuElements;
using Assets.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scenes.MainMenu
{
    /// <summary>
    /// Manages the logic of the main menu.
    /// </summary>
    /// <inheritdoc/>
    public class MainMenuManager : MonoBehaviour
    {
        public static DifficultyScale SelectedDifficultyScale { get; set; } = DifficultyScale.VeryHard;

        #region Prefabs

        [SerializeField]
        private float _ButtonOffset = GameConstants.PrefabNumber;

        [SerializeField]
        private TextMeshHolder _Title = null;

        [SerializeField]
        private ButtonHolder _PlayButton = null;

        [SerializeField]
        private ButtonHolder _OptionsButton = null;

        [SerializeField]
        private LoopingBackgroundSprite _SpaceLarge = null;

        [SerializeField]
        private LoopingBackgroundSprite _SpaceSmall = null;

        [SerializeField]
        private ButtonHolder _PlayVeryEasy = null;

        [SerializeField]
        private ButtonHolder _PlayEasy = null;

        [SerializeField]
        private ButtonHolder _PlayMedium = null;

        [SerializeField]
        private ButtonHolder _PlayHard = null;

        [SerializeField]
        private ButtonHolder _PlayVeryHard = null;

        #endregion Prefabs


        #region Prefab Properties

        private float ButtonOffset => _ButtonOffset;
        private float ButtonOffsetHalf => ButtonOffset * 0.5f;

        private TextMeshHolder Title => _Title;
        private ButtonHolder PlayButton => _PlayButton;
        private ButtonHolder OptionsButton => _OptionsButton;
        private LoopingBackgroundSprite SpaceLarge => _SpaceLarge;
        private LoopingBackgroundSprite SpaceSmall => _SpaceSmall;

        private ButtonHolder PlayEasy => _PlayEasy;
        private ButtonHolder PlayVeryEasy => _PlayVeryEasy;
        private ButtonHolder PlayMedium => _PlayMedium;
        private ButtonHolder PlayHard => _PlayHard;
        private ButtonHolder PlayVeryHard => _PlayVeryHard;

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

            PlayButton.Init();
            Title.Init();

            Title.PositionY = SpaceUtil.WorldMap.Top.y - Title.BoxMap.Height;

            Vector2 buttonSize = PlayButton.ButtonSize;
            Vector2 buttonSizeHalf = buttonSize * 0.5f;

            PlayButton.PositionY = SpaceUtil.WorldMap.Center.y + buttonSizeHalf.y + ButtonOffsetHalf;
            OptionsButton.PositionY = SpaceUtil.WorldMap.Center.y - buttonSizeHalf.y - ButtonOffsetHalf;

            PlayVeryEasy.PositionY = SpaceUtil.WorldMap.Center.y + ((buttonSize.y + ButtonOffset) * 2);
            PlayEasy.PositionY = SpaceUtil.WorldMap.Center.y + (buttonSize.y + ButtonOffset);
            PlayMedium.PositionY = SpaceUtil.WorldMap.Center.y;
            PlayHard.PositionY = SpaceUtil.WorldMap.Center.y - (buttonSize.y + ButtonOffset);
            PlayVeryHard.PositionY = SpaceUtil.WorldMap.Center.y - ((buttonSize.y + ButtonOffset) * 2);

            SetPlayAndOptionButtonsEnabled(true);
            SetDifficultyButtonsEnabled(false);
        }

        private void Update()
        {
            // A Unity bug prevents us from preloading the GameScene in the Awake() method.
            //if (GameSceneLoad == null)
            //{
            //    GameSceneLoad = SceneManager.LoadSceneAsync(GameConstants.SceneNameGame, LoadSceneMode.Single);
            //    GameSceneLoad.allowSceneActivation = false;
            //}
        }

        private void SetPlayAndOptionButtonsEnabled(bool enabled)
        {
            PlayButton.gameObject.SetActive(enabled);
            OptionsButton.gameObject.SetActive(enabled);
        }

        private void SetDifficultyButtonsEnabled(bool enabled)
        {
            PlayEasy.gameObject.SetActive(enabled);
            PlayVeryEasy.gameObject.SetActive(enabled);
            PlayMedium.gameObject.SetActive(enabled);
            PlayHard.gameObject.SetActive(enabled);
            PlayVeryHard.gameObject.SetActive(enabled);
        }

        public void PlayGameButtonPressed()
        {
            SetPlayAndOptionButtonsEnabled(false);
            SetDifficultyButtonsEnabled(true);
        }

        public void OptionsButtonPressed()
        {
            if (!SceneIsLoading)
                GameSceneLoad = SceneManager.LoadSceneAsync(GameConstants.SceneNameOptions, LoadSceneMode.Single);
        }

        private void PlayDifficultyPressed(DifficultyScale selectedScale, Button button)
        {
            SelectedDifficultyScale = selectedScale;

            if (!SceneIsLoading)
            {
                GameSceneLoad = SceneManager.LoadSceneAsync(GameConstants.SceneNameGame, LoadSceneMode.Single);

                void DeactivateIfNotSelectedButton(ButtonHolder b)
                {
                    if (b.Button != button)
                        b.gameObject.SetActive(false);
                }
                DeactivateIfNotSelectedButton(PlayEasy);
                DeactivateIfNotSelectedButton(PlayVeryEasy);
                DeactivateIfNotSelectedButton(PlayMedium);
                DeactivateIfNotSelectedButton(PlayHard);
                DeactivateIfNotSelectedButton(PlayVeryHard);
            }
        }

        public void PlayVeryEasyPressed(Button button) => PlayDifficultyPressed(DifficultyScale.VeryEasy, button);
        public void PlayEasyPressed(Button button) => PlayDifficultyPressed(DifficultyScale.Easy, button);
        public void PlayMediumPressed(Button button) => PlayDifficultyPressed(DifficultyScale.Medium, button);
        public void PlayHardPressed(Button button) => PlayDifficultyPressed(DifficultyScale.Hard, button);
        public void PlayVeryHardPressed(Button button) => PlayDifficultyPressed(DifficultyScale.VeryHard, button);
    }
}
