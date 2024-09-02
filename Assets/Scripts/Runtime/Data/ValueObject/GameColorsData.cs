using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public struct GameColorsData
    {
        [Header("Game Color Data")]
        public GameColors gameColor;
        public Color color;
        public Material materialColor;
    }
}