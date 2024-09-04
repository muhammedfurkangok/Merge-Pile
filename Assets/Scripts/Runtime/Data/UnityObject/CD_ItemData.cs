using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_ItemData", menuName = "ScriptableObjects/CD_ItemData", order = 0)]
    public class CD_ItemData : ScriptableObject
    {
          public ItemData[] itemData;
    }
}