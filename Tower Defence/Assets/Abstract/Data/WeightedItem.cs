using UnityEngine;

namespace Abstract.Data
{
    /// <summary>
    /// An item with a weight
    /// </summary>
    /// <typeparam name="T">The type of item to store</typeparam>
    [System.Serializable]
    public struct WeightedItem<T>
    {
        public T item;
        [Min(0)]
        public float weight;

        public WeightedItem(T item, float weight)
        {
            this.item = item;
            this.weight = weight;
        }
    }
}
