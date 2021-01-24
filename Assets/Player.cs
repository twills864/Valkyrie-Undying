using Assets.Util;
using Assets.Util.AssetsDebug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class Player : MonoBehaviour
    {
        Rigidbody2D Body => GetComponent<Rigidbody2D>();
        Renderer Renderer => GetComponent<Renderer>();

        public Vector2 Size => Renderer.bounds.size;
        private TrackedBoxMap BoxMap { get; set; }


        [SerializeField]
        private float MobileYOffset = 100;
        private static float MobileY;

        private void Start()
        {
            BoxMap = new TrackedBoxMap(this);

            DebugUI.SetDebugLabel("BoxMapTop", () => BoxMap.Top);
            DebugUI.SetDebugLabel("FirePosition", () => FirePosition());
            DebugUI.SetDebugLabel("PlayerPosition", () => transform.position);
            DebugUI.SetDebugLabel("PlayerLocalPosition", () => transform.localPosition);
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
                SetMobilePosition(SpaceUtil.WorldPositionUnderMouse());
            else if (Input.GetMouseButton(1))
                SetPosition(SpaceUtil.WorldPositionUnderMouse());
        }

        public void Init()
        {
            var targetY = Camera.main.ScreenToWorldPoint(new Vector3(0, MobileYOffset));
            MobileY = targetY.y;

            SetMobilePosition(Vector2.zero);
        }

        public void SetMobilePosition(Vector2 pos)
        {
            Vector2 newPos = new Vector2(pos.x, MobileY);
            SetPosition(newPos);
        }
        public void SetPosition(Vector2 pos)
        {
            Body.transform.localPosition = pos;
        }

        public Vector2 FirePosition()
        {
            var ret = BoxMap.Top;
            return ret;
        }
    }
}