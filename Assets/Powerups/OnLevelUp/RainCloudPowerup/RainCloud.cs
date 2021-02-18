using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    public class RainCloud : ManagedVelocityObject
    {
        [SerializeField]
        private float Speed;

        private Vector2 Size { get; set; }
        private float BufferX;
        private TrackedBoxMap BoxMap;

        private LoopingFrameTimer FireTimer = new LoopingFrameTimer(0.1f);

        public override void OnInit()
        {
            var sprite = GetComponent<SpriteRenderer>();
            Size = sprite.size;
            BufferX = Size.x * 0.5f;
            BoxMap = new TrackedBoxMap(this);
        }

        protected override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            var targetX = Player.Instance.transform.position.x;

            var diffX = transform.position.x - targetX;

            if (Mathf.Abs(diffX) > BufferX)
            {
                if (transform.position.x < targetX)
                    VelocityX = Speed;
                else
                    VelocityX = -Speed;
            }

            if(FireTimer.UpdateActivates(deltaTime))
            {
                var pos = FirePosition();
                GameManager.Instance.CreateRaindrop(pos);
            }
        }

        public Vector2 FirePosition()
        {
            var top = BoxMap.Top;

            var x = top.x + RandomUtil.Float(-BufferX, BufferX);
            var y = top.y;

            var ret = new Vector2(x, y);
            return ret;
        }
    }
}
