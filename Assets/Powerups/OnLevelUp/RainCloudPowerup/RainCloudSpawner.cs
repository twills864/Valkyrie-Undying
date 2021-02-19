using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    public class RainCloudSpawner : GameTaskRunner
    {
        public static RainCloudSpawner Instance { get; set; }

        [SerializeField]
        private float Duration;

        private MoveTo Move;
        public override GameTaskType TaskType => GameTaskType.Player;

        public void Init(Vector2 spawnPosition, float offsetFromBottom)
        {
            transform.position = spawnPosition;

            var x = transform.position.x;
            var y = SpaceUtil.WorldMap.Bottom.y + offsetFromBottom;
            var moveVector = new Vector2(x, y);
            Move = new MoveTo(this, moveVector, Duration);
            StartTask(Move);
        }

        public override void OnInit() { }

        protected override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            //Move.RunFrame(deltaTime);
            if (Move.IsFinished)
            {
                RainCloud.Instance.Activate(transform.position.x);
                gameObject.SetActive(false);
                //Destroy(this);
            }
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            Init(Player.Instance.transform.position, RainCloud.Instance.OffsetFromBottom);
        }
    }
}
