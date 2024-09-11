using UnityEngine;
using Runtime.Enums;
using UnityEngine.Serialization;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public struct GridData
    {
        public bool isOccupied;
        [FormerlySerializedAs("gameColor")] public ItemTypes ıtemType;
        public Vector2Int position;
    }
}