using System.Collections.Generic;
using Runtime.Data.UnityObject;
using Runtime.Entities;
using Runtime.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Managers
{
    public class GridManager : MonoBehaviour
    {
        public Slot[] slots;
        public CD_ItemSprite spriteData;
        public GameObject selectedGem;
        private ItemType selectedItemType;

        private void Start()
        {
            InitializeSlots();
        }

        public void InitializeSlots()
        {
            foreach (var slot in slots)
            {
                slot.ChangeSprite(GetSprite(ItemType.None));
            }
        }

        public void PlaceGem()
        {
            
            selectedItemType = selectedGem.GetComponent<Item>().itemType;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemType == ItemType.None)
                {
                    slots[i].ChangeSprite(GetSprite(selectedItemType));
                    slots[i].itemType = selectedItemType;
                    break;
                }
            }

            CheckMatches();
        }

        private void CheckMatches()
        {
            List<int> matchedIndexes = new List<int>();

            for (int i = 0; i < slots.Length - 2; i++)
            {
                if (slots[i].itemType == slots[i + 1].itemType && slots[i].itemType == slots[i + 2].itemType)
                {
                    
                    matchedIndexes.Add(i);
                    matchedIndexes.Add(i + 1);
                    matchedIndexes.Add(i + 2);

                    
                    slots[i].ChangeSprite(GetSprite(ItemType.None));
                    slots[i].itemType = ItemType.None;
                    slots[i + 1].ChangeSprite(GetSprite(ItemType.None));
                    slots[i + 1].itemType = ItemType.None;
                    slots[i + 2].ChangeSprite(GetSprite(ItemType.None));
                    slots[i + 2].itemType = ItemType.None;
                }
            }

            if (matchedIndexes.Count > 0)
            {
                Debug.Log("Puan kazandınız!");
            }
            else
            {
               
                Debug.Log("3'lü eşleşme bulunamadı.");
            }
        }
        
        public Sprite GetSprite(ItemType itemType)
        {
            foreach ( var item in spriteData.itemSpriteData)
            {
                if (item.itemType == itemType)
                {
                    return item.itemSprite;
                }
            }
            
            return null;
        }
    }
}
