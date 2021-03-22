using UnityEngine;

namespace Assets.Util
{
    /// <inheritdoc />
    public class ColliderBoxMap : TrackedBoxMap
    {
        public Collider2D Collider { get; private set; }
        protected override Bounds Bounds => Collider.bounds;

        public ColliderBoxMap(MonoBehaviour target) : base(target)
        {
            Collider = Target.GetComponent<Collider2D>();
        }
    }
}
