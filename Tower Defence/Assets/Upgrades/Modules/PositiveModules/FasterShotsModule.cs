using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Modules
{
    [CreateAssetMenu(fileName = "FasterShotsT0", menuName = "Modules/Faster Shots")]
    public class FasterShotsModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter) };

        [SerializeField]
        private float percentageChange;
        public override void AddModule(Turret turret) { }

        public override void RemoveModule(Turret turret) { }

        public override void OnShoot(Bullet bullet)
        {
            bullet.speed.AddModifier(percentageChange);
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}