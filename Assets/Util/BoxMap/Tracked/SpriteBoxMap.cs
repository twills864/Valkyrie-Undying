using UnityEngine;

namespace Assets.Util
{
    /// <inheritdoc />
    public class SpriteBoxMap : TrackedBoxMap
    {
        private Renderer Sprite { get; }
        protected override Bounds Bounds => Sprite.bounds;

        public SpriteBoxMap(MonoBehaviour target) : base(target)
        {
            Sprite = Target.GetComponent<Renderer>();
        }

        public SpriteBoxMap(MonoBehaviour target, Renderer renderer) : base(target)
        {
            Sprite = renderer;
        }
    }
}
