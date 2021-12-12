using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;

namespace Turrets
{

    public class Smasher : Turret
    {
        public ParticleSystem smashEffect;

        // Update is called once per frame
        private void Update()
        {
            // Don't do anything if no enemy is in range
            if (!Physics2D.OverlapCircleAll(transform.position, range.GetStat()).Any(x => 
                x.CompareTag(enemyTag)))
            {
                fireCountdown -= Time.deltaTime;
                return;
            }

            if (fireCountdown <= 0)
            {
                Attack();
                fireCountdown = 1 / fireRate.GetStat();
            }
            
            fireCountdown -= Time.deltaTime;
        }

        protected override void Attack()
        {
            smashEffect.Play();
            
            // Get's all the enemies in the AoE and calls Damage on them
            var colliders = Physics2D.OverlapCircleAll(transform.position, range.GetStat());
            var enemies = new List<Enemy>();
            foreach (var collider2d in colliders)
            {
                if (!collider2d.CompareTag(enemyTag)) continue;
                
                var enemy = collider2d.GetComponent<Enemy>();
                enemies.Add(enemy);
                enemy.TakeDamage(damage.GetStat());
            }

            foreach (var upgrade in upgrades)
            {
                upgrade.OnHit(enemies.ToArray());
            }
        }
    }
}
