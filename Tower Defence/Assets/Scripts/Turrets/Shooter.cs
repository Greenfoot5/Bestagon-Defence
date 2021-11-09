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
            // Don't do anything if we don't have a target
            if (_target == null)
            {
                _fireCountdown -= Time.deltaTime;
                return;
            }
        
            // Rotates the turret each frame
            LookAtTarget();

            if (!IsLookingAtTarget())
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

        // Create the bullet and set the target
        protected override void Attack()
        {
            var bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var bullet = bulletGO.GetComponent<Bullet>();
            bullet.damage = damage;

            if (bullet == null) return;
            
            foreach (var upgrade in upgrades)
            {
                bullet.AddUpgrade(upgrade);
            }
            bullet.Seek(_target);
        }
    }
}
