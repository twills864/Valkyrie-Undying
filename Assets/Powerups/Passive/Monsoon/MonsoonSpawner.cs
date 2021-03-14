using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    public class MonsoonSpawner : ValkyrieSprite
    {
        public static MonsoonSpawner Instance { get; set; }

        public override GameTaskType TaskType => GameTaskType.Player;

        [SerializeField]
        private SpriteRenderer Sprite = null;
        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        [SerializeField]
        private float Duration = GameConstants.PrefabNumber;

        private MoveTo Move { get; set; }

        public void Init(Vector3 spawnPosition, float offsetFromBottom)
        {
            Instance = this;

            transform.position = spawnPosition;

            var x = transform.position.x;
            var y = SpaceUtil.WorldMap.Bottom.y + offsetFromBottom;
            var moveVector = new Vector3(x, y);
            Move = new MoveTo(this, moveVector, Duration);
        }

        //protected override void OnInit() { }

        protected override void OnFrameRun(float deltaTime)
        {
            if (Move.FrameRunFinishes(deltaTime))
            {
                Monsoon.Instance.Activate(transform.position.x);
                gameObject.SetActive(false);
                Move = null;
            }
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            Init(Player.Instance.transform.position, Monsoon.Instance.OffsetFromBottom);
        }
    }
}
