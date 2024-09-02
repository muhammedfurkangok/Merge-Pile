using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_ItemSprite", menuName = "ScriptableObject/CD_ItemSprite", order = 0)]
    public class CD_ItemSprite : ScriptableObject
    {
         public ItemSpriteData[] itemSpriteData;
    }
}