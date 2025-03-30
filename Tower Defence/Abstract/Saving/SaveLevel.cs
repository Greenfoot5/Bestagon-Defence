using System;
using System.Collections.Generic;
using Abstract.Data;
using Godot;
using Modules;
using Turrets;
// TODO - Update Random
//using Random = UnityEngine.Random;

namespace Abstract.Saving
{
    /// <summary>
    /// Represents a level's save data
    /// </summary>
    public partial class SaveLevel : Resource
    {
        [Serializable]
        public struct NodeData
        {
            public string uuid;
            public List<string> moduleNames;
            public List<int> moduleTiers;
            public DynamicTurret.TargetingMethod targetingMethod;
            public float turretRotation;
            public string blueprintName;
        }
        public List<NodeData> Nodes;
        
        public int Energy;
        public int Powercells;
        public int Lives;
        public int WaveIndex;
        public int TotalCellsCollected;
        
        // public Random.State RandomState;
        public int RandomSeed;
        public int ShopRandomN;

        public List<TurretBlueprint> TurretInventory;
        public List<ModuleChainHandler> ModuleInventory;

        // Save file version
        private readonly Variant _version = ProjectSettings.GetSettingWithOverride("application/config/save_version");

        // Inventory
        public static readonly Dictionary<string, TurretBlueprint> Blueprints = new();
        public static readonly Dictionary<string, ModuleChain> Chains = new();
        // Placed items
        // private static AsyncOperationHandle<IList<TurretBlueprint>> _turretOp;
        // private static AsyncOperationHandle<IList<ModuleChain>> _chainOp;
        
        /// <summary>
        /// Translates the class into json format
        /// </summary>
        /// <returns>This class in json format</returns>
        public string ToJson()
        {
            return Json.Stringify(this);
        }
        
        // TODO - Still load stuff
        /// <summary>
        /// Loads json without loading addressables
        /// </summary>
        /// <param name="json">The json to load from</param>
        /// <returns>Save's version</returns>
        public static string LoadVersion(string json)
        {
            var save = Json.ParseString(json).As<SaveLevel>();

            return save._version.AsString();
        }
        
        /// <summary>
        /// Loads this class from json with addressables loaded into memory
        /// </summary>
        /// <param name="json">The json to load from</param>
        public void LoadFromJson(string json)
        {
            // // Matches the labels assigned to the addressables
            // _turretOp = Addressables.LoadAssetsAsync<TurretBlueprint>(new List<string> { "TurretBlueprint" },
            //     addressable => { Blueprints.Add(addressable.Name, addressable); },
            //     Addressables.MergeMode.Union);
            // _turretOp.WaitForCompletion();
            // _chainOp = Addressables.LoadAssetsAsync<ModuleChain>(new List<string> { "ModuleChain" },
            //     addressable =>
            //     {
            //         Chains.Add(addressable.Name, addressable);
            //     },
            //     Addressables.MergeMode.Union);
            // _chainOp.WaitForCompletion();
            
            // TODO - Load the save
            
            var save = Json.ParseString(json);
        }
    }
    
    /// <summary>
    /// Handles the loading and populating of the save data
    /// Designed to be implemented by MonoBehaviours
    /// </summary>
    public interface ISaveableLevel
    {
        void PopulateSaveData(SaveLevel saveData);
        void LoadFromSaveData(SaveLevel saveData);
    }
}