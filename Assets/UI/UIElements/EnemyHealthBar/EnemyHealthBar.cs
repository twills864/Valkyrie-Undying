using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Statuses;
using Assets.UI;
using Assets.UI.UIElements.EnemyHealthBar;
using Assets.Util;
using UnityEditor;
using UnityEngine;

namespace Assets.UI
{
    public class EnemyHealthBar : UIElement
    {
        public static float HealthBarHeight { get; private set; }

        #region Prefabs

        [SerializeField]
        private TextMesh _TextMesh;

        #endregion Prefabs


        #region Prefab Properties

        public TextMesh TextMesh => _TextMesh;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new TextMeshColorHandler(TextMesh);

        public EnemyStatusBarHolder StatusBarHolder { get; private set; }

        private Color InitialColor => Color.white;
        private Color IgniteColor => new Color(1f, 0.5f, 0);

        public float Height { get; private set; }
        public float HeightHalf { get; private set; }

        protected override void OnUIElementInit()
        {
            StatusBarHolder = PoolManager.Instance.UIElementPool.Get<EnemyStatusBarHolder>();
            StatusBarHolder.HealthBar = this;

            if(!this.IsOriginalPrefab)
                StatusBarHolder.transform.parent = transform;

            var sprite = gameObject.GetComponent<Renderer>();
            Height = sprite.bounds.size.y;
            HeightHalf = Height * 0.5f;

            StatusBarHolder.Init();
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

        public override void OnSpawn()
        {
            StatusBarHolder.OnSpawn();
        }

        public void Ignite(BurningStatus status)
        {
            StatusBarHolder.BurningDamage = status.Power;
        }

        public void AddPoison(PoisonedStatus status)
        {
            StatusBarHolder.PoisonDamage = status.Power;
        }

        public void AddParasites(ParasiteStatus status)
        {
            StatusBarHolder.ParasiteDamage = status.Power;
        }

        public void AddAcid(AcidicStatus status)
        {
            StatusBarHolder.AcidDamage = status.Power;
        }

        public void AddChill(ChilledStatus status)
        {
            StatusBarHolder.ChilledTime = status.Power;
        }

        public static void StaticInit()
        {
            var healthBar = PoolManager.Instance.UIElementPool.Get<EnemyHealthBar>();
            HealthBarHeight = healthBar.Height;
            healthBar.DeactivateSelf();
        }
    }
}