using Abstract.Managers;
using Turrets;
using Turrets.Blueprints;
using Turrets.Modules;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Levels
{
    /// <summary>
    /// Manages all data and actions for a single node on a level map
    /// </summary>
    public class Node : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // The colour to set out node to
        public Color hoverColour;
        public Color cantAffordColour;
        private Color _defaultColour;
        
        // Turret info
        [HideInInspector]
        public GameObject turret;
        [HideInInspector]
        public TurretBlueprint turretBlueprint;
    
        private Renderer _rend;
        private BuildManager _buildManager;

        private void Start()
        {
            _rend = GetComponent<Renderer>();
            _defaultColour = _rend.material.color;
            _buildManager = BuildManager.instance;
        }

        /// <summary>
        /// Places the turret on the node
        /// </summary>
        /// <param name="blueprint">The blueprint of the turret to build</param>
        private void BuildTurret(TurretBlueprint blueprint)
        {
            // Spawn the turret and set the turret and blueprint
            var nodePosition = transform.position;
            var newTurret = Instantiate(blueprint.prefab, nodePosition, Quaternion.identity);
            turret = newTurret;
            var turretClass = turret.GetComponent<Turret>();
            turretBlueprint = blueprint;
        
            foreach (var turretModule in blueprint.Modules)
            {
                turretClass.AddModule(turretModule);
            }
        
            // Spawn the build effect and destroy after
            var effect = Instantiate(_buildManager.buildEffect, nodePosition, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        }
    
        /// <summary>
        /// Called when upgrading a turret
        /// </summary>
        /// <param name="module">The Module to add to the turret</param>
        /// <returns>If the Module was applied</returns>
        public bool ModuleTurret(Module module)
        {
            // Apply the Module
            var appliedModule = turret.GetComponent<Turret>().AddModule(module);
            if (!appliedModule) return false;

            // Spawn the build effect
            var effect = Instantiate(_buildManager.buildEffect, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
            // Deselect and reselect to avoid issues from upgrading
            BuildManager.instance.DeselectNode();
            BuildManager.instance.SelectNode(this);
            return true;
        }
    
        /// <summary>
        /// Called when the turret is sold
        /// </summary>
        public void SellTurret()
        {
            // // Grant the money
            // GameStats.money += turretBlueprint.GetSellAmount();
        
            // Spawn the sell effect
            var effect = Instantiate(_buildManager.sellEffect, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
            // Destroy the turret and reset any of the node's selection variables
            Destroy(turret);
            turretBlueprint = null;

            BuildManager.instance.DeselectNode();
        }
    
        /// <summary>
        /// Called when the mouse is down.
        /// Either Selects the turret or builds
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            // Select the node/turret
            if (turret != null)
            {
                _buildManager.SelectNode(this);
                return;
            }
        
            // Check the player is trying to build
            if (!_buildManager.CanBuild)
            {
                _buildManager.Deselect();
                return;
            }
        
            // Construct a turret
            BuildTurret(_buildManager.GetTurretToBuild());
            _buildManager.BuiltTurret();
        }
    
        // Called when the mouse hovers over the node
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Make sure the player is trying to build
            if (!_buildManager.CanBuild)
            {
                return;
            }
            // Check if the player can afford the selected turret
            _rend.material.color = hoverColour;
        }
    
        /// <summary>
        /// Called when the mouse is no longer over the node
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            _rend.material.color = _defaultColour;
        }
    }
}
