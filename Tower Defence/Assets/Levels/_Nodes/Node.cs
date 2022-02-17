﻿using Gameplay;
using Modules;
using Turrets;
using Turrets.Blueprints;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Levels._Nodes
{
    /// <summary>
    /// Manages all data and actions for a single node on a level map
    /// </summary>
    public class Node : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerUpHandler
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

        // Pointer handling
        private bool _pressOnRelease;

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
            Vector3 nodePosition = transform.position;
            GameObject newTurret = Instantiate(blueprint.prefab, nodePosition, Quaternion.identity);
            turret = newTurret;
            var turretClass = turret.GetComponent<Turret>();
            turretBlueprint = blueprint;
        
            foreach (Module turretModule in blueprint.modules)
            {
                turretClass.AddModule(turretModule);
            }
        
            // Spawn the build effect and destroy after
            GameObject effect = Instantiate(_buildManager.buildEffect, nodePosition, Quaternion.identity);
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
            bool appliedModule = turret.GetComponent<Turret>().AddModule(module);
            if (!appliedModule) return false;

            // Spawn the build effect
            GameObject effect = Instantiate(_buildManager.buildEffect, transform.position, Quaternion.identity);
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
            GameObject effect = Instantiate(_buildManager.sellEffect, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
            // Destroy the turret and reset any of the node's selection variables
            Destroy(turret);
            turretBlueprint = null;

            BuildManager.instance.DeselectNode();
        }
    
        /// <summary>
        /// Called when the mouse is down.
        /// Depending on platform, processes the input for later interaction of the node
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            // Ignore camera pan as a click
            if (eventData.button == PointerEventData.InputButton.Middle)
                return;

            // If on Android, queue interaction for release of touch
            // Makes it possible to cancel the input if the touch was meant to drag the camera
            if (Application.platform == RuntimePlatform.Android)
            {
                _pressOnRelease = true;
                return;
            }

            // If on Desktop and click wasn't a camera pan - handle input as normal
            HandlePointerInteract();
        }

        /// <summary>
        /// Called when the pointer was dragged.
        /// Acts as a cancel for interaction on Android
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            _pressOnRelease = false;
        }

        /// <summary>
        /// Called when the pointer was released.
        /// If the pointer wasn't dragged, then on Android this calls the interaction handler
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_pressOnRelease)
                HandlePointerInteract();
        }

        /// <summary>
        /// Handles interaction.
        /// Either Selects the turret or builds
        /// </summary>
        private void HandlePointerInteract()
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