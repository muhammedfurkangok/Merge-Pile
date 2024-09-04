using Runtime.Enums;
using UnityEngine;


namespace Runtime.Data.ValueObject
{
    [System.Serializable]   
    public struct ItemSpriteData
    {
        public ItemType itemType;
        public GameObject itemPrefab;
    }
}