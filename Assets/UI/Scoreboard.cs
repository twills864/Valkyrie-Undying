using System.Collections;
using System.Collections.Generic;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <summary>
    /// The label that displays how many points the player currently has.
    /// </summary>
    /// <inheritdoc/>
    public class Scoreboard : MonoBehaviour
    {
        public static Scoreboard Instance { get; set; }

        #region Property Fields

        private int _score;

        #endregion Property Fields

        #region Prefabs

        [SerializeField]
        private TextMesh _Label = null;

        #endregion Prefabs


        #region Prefab Properties

        private TextMesh Label => _Label;

        #endregion Prefab Properties

        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                Label.text = value.ToString("00000000");
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        public void Init()
        {
            Score = 0;

            var textRenderer = Label.GetComponent<Renderer>();
            Vector2 size = textRenderer.bounds.size;

            Vector3 newPos = SpaceUtil.WorldMap.Top;
            newPos.y -= size.y * 0.5f;

            transform.position = newPos;
        }

        public void AddScore(int score)
        {
            Score += score;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}