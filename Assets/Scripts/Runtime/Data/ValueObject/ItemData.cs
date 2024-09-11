using Runtime.Enums;
using UnityEngine;


namespace Runtime.Data.ValueObject
{
    [System.Serializable]   
    public struct ItemData
    {
        public string key;
        public ItemTypes itemType;
        public GameObject itemPrefab;
        public Material itemMaterial;
    }
}