using System.Collections.Generic;
using Runtime.Data.UnityObject;
using Runtime.Entities;
using Runtime.Enums;
using Runtime.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Managers
{
    public class GridManager : SingletonMonoBehaviour<GridManager>
    {
        #region Self Variables

        #region Public Variables

            public Slot[] slots;
            public List<ItemType> items;
            public CD_ItemSprite spriteData;
            public GameObject selectedItemGameObject;

        #endregion

        #region Private Variables

            private ItemType selectedItemType;
            private bool isHaveSameType;

        #endregion
       
        #endregion
       
        private void Start()
        {
            InitializeSlots();
        }
        public void InitializeSlots()
        {
            foreach (var slot in slots)
            {
                slot.ChangeSprite(GetSprite(ItemType.None));
                slot.itemType = ItemType.None;
            }
        }

        public void SelectAndPlaceItem(GameObject item)
        {
            selectedItemGameObject = item;
            PlaceItem();
        }


        public void PlaceItem()
        {
            selectedItemType = selectedItemGameObject.GetComponent<Item>().itemType;
            
            
            for (int i = 0; i < slots.Length - 1 ; i++)
            {
                if (slots[i].itemType == selectedItemType &&  slots[i + 1].itemType != ItemType.None)
                {
                    isHaveSameType = true;
                    
                    if (slots[i + 1].itemType == selectedItemType)
                    {
                           
                        
                            slots[i].ChangeSprite(GetSprite(ItemType.None));
                            slots[i].itemType = ItemType.None;
                            items.RemoveAt(i);
                            
                            slots[i + 1].ChangeSprite(GetSprite(ItemType.None));
                            slots[i + 1].itemType = ItemType.None;
                            items.RemoveAt(i + 1);
                            
                            slots[i + 2].ChangeSprite(GetSprite(ItemType.None));
                            slots[i + 2].itemType = ItemType.None;
                            items.RemoveAt(i + 2);
                            
                            ShiftItemsRightByGivenIndex(i);
                            break;   
                    }
                    else
                    {
                        var x = items[i + 1];
                        GetEmptySlot(x);
                        items.Insert(i + 1, selectedItemType);
                        slots[i + 1].ChangeSprite(GetSprite(selectedItemType));
                        slots[i + 1].itemType = selectedItemType;  
                    }
                    
                    break;
                }
            }

            if (!isHaveSameType)
            {
                items.Add(selectedItemGameObject.GetComponent<Item>().itemType);
                GetEmptySlot(selectedItemType);
            }
            
            isHaveSameType = false;
           
        }

        private void GetEmptySlot(ItemType itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemType == ItemType.None)
                {
                    slots[i].ChangeSprite(GetSprite(itemType));
                    slots[i].itemType = itemType;
                    break;
                }
            }
        }


        private void ShiftAllItemsLeft()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemType == ItemType.None)
                {
                    for (int j = i + 1; j < slots.Length; j++)
                    {
                        if (slots[j].itemType != ItemType.None)
                        {
                            slots[i].ChangeSprite(GetSprite(slots[j].itemType));
                            slots[i].itemType = slots[j].itemType;
                            slots[j].ChangeSprite(GetSprite(ItemType.None));
                            slots[j].itemType = ItemType.None;
                            break;
                        }
                    }
                }
            }
        }

        private void ShiftItemsRightByGivenIndex(int index)
        {
            for (int j = index; j < slots.Length; j++)
            {
                if (slots[j].itemType != ItemType.None)
                {
                    slots[index].ChangeSprite(GetSprite(slots[j].itemType));
                    slots[index].itemType = slots[j].itemType;
                    slots[j].ChangeSprite(GetSprite(ItemType.None));
                    slots[j].itemType = ItemType.None;
                    break;
                }
            }
        }
        

        public Sprite GetSprite(ItemType itemType)
        {
            foreach (var item in spriteData.itemSpriteData)
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
