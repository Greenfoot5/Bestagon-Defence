using UnityEngine;

namespace Turrets
{

    public class Laser : DynamicTurret
    {
        // Lasers
        public LineRenderer lineRenderer;
        public ParticleSystem impactEffect;

        // Update is called once per frame
        private void Update()
        {
            // Don't do anything if we don't have a target
            if (target == null)
            {
                if (!lineRenderer.enabled) return;
                
                lineRenderer.enabled = false;
                impactEffect.Stop();
                return;
            }
        
            // Rotates the turret each frame
            LookAtTarget();

            if (!IsLookingAtTarget())
            {
                if (!lineRenderer.enabled) return;
                
                lineRenderer.enabled = false;
                impactEffect.Stop();
                return;
            }

            Attack();
        }

        // TODO - Animate the laser slightly (make it pulse)
        protected override void Attack()
        {
            // Deal damage
            targetEnemy.TakeDamage(damage.GetStat() * Time.deltaTime, gameObject);

            foreach (var Module in Modules)
            {
                Module.OnHit(new [] {targetEnemy});
            }

            // Enable visuals
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
                impactEffect.Play();
            }
        
            // Set Laser positions
            var targetPosition = target.position;
            var firePointPosition = firePoint.position;
            lineRenderer.SetPosition(0, firePointPosition);
            lineRenderer.SetPosition(1, targetPosition);

            // Set impact effect rotation
            var impactEffectTransform = impactEffect.transform;
            var aimDir = (Vector3)((Vector2)firePointPosition - (Vector2)impactEffectTransform.position).normalized;
            impactEffectTransform.rotation = Quaternion.LookRotation(aimDir);

            // Set impact effect position
            impactEffectTransform.position = targetPosition + aimDir * 0.2f;
        }
    }
}
