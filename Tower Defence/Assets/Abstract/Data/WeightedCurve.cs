using UnityEngine;

namespace Abstract.Data
{
    /// <summary>
    /// An item with a weight
    /// </summary>
    /// <typeparam name="T">The type of item to store</typeparam>
    [System.Serializable]
    public struct WeightedCurve<T>
    {
        public T item;
        public AnimationCurve weight;

        public WeightedCurve(T item)
        {
            this.item = item;
            this.weight = new AnimationCurve();
        }
    }
}