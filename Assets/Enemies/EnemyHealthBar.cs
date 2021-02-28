using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private Color InitialColor => Color.white;
    private Color IgniteColor => new Color(1f, 0.5f, 0);

    private TextMesh TextMesh { get; set; }

    private Color _Color
    {
        get => TextMesh.color;
        set
        {
            if (TextMesh.color != value)
                TextMesh.color = value;
        }
    }

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

    public void OnActivate()
    {
        _Color = InitialColor;
    }

    public void Ignite()
    {
        _Color = IgniteColor;
    }
}
