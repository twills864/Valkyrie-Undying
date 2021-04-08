using Assets.Util;
using UnityEngine;

namespace Assets.ScreenEdgeColliders
{
    public class ScreenEdgeColliderSet : MonoBehaviour
    {
        #region Prefabs

        [SerializeField]
        private ScreenEdgeCollider _Top = null;
        [SerializeField]
        private ScreenEdgeCollider _Right = null;
        [SerializeField]
        private ScreenEdgeCollider _Bottom = null;
        [SerializeField]
        private ScreenEdgeCollider _Left = null;

        #endregion Prefabs

        #region Prefab Properties

        private ScreenEdgeCollider Top => _Top;
        private ScreenEdgeCollider Right => _Right;
        private ScreenEdgeCollider Bottom => _Bottom;
        private ScreenEdgeCollider Left => _Left;

        #endregion Prefab Properties

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