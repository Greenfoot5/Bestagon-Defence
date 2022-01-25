using System.Collections.Generic;
using Turrets.Modules;
using UnityEngine;

namespace Turrets
{
    /// <summary>
    /// The various targeting methods a turret can use to find a target
    /// </summary>
    public enum TargetingMethod
    {
        Closest = 0,
        Weakest,
        Strongest,
        First,
        Last
    }
    
    public abstract class Turret : MonoBehaviour
    {
        public UpgradableStat damage;

        // System
        public string enemyTag = "Enemy";
        
        public UpgradableStat range = new UpgradableStat(2.5f);
        public GameObject rangeDisplay;

        // Attack speed
        [Tooltip("Time between each shot")]
        public UpgradableStat fireRate = new UpgradableStat(1f);
        protected float fireCountdown;
        
        // Effects
        public float slowPercentage;
        
        // Modules
        public List<Module> Modules = new List<Module>();

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
            Gizmos.DrawWireSphere(transform.position, range.GetStat());
        }
        
        /// <summary>
        /// Adds Modules to our turret after checking they're valid.
        /// </summary>
        /// <param name="Module">The Module to apply to the turret</param>
        /// <returns>true If the Module was applied successfully</returns>
        public bool AddModule(Module Module)
        {
            if (!Module.ValidModule(this))
            {
                Debug.Log("Invalid Module");
                return false;
            }
            
            // TODO - Add as difficulty modifier
            // Module oldModule = null;
            // // Check if we already have an Module of the same type
            // foreach (var turretModule in Modules.Where(turretModule => turretModule.GETModuleType() == Module.GETModuleType()))
            // {
            //     // If it's of a higher level, remove the current level
            //     if (turretModule.ModuleTier < Module.ModuleTier)
            //     {
            //         Debug.Log("Removing lower level Module");
            //         oldModule = turretModule;
            //     }
            //     // If it's of a lower level, we can't Module
            //     else
            //     {
            //         Debug.Log("This turret already has an Module of the same type at" +
            //                   " the same level or better!");
            //         return false;
            //     }
            // }
            //
            // if (oldModule != null)
            // {
            //     oldModule.RemoveModule(this);
            //     Modules.Remove(oldModule);
            // }

            Module.AddModule(this);
            Modules.Add(Module);
            
            // Update the range shader's size
            var localScale = transform.localScale;
            rangeDisplay.transform.localScale = new Vector3(
                range.GetStat() / localScale.x * 2,
                range.GetStat() / localScale.y * 2,
                1);
            return true;
        }

        public void Selected()
        {
            // Update the range shader's size
            var localScale = transform.localScale;
            rangeDisplay.transform.localScale = new Vector3(
                range.GetStat() / localScale.x * 2,
                range.GetStat() / localScale.y * 2,
                1);
            rangeDisplay.SetActive(true);
        }

        public void Deselected()
        {
            rangeDisplay.SetActive(false);
        }
    }
}
