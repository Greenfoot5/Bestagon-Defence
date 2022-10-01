using UnityEngine;
using UnityEngine.Serialization;

namespace Abstract.Data
{
    [CreateAssetMenu]
    public class CurvedVariable : ScriptableObject
    {
        public AnimationCurve value;
    }
}
