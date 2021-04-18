using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Pickups
{
    public class OneUpPickup : EnemyLootPickup
    {
        protected override void OnPickUp()
        {
            GameManager.Instance.LivesLeft++;
            Player.Instance.CreateFleetingTextAtCenter("One up!").SpriteColor = Color.green;
        }
    }
}
