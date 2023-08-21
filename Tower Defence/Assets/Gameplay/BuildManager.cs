using Levels._Nodes;
using Turrets;
using UI.Inventory;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Handles all tasks related to building turrets and selecting nodes
    /// </summary>
    public class BuildManager : MonoBehaviour
    {
        [Tooltip("The instance of the BuildManager")]
        public static BuildManager instance;
        
        [Tooltip("The effect spawned when a turret is built.")]
        public GameObject buildEffect;
        [Tooltip("The effect spawned when a turret is sold")]
        public GameObject sellEffect;

        private TurretBlueprint _turretToBuild;
        private GameObject _buildingButton;
        private Node _selectedNode;
        
        /// <summary>
        /// If the player is currently building or not
        /// </summary>
        public bool CanBuild => _turretToBuild != null;
        
        /// <summary>
        /// Check there is only one build manager when loading in
        /// </summary>
        private void Awake()
        {
            // Make sure there is only ever have one BuildManager
            if (instance != null)
            {
                Debug.LogError("More than one build manager in scene!");
                return;
            }
            instance = this;
        }

        /// <summary>
        /// Sets the turret the player want's to build
        /// </summary>
        /// <param name="turret">The blueprint of the turret to build</param>
        /// <param name="buttonToDelete">The inventory button to remove</param>
        public void SelectTurretToBuild(TurretBlueprint turret, GameObject buttonToDelete)
        {
            _turretToBuild = turret;
            _buildingButton = buttonToDelete;
            //DeselectNode();
        }
        
        /// <summary>
        /// Builds the turret on the node and removes the inventory button
        /// </summary>
        public void BuildTurret()
        {
            Destroy(_buildingButton);
            GameManager.TurretInventory.Remove(_turretToBuild);
            DeselectNode();
            _turretToBuild = null;
        }
    
        /// <summary>
        /// Gets the blueprint of the turret the player currently want to build
        /// </summary>
        /// <returns>The turret blueprint of the turret the player wants to build</returns>
        public TurretBlueprint GetTurretToBuild()
        {
            return _turretToBuild;
        }
    
        /// <summary>
        /// Sets the selected node and moves the NodeUI
        /// </summary>
        /// <param name="node">The selected node</param>
        public void SelectNode(Node node)
        {
            if (_selectedNode == node)
            {
                DeselectNode();
                TurretInfo.instance.Close();
                return;
            }
            if (_selectedNode != null)
                DeselectNode();
        
            _selectedNode = node;
            _turretToBuild = null;
            
            TurretInfo.instance.SetTarget(node);
        }
    
        /// <summary>
        /// Deselects the node the player currently has selected
        /// </summary>
        public void DeselectNode()
        {
            Debug.Log("Deselecting Node");
            if (_selectedNode != null && _selectedNode.turret != null)
            {
                _selectedNode.turret.GetComponent<Turret>().Deselected();
            }

            _selectedNode = null;
        }
        
        /// <summary>
        /// Deselects both the node and the turret the player wants to build
        /// </summary>
        public void Deselect()
        {
            DeselectNode();
            _turretToBuild = null;
            Debug.Log("Closing");
            TurretInfo.instance.Close();
        }
    }
}
