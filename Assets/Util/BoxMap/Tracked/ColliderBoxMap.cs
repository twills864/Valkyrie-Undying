using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Tracks the BoxMap of a given GameObject using its Collider's bounds.
    /// </summary>
    /// <inheritdoc />
    public class ColliderBoxMap : TrackedBoxMap
    {
        public Collider2D Collider { get; private set; }
        protected override Bounds Bounds => Collider.bounds;

        public ColliderBoxMap(MonoBehaviour target) : base(target)
        {
            Collider = Target.GetComponent<Collider2D>();
        }

        public ColliderBoxMap(Collider2D collider) : base(collider.gameObject.GetComponent<MonoBehaviour>())
        {
            Collider = collider;
        }
    }
}
