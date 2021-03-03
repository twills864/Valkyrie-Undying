using Assets.ObjectPooling;
using Assets.UI;
using UnityEngine;

public class EnemyHealthBar : UIElement
{
    public static float HealthBarHeight { get; private set; }

    private Color InitialColor => Color.white;
    private Color IgniteColor => new Color(1f, 0.5f, 0);

    private TextMesh TextMesh { get; set; }

    public float Height { get; private set; }
    public float HeightHalf { get; private set; }

    private Color _Color
    {
        get => TextMesh.color;
        set
        {
            if (TextMesh.color != value)
                TextMesh.color = value;
        }
    }

    public float Alpha
    {
        get => _Color.a;
        set
        {
            var color = _Color;
            color.a = value;
            _Color = color;
        }
    }

    private void Awake()
    {
        TextMesh = GetComponent<TextMesh>();
    }

    protected override void OnUIElementInit()
    {
        var sprite = gameObject.GetComponent<Renderer>();
        Height = sprite.bounds.size.y;
        HeightHalf = Height * 0.5f;
    }

    public void SetText(int health)
    {
        SetText(health.ToString());
    }
    public void SetText(string text)
    {
        TextMesh.text = text;
    }

    protected override void OnActivate()
    {
        _Color = InitialColor;
    }

    public void Ignite()
    {
        _Color = IgniteColor;
    }

    public static void InitStatic()
    {
        var healthBar = PoolManager.Instance.UIElementPool.Get<EnemyHealthBar>();
        HealthBarHeight = healthBar.Height;
        healthBar.DeactivateSelf();
    }
}
