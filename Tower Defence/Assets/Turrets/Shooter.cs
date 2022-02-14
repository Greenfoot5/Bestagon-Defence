using UnityEngine;

namespace Turrets
{
    /// <summary>
    /// Extends DynamicTurret to add Shooting functionality.
    /// </summary>
    public class Shooter : DynamicTurret
    {
        // Bullets
        public GameObject bulletPrefab;

        /// <summary>
        /// Rotates towards the target if the turret have one.
        /// Shoots if the turret is looking towards the target
        /// </summary>
        private void Update()
        {
            // Don't do anything if the turret doesn't have a target
            if (target == null)
            {
                fireCountdown -= Time.deltaTime;
                return;
            }
        
            // Rotates the turret each frame
            LookAtTarget();

            if (!IsLookingAtTarget())
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

        /// <summary>
        /// Create the bullet and give it a target
        /// </summary>
        protected override void Attack()
        {
            // Creates the bullet
            var bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var bullet = bulletGo.GetComponent<Bullet>();
            bullet.damage = damage;
            
            // If for some reason the bullet no longer has a Bullet component
            if (bullet == null) return;
            
            // Adds the modules to the bullet
            foreach (var module in modules)
            {
                bullet.AddModule(module);
            }
            
            bullet.Seek(target);
        }
    }
}
