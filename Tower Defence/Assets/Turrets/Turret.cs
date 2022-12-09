using System.Collections.Generic;
using Abstract.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
    
    /// <summary>
    /// The base Turret class that can be extended to add other turret types
    /// </summary>
    public abstract class Turret : MonoBehaviour
    {
        [Tooltip("How much damage the turret deals")]
        public UpgradableStat damage;

        // System
        [Tooltip("What GameObject tag the turret targets")]
        public string enemyTag = "Enemy";
        
        [Tooltip("The range of the turret")]
        public UpgradableStat range = new UpgradableStat(2.5f);
        [Tooltip("The shader that display's the turret's range when clicked")]
        [SerializeField]
        private GameObject rangeDisplay;

        // Attack speed
        [Tooltip("How many times per second the turret attacks")]
        public UpgradableStat fireRate = new UpgradableStat(1f);
        /// <summary> How long left until the next attack </summary>
        public float fireCountdown;

        // Modules
        [FormerlySerializedAs("modules")]
        [Tooltip("Which modules the turret has applied")]
        [SerializeField]
        public List<ModuleChainHandler> moduleHandlers = new List<ModuleChainHandler>();
        
        [Tooltip("What events to run when starting")]
        [SerializeField]
        private UnityEvent awakeEvents;
        
        /// <summary>
        /// Stops the range displaying
        /// </summary>
        private void Awake()
        {
            rangeDisplay.SetActive(false);
            awakeEvents.Invoke();
        }
        
        /// <summary>
        /// Turret types will override this as attack type will be different for each turret
        /// </summary>
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
        /// <param name="handler">The ModuleChainHandler to apply to the turret</param>
        /// <returns>true If the Module was applied successfully</returns>
        public bool AddModule(ModuleChainHandler handler)
        {
            if (!handler.GetModule().ValidModule(this))
            {
                //Debug.Log("Invalid Module Applied");
                return false;
            }

            handler = CalculateUpgrades(handler);
            
            moduleHandlers.Add(handler);
            handler.GetModule().AddModule(this);

            // Update the range shader's size
            Vector3 localScale = transform.localScale;
            rangeDisplay.transform.localScale = new Vector3(
                range.GetStat() / localScale.x * 2,
                range.GetStat() / localScale.y * 2,
                1);
            return true;
        }
        
        /// <summary>
        /// Performs any module upgrades that are possible with the addition of a new handler
        /// </summary>
        /// <param name="handler">The handler to check for upgrades against</param>
        private ModuleChainHandler CalculateUpgrades(ModuleChainHandler handler)
        {
            var i = 0;
            while (i < moduleHandlers.Count)
            {
                bool canUpgrade = handler.Upgrade(moduleHandlers[i].GetTier());
                if (canUpgrade)
                {
                    Debug.Log("Upgrading!");
                    RemoveModule(moduleHandlers[i]);
                    handler = CalculateUpgrades(handler);
                    break;
                }

                i++;
            }

            return handler;
        }
        
        /// <summary>
        /// Removes a module from the turret
        /// </summary>
        /// <param name="handler">The handler of the module to remove</param>
        public void RemoveModule(ModuleChainHandler handler)
        {
            moduleHandlers.Remove(handler);
            handler.GetModule().RemoveModule(this);
            Debug.Log("Removing: " + handler.GetModule().displayName);
            // Update the range shader's size
            Vector3 localScale = transform.localScale;
            rangeDisplay.transform.localScale = new Vector3(
                range.GetStat() / localScale.x * 2,
                range.GetStat() / localScale.y * 2,
                1);
        }
        
        /// <summary>
        /// Called when the turret is selected, displays the turret's range
        /// </summary>
        public void Selected()
        {
            // Update the range shader's size
            Vector3 localScale = transform.localScale;
            rangeDisplay.transform.localScale = new Vector3(
                range.GetStat() / localScale.x * 2,
                range.GetStat() / localScale.y * 2,
                1);
            rangeDisplay.SetActive(true);
        }
        
        /// <summary>
        /// Called when the turret is deselected, disables the turret's range view.
        /// </summary>
        public void Deselected()
        {
            rangeDisplay.SetActive(false);
        }
    }
}
