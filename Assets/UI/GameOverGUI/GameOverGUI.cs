using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI
{
    public class GameOverGUI : MonoBehaviour
    {
        #region Prefab Fields

        [SerializeField]
        private Text _GameOverLabel = null;

        [SerializeField]
        private Text _YourScoreLabel = null;

        [SerializeField]
        private Text _HighScoreLabel = null;

        [SerializeField]
        private Button _PlayAgainButton = null;

        #endregion Prefab Fields


        #region Prefab Properties

        private Text GameOverLabel => _GameOverLabel;
        public Text YourScoreLabel => _YourScoreLabel;
        public Text HighScoreLabel => _HighScoreLabel;
        public Button PlayAgainButton => _PlayAgainButton;

        #endregion Prefab Properties

        public void Init()
        {
#if !UNITY_EDITOR
            int scale = (int) DebugUI.MobileGuiScale;

            void FixlLabel(Text text)
            {
                text.fontSize *= scale;

                var textTransform = (RectTransform)text.transform;
                textTransform.anchoredPosition = textTransform.anchoredPosition.ScaleY(scale);
            }

            FixlLabel(GameOverLabel);
            FixlLabel(YourScoreLabel);
            FixlLabel(HighScoreLabel);

            var buttonTransform = (RectTransform)PlayAgainButton.transform;
            buttonTransform.sizeDelta *= scale;
            buttonTransform.anchoredPosition = buttonTransform.anchoredPosition.ScaleY(scale);

            var buttonTextTransform = PlayAgainButton.transform.GetChild(0);
            var buttonText = buttonTextTransform.GetComponent<Text>();
            buttonText.fontSize *= scale;
#endif

        }

        public void Activate(int score, int highScore)
        {
            gameObject.SetActive(true);

            YourScoreLabel.text = $"Your score: {score.ToString()}";
            HighScoreLabel.text = $"High score: {highScore.ToString()}";
        }

        public void PlayAgainPressed()
        {
            GameManager.ResetScene();
        }
    }
}
