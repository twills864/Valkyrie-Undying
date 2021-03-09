using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.UI;
using UnityEngine;

public class EnemyHealthBar : UIElement
{
    public static float HealthBarHeight { get; private set; }

    protected override ColorHandler DefaultColorHandler()
        => new TextMeshColorHandler(TextMesh);

    private Color InitialColor => Color.white;
    private Color IgniteColor => new Color(1f, 0.5f, 0);

    [SerializeField]
    private TextMesh TextMesh;

    public float Height { get; private set; }
    public float HeightHalf { get; private set; }

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
        SpriteColor = InitialColor;
    }

    public void Ignite()
    {
        SpriteColor = IgniteColor;
    }

    public static void StaticInit()
    {
        var healthBar = PoolManager.Instance.UIElementPool.Get<EnemyHealthBar>();
        HealthBarHeight = healthBar.Height;
        healthBar.DeactivateSelf();
    }
}
