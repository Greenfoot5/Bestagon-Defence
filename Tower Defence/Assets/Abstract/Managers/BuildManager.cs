using Turrets;
using Turrets.Blueprints;
using UI;
using UnityEngine;

namespace Abstract.Managers
{
    /// <summary>
    /// Handles all tasks related to building turrets and selecting nodes
    /// </summary>
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager instance;
        
        [Tooltip("The effect spawned when a turret is built.")]
        public GameObject buildEffect;
        [Tooltip("The effect spawned when a turret is sold")]
        public GameObject sellEffect;

        private TurretBlueprint _turretToBuild;
        private GameObject _buildingButton;
        private Node _selectedNode;
    
        [Tooltip("The UI to move above the turret")]
        public NodeUI nodeUI;

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
            DeselectNode();
        }
        
        /// <summary>
        /// Builds the turret on the node and removes the inventory button
        /// </summary>
        public void BuiltTurret()
        {
            Destroy(_buildingButton);
            Deselect();
        }
    
        /// <summary>
        /// Gets the blueprint of the turret the player currently want to build
        /// </summary>
        /// <returns>The turret blueprint of the turret the player wants to build</returns>
        public TurretBlueprint GetTurretToBuild()
        {
            return _turretToBuild;
        }
    
        // Set's the selected node so the game can move the NodeUI
        public void SelectNode(Node node)
        {
            if (_selectedNode == node)
            {
                DeselectNode();
                return;
            }
            if (_selectedNode != null)
                DeselectNode();
        
            _selectedNode = node;
            _turretToBuild = null;

            nodeUI.SetTarget(node);
        }
    
        // Deselects the node
        public void DeselectNode()
        {
            if (_selectedNode != null && _selectedNode.turret != null)
            {
                _selectedNode.turret.GetComponent<Turret>().Deselected();
            }

            _selectedNode = null;
            nodeUI.Hide();
        }

        public void Deselect()
        {
            DeselectNode();
            _turretToBuild = null;
        }
    }
}
