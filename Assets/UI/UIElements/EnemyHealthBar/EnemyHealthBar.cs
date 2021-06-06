using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.UI;
using Assets.UI.UIElements.EnemyHealthBar;
using UnityEngine;

namespace Assets.UI
{
    public class EnemyHealthBar : UIElement
    {
        public static float HealthBarHeight { get; private set; }

        #region Prefabs

        [SerializeField]
        private TextMesh _TextMesh = null;

        [SerializeField]
        private EnemyStatusBarHolder _StatusBarHolder = null;

        #endregion Prefabs


        #region Prefab Properties

        private TextMesh TextMesh => _TextMesh;
        public EnemyStatusBarHolder StatusBarHolder => _StatusBarHolder;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new TextMeshColorHandler(TextMesh);

        private Color InitialColor => Color.white;
        private Color IgniteColor => new Color(1f, 0.5f, 0);

        public float Height { get; private set; }
        public float HeightHalf { get; private set; }

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
}