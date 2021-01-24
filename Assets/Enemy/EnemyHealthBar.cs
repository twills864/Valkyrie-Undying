using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private TextMesh TextMesh { get; set; }

    public void Start()
    {
        Init();
    }
    public void Init()
    {
        TextMesh = GetComponent<TextMesh>();
    }

    public void SetText(int health)
    {
        SetText(health.ToString());
    }
    public void SetText(string text)
    {
        TextMesh.text = text;
    }
}
