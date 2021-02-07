﻿using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private TextMesh TextMesh { get; set; }

    private void Awake()
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