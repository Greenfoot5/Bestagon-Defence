using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Abstract.Data
{
    /// <summary>
    /// A list of items and their weight.
    /// Can get a random item and total weight of the values
    /// </summary>
    /// <typeparam name="T">The type of the list</typeparam>
    [Serializable]
    public struct WeightedList<T>
    {
        public List<WeightedItem<T>> list;
    
        /// <summary>
        /// Basic constructor for the list
        /// </summary>
        /// <param name="list">The list to create</param>
        public WeightedList(List<WeightedItem<T>> list)
        {
            this.list = list;
        }
    
        /// <summary>
        /// Gets a random item from the list using the weights
        /// </summary>
        /// <returns>A random item, if all weights are 0, selects a random with equal weighting</returns>
        /// <exception cref="NullReferenceException">The list is empty</exception>
        public T GetRandomItem()
        {
            if (list.Count == 0) throw new NullReferenceException("WeightedList is empty");
        
            var total = list.Sum(t => t.weight);
            total = Random.Range(0f, total);

            if (total == 0) return list[Random.Range(0, list.Count)].item;

            var i = 0;
            while (total >= 0 && i < list.Count)
            {
                if (total < list[i].weight) return list[i].item;

                total -= list[i].weight;
                i++;
            }
        
            // It's not perfect, but it's better than nothing
            if (total == 0) return list[Random.Range(0, list.Count)].item;
        
            throw new NullReferenceException("Cannot return a random item from the WeightedList");
        }
    
        /// <summary>
        /// Gets the total weight of all elements in the list combined
        /// </summary>
        /// <returns>The total overall weight</returns>
        /// <exception cref="NullReferenceException">The list is empty</exception>
        public float GetTotalWeight()
        {
            if (list.Count == 0) throw new NullReferenceException("WeightedList is empty");
        
            var total = list.Sum(t => t.weight);

            return total;
        }
    
        /// <summary>
        /// Empties/Cleans the list of all elements
        /// </summary>
        public void Clear()
        {
            list = new List<WeightedItem<T>>();
        }
    }
}
