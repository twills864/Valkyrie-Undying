using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #endregion Prefabs


        #region Prefab Properties

        public TitleHolder Title => _Title;
        public ButtonHolder PlayButton => _PlayButton;

        #endregion Prefab Properties


        private AsyncOperation GameSceneLoad { get; set; }

        private void Awake()
        {
            SpaceUtil.Init();
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
