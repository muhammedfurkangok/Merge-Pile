using UnityEngine;
using Runtime.Enums;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public struct GridData
    {
        public bool isOccupied;
        public GameColors gameColor;
        public Vector2Int position;
    }
}