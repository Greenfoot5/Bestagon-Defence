﻿using System;
using Abstract.Data;
using Gameplay;
using Modules;
using UI.Nodes;
using UnityEngine;

namespace Turrets.Gunner
{
    /// <summary>
    /// Extends DynamicTurret to add Shooting functionality.
    /// </summary>
    public class Gunner : DynamicTurret
    {
        // Bullets
        [Tooltip("The bullet prefab to spawn each attack")]
        [SerializeField]
        private GameObject bulletPrefab;
        
        // Spin up stats
        private float _fireRateIncrease = 1f;
        private float _oldIncrease = 1f;
        
        [Tooltip("That amount to increase the fireRate by each attack")]
        public UpgradableStat spinMultiplier = new UpgradableStat(1.1f);
        [Tooltip("How much to divide the fire rate when not attacking")]
        public UpgradableStat spinCooldown = new UpgradableStat(1.08f);
        [Tooltip("The maximum fire rate increase the gunner can reach without modules")]
        public UpgradableStat maxFireRate = new UpgradableStat(6.5f);

        /// <summary>
        /// Rotates towards the target if the turret have one.
        /// Shoots if the turret is looking towards the target
        /// </summary>
        private void Update()
        {
            // Don't do anything if the turret doesn't have a target
            if (target == null)
            {
                if (fireCountdown <= 0f)
                {
                    UpdateFireRate(false);
                    fireCountdown = 1 / fireRate.GetStat();
                }
                
                fireCountdown -= Time.deltaTime;

                return;
            }
        
            // Rotates the turret each frame
            LookAtTarget();

            if (!IsLookingAtTarget())
            {
                if (fireCountdown <= 0f)
                {
                    UpdateFireRate(false);
                    fireCountdown = 1 / fireRate.GetStat();
                }

                fireCountdown -= Time.deltaTime;
                return;
            }
            
            
            if (fireCountdown <= 0)
            {
                UpdateFireRate(true);
                
                fireCountdown = 1 / fireRate.GetStat();
                
                Attack();
            }

            fireCountdown -= Time.deltaTime;
        }
        
        /// <summary>
        /// Updates the fire rate so there's no duplicated code in Attack().
        /// Also handles all edge cases with the increase being too low or high
        /// </summary>
        /// <param name="isIncrease">To increase or decrease the fireRate</param>
        private void UpdateFireRate(bool isIncrease)
        {
            if (isIncrease)
            {
                _fireRateIncrease *= spinMultiplier.GetStat();
                if (_fireRateIncrease > maxFireRate.GetStat())
                {
                    _fireRateIncrease = maxFireRate.GetStat();
                }
            }
            else
            {
                _fireRateIncrease /= spinCooldown.GetStat();
                if (_fireRateIncrease < 1f)
                {
                    _fireRateIncrease = 1f;
                }
            }
            
            // Check the old and new values are actually different
            if (Math.Abs(_fireRateIncrease - _oldIncrease) > 0.0001)
            {
                fireRate.TakeModifier(_oldIncrease - 1);
                fireRate.AddModifier(_fireRateIncrease - 1);
                _oldIncrease = _fireRateIncrease;
            }
            
            // Update the stats of the turret if it's selected
            if (NodeUI.instance.GetTurret() != null && NodeUI.instance.GetTurret() == gameObject)
            {
                NodeUI.instance.UpdateStats();
            }
        }

        /// <summary>
        /// Create the bullet and give it a target
        /// </summary>
        protected override void Attack()
        {
            // Creates the bullet
            GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletGo.name = "_" + bulletGo.name;
            var bullet = bulletGo.GetComponent<Bullet>();
            bullet.damage = damage;
            
            // If for some reason the bullet no longer has a Bullet component
            if (bullet == null) return;
            
            // Adds the modules to the bullet
            foreach (Module module in modules)
            {
                bullet.AddModule(module);
            }
            
            bullet.Seek(target, this);
        }
    }
}
