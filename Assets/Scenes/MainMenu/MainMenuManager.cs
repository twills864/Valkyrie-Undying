using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Background;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        #region Prefabs

        [SerializeField]
        private TitleHolder _Title = null;

        [SerializeField]
        private ButtonHolder _PlayButton = null;

        [SerializeField]
        private LoopingBackgroundSprite _SpaceLarge = null;

        [SerializeField]
        private LoopingBackgroundSprite _SpaceSmall = null;

        #endregion Prefabs


        #region Prefab Properties

        public TitleHolder Title => _Title;
        public ButtonHolder PlayButton => _PlayButton;
        public LoopingBackgroundSprite SpaceLarge => _SpaceLarge;
        public LoopingBackgroundSprite SpaceSmall => _SpaceSmall;

        #endregion Prefab Properties


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
        }

        private void Update()
        {
            // A Unity bug prevents us from preloading the GameScene in the Awake() method.
            if (GameSceneLoad == null)
            {
                GameSceneLoad = SceneManager.LoadSceneAsync(GameConstants.SceneNameGame, LoadSceneMode.Single);
                GameSceneLoad.allowSceneActivation = false;
            }
        }

        public void PlayGameButtonPressed()
        {
            GameSceneLoad.allowSceneActivation = true;
        }

        public void OptionsButtonPressed()
        {
            Debug.Log("OptionsButtonPressed()");
        }
    }
}
