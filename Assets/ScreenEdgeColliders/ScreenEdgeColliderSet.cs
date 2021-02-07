using Assets.Util;
using UnityEngine;

namespace Assets.ScreenEdgeColliders
{
    public class ScreenEdgeColliderSet : MonoBehaviour
    {
        [SerializeField]
        private ScreenEdgeCollider Top;
        [SerializeField]
        private ScreenEdgeCollider Right;
        [SerializeField]
        private ScreenEdgeCollider Bottom;
        [SerializeField]
        private ScreenEdgeCollider Left;

        /// <summary>
        /// Initializes this set of ScreenEdgeColliders to perfectly cover each edge of the visible screen.
        /// </summary>
        public void Init()
        {
            Top.Init(ScreenSide.Top);
            Right.Init(ScreenSide.Right);
            Bottom.Init(ScreenSide.Bottom);
            Left.Init(ScreenSide.Left);
        }
    }
}