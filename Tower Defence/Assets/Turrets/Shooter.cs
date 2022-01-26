using UnityEngine;

namespace Turrets
{

    public class Shooter : DynamicTurret
    {
        // Bullets
        public GameObject bulletPrefab;

        // Update is called once per frame
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

        // Create the bullet and set the target
        protected override void Attack()
        {
            var bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var bullet = bulletGo.GetComponent<Bullet>();
            bullet.damage = damage;

            if (bullet == null) return;
            
            foreach (var module in modules)
            {
                bullet.AddModule(module);
            }
            bullet.Seek(target);
        }
    }
}
