using System.Collections.Generic;
using UnityEngine;

namespace Turrets
{

    public class Smasher : Turret
    {
        public ParticleSystem smashEffect;

        // Update is called once per frame
        private void Update()
        {
            // Don't do anything if we don't have a target
            if (_target == null)
            {
                _fireCountdown -= Time.deltaTime;
                return;
            }

            if (_fireCountdown <= 0)
            {
                Attack();
                _fireCountdown = 1 / fireRate;
            }
            
            _fireCountdown -= Time.deltaTime;
        }

        protected override void Attack()
        {
            smashEffect.Play();
            
            // Get's all the enemies in the AoE and calls Damage on them
            var colliders = Physics2D.OverlapCircleAll(transform.position, range);
            var enemies = new List<Enemy>();
            foreach (var collider2d in colliders)
            {
                if (!collider2d.CompareTag("Enemy")) continue;
                
                var enemy = collider2d.GetComponent<Enemy>();
                enemies.Add(enemy);
                enemy.TakeDamage(damage);
            }

            foreach (var upgrade in upgrades)
            {
                upgrade.OnHit(enemies.ToArray());
            }
        }
    }
}
