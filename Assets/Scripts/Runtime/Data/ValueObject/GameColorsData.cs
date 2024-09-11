using Runtime.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public struct GameColorsData
    {
       
         public ItemTypes itemType;
        public Color color;
        public Material materialColor;
    }
}