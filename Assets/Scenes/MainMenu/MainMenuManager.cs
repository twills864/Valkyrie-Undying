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

namespace Assets.Scenes.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        #region Prefabs

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

        #endregion Prefabs


        #region Prefab Properties

        public TextMeshHolder Title => _Title;
        public ButtonHolder PlayButton => _PlayButton;
        public ButtonHolder OptionsButton => _OptionsButton;
        public LoopingBackgroundSprite SpaceLarge => _SpaceLarge;
        public LoopingBackgroundSprite SpaceSmall => _SpaceSmall;

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
            PlayButton.PositionY = SpaceUtil.WorldMap.Center.y;
            OptionsButton.PositionY = (SpaceUtil.WorldMap.Center.y + SpaceUtil.WorldMap.Bottom.y) * 0.5f;
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

        public void PlayGameButtonPressed()
        {
            //GameSceneLoad.allowSceneActivation = true;
            if(!SceneIsLoading)
                GameSceneLoad = SceneManager.LoadSceneAsync(GameConstants.SceneNameGame, LoadSceneMode.Single);
        }

        public void OptionsButtonPressed()
        {
            if (!SceneIsLoading)
                SceneManager.LoadSceneAsync(GameConstants.SceneNameOptions, LoadSceneMode.Single);
        }
    }
}
