using System.Collections.Generic;
using Runtime.Data.UnityObject;
using Runtime.Entities;
using Runtime.Enums;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Managers
{
    public class GridManager : SingletonMonoBehaviour<GridManager>
    {
        #region Self Variables

        #region Public Variables

            public List<Slot> slots;
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

            for (int i = 0; i < slots.Count - 1; i++)
            {
               
                if (slots[i].itemType == selectedItemType && slots[i + 1].itemType != ItemType.None)
                {
                    isHaveSameType = true;

                   
                    if (slots[i + 1].itemType == selectedItemType)
                    {
                       
                            InsertItemAtIndex( i + 2, selectedItemType);
                            ClearSlots(i, i + 1, i + 2);
                            ShiftAllItemsLeft();
                        
                    }
                    else
                    { 
                        InsertItemAtIndex(i + 1, selectedItemType);
                    }
                    break;
                }
            }

            if (!isHaveSameType)
            {
                
                var emptySlot = GetEmptySlot();
                if (emptySlot != null)
                {
                    slots[emptySlot.Value].ChangeSprite(GetSprite(selectedItemType));
                    slots[emptySlot.Value].itemType = selectedItemType;
                }
            }
            
            isHaveSameType = false;
        }

      
        public void InsertItemAtIndex(int index, ItemType newItemType)
        {
           
            if (slots[slots.Count - 1].itemType != ItemType.None)
            {
                Debug.LogError("Son slot dolu, araya ekleme yapılamaz!");
                return;
            }

          
            ShiftItemsRightByGivenIndex(index);

          
            slots[index].ChangeSprite(GetSprite(newItemType));
            slots[index].itemType = newItemType;

        }

       
        private int? GetEmptySlot()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].itemType == ItemType.None)
                {
                    return i;
                }
            }
            return null;
        }

       
        private void ClearSlots(params int[] indices)
        {
            foreach (var index in indices)
            {
                slots[index].ChangeSprite(GetSprite(ItemType.None));
                slots[index].itemType = ItemType.None;
            }
        }

    
        private void ShiftAllItemsLeft()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].itemType == ItemType.None)
                {
                    for (int j = i + 1; j < slots.Count; j++)
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

       
        public void ShiftItemsRightByGivenIndex(int index)
        {
            if (slots[slots.Count - 1].itemType != ItemType.None)
            {
                return;
            }

            for (int i = slots.Count - 1; i > index; i--)
            {
                if (slots[i - 1].itemType != ItemType.None)
                {
                    slots[i].ChangeSprite(GetSprite(slots[i - 1].itemType));
                    slots[i].itemType = slots[i - 1].itemType;

                    // Önceki slotu temizle
                    slots[i - 1].ChangeSprite(GetSprite(ItemType.None));
                    slots[i - 1].itemType = ItemType.None;
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
