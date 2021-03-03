using UnityEngine;

namespace Assets.Util
{
    /// <inheritdoc />
    public class ColliderBoxMap : TrackedBoxMap
    {
        private Collider2D Collider { get; }
        protected override Bounds Bounds => Collider.bounds;

        public ColliderBoxMap(MonoBehaviour target) : base(target)
        {
            Collider = Target.GetComponent<Collider2D>();
        }
    }
}
