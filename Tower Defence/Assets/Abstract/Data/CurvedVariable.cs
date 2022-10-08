using UnityEngine;

namespace Abstract.Data
{
    [CreateAssetMenu(order = 1)]
    public class CurvedVariable : ScriptableObject
    {
        public AnimationCurve value;
    }
}
