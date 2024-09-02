using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public struct GridData
    {
        public bool isOccupied;
        public Color gridColor;
        public Vector2Int position;
    }
}