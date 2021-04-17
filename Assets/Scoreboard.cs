using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int Score
    {
        get => _score;
        set
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
