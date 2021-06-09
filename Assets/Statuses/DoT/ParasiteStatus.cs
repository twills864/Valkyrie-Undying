using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Statuses
{
    public class ParasiteStatus : DamageOverTimeStatus
    {
        public ParasiteStatus(Enemy target) : base(target)
        {

        }

        // Parasite damage doesn't change as a result of time.
        public override int GetAndUpdateDamage() => Damage;

        protected override void UpdateStatusBar()
        {
            Target.HealthBar.AddParasites(this);
        }

        public void AddParasites(int numberParasites)
        {
            Damage += numberParasites;
            UpdateStatusBar();
        }

        public void OnDeath()
        {
            if (IsActive)
            {
                Vector3 spawnPosition = Target.transform.position;
                int numberParasites = Damage;

                GameManager.Instance.AddParasites(spawnPosition, numberParasites);
            }
        }
    }
}
