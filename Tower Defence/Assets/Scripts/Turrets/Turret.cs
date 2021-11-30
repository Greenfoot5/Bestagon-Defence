using System;
using System.Collections.Generic;
using System.Linq;
using Turrets.Upgrades;
using UnityEngine;

namespace Turrets
{
    /// <summary>
    /// The various targeting methods a turret can use to find a target
    /// </summary>
    public enum TargetingMethod
    {
        Closest = 0,
        Weakest = 1,
        Strongest = 2
    }
    
    public abstract class Turret : MonoBehaviour
    {
        public float damage;

        // System
        public string enemyTag = "Enemy";
        
        public float range = 2.5f;
        public GameObject rangeDisplay;

        // Attack speed
        [Tooltip("Time between each shot")]
        public float fireRate = 1f;
        protected float fireCountdown;
        
        // Effects
        public float slowPercentage;
        
        // Upgrades
        public List<Upgrade> upgrades = new List<Upgrade>();

        private void Awake()
        {
            rangeDisplay.SetActive(false);
        }

        protected abstract void Attack();

        /// <summary>
        /// Allows the editor to display the range of the turret
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
        
        /// <summary>
        /// Adds upgrades to our turret after checking they're valid.
        /// </summary>
        /// <param name="upgrade">The upgrade to apply to the turret</param>
        /// <returns>true If the upgrade was applied successfully</returns>
        public bool AddUpgrade(Upgrade upgrade)
        {
            if (!upgrade.ValidUpgrade(this))
            {
                Debug.Log("Invalid Upgrade");
                return false;
            }

            upgrade.AddUpgrade(this);
            upgrades.Add(upgrade);
            return true;
        }

        public void Selected()
        {
            rangeDisplay.SetActive(true);
        }

        public void Deselected()
        {
            rangeDisplay.SetActive(false);
        }

        public virtual void OnInspectorGUI() { }
    }
}
