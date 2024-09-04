using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Runtime.Data.UnityObject;
using Runtime.Entities;
using Runtime.Enums;
using Runtime.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class SlotManager : SingletonMonoBehaviour<SlotManager>
    {
        #region Self Variables

        #region Public Variables

        public List<Slot> slots;
        public Transform[] slotTransforms;
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
            //Camera.main.ScreenToWorldPoint(slots[0].handlerSlotImage.transform.position);
        }

        public void InitializeSlots()
        {
            foreach (var slot in slots)
            {
                slot.itemType = ItemType.None;
            }
        }
        
        public List<Item> currentItems = new List<Item>();
        public List<GameObject> currentItemGOs = new List<GameObject>();
        public List<ItemType> currentItemsType = new List<ItemType>();
        public void SelectAndPlaceItem(GameObject item)
        {
            // selectedItemGameObject = item;
            
            PlaceItem(item);
        }


        public void PlaceItem(GameObject itemGO)
        {
            var item = itemGO.GetComponent<Item>();
            //selectedItemType = selectedItemGameObject.GetComponent<Item>().itemType;
            
            var similarItemLastIndex = currentItems.Select(x => x.itemType).ToList().LastIndexOf(item.itemType);

            if (similarItemLastIndex > -1)
            {
                InsertItemAtIndex(similarItemLastIndex + 1, item.itemType);
                currentItems.Insert(similarItemLastIndex + 1, item);

                ShiftItemsRightByGivenIndex(similarItemLastIndex + 1);
            }
            else
            {
                InsertItemAtIndex(currentItems.Count, item.itemType);
            }

            CheckMatchCondition();
            
            ////////////////////////////////////////////////
            for (int i = 0; i < slots.Count - 1; i++)
            {

                if (slots[i].itemType == selectedItemType && slots[i + 1].itemType != ItemType.None)
                {
                    isHaveSameType = true;


                    if (slots[i + 1].itemType == selectedItemType)
                    {

                        InsertItemAtIndex(i + 2, selectedItemType);
                        ClearSlots(i, i + 1, i + 2);

                        float randomValue = Random.Range(1, -1) > 0 ? 0.5f : -0.5f;

                        var matchedGameObject = GetNextGameObjectByItemType(selectedItemType);
                        var randomPosition = slotTransforms[i + 1].transform.position + new Vector3(0, 0, randomValue);
                        ItemManager.Instance.SpawnGameObjectGivenPosition(matchedGameObject, randomPosition);

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
            CheckLoseCondition();
        }

        public void CheckMatchCondition()
        {
            var distinctItems = slots.Distinct();
            
            foreach (var item in distinctItems)
            {
                if (item.itemType != ItemType.None)
                {
                    var count = slots.Count(x => x.itemType == item.itemType);
                    if (count >= 3)
                    {
                        var indices = new List<int>();
                        for (int i = 0; i < slots.Count; i++)
                        {
                            if (slots[i].itemType == item.itemType)
                            {
                                indices.Add(i);
                            }
                        }

                        ClearSlots(indices.ToArray());
                        ShiftAllItemsLeft();
                    }
                }
            }
        }

        public void InsertItemAtIndex(int index, ItemType newItemType)
        {

            if (slots[slots.Count - 1].itemType != ItemType.None)
            {
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

        private void ShiftAllItemsLeft()
        {
            for (int i = 0; i < currentItems.Count; i++)
            {
                currentItems[i].transform.DOMove(slots[i].handlerSlotImage.transform.position, 0.5f);
            }
            ///////////////
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].itemType == ItemType.None)
                {
                    for (int j = i + 1; j < slots.Count; j++)
                    {
                        if (slots[j].itemType != ItemType.None)
                        {
                           
                            slots[i].itemType = slots[j].itemType;
                            slots[j].itemType = ItemType.None;

                            MoveSpriteToGivenIndicies( j, i).OnComplete( () =>
                            {
                                slots[i].ChangeSprite(GetSprite(slots[j].itemType));
                                slots[j].ChangeSprite(GetSprite(slots[j].itemType));
                            });



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
                    slots[i].itemType = slots[i - 1].itemType;
                    slots[i - 1].itemType = ItemType.None;

                    slots[i].ChangeSprite(GetSprite(slots[i - 1].itemType));
                    slots[i - 1].ChangeSprite(null);
                    
                    MoveSpriteToGivenIndicies( i - 1, i);
            }
        }

       
        [Button]
        public Tween MoveSpriteToGivenIndicies(int index1, int index2)
        {
            return slots[index1].handlerSlotImage.transform
                .DOMove(slots[index2].handlerSlotImage.transform.position, 0.5f);
        }
        
        private void ClearSlots(params int[] indices)
        {
            foreach (var index in indices)
            {
                slots[index].ChangeSprite(null);
                slots[index].itemType = ItemType.None;
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
        
        public GameObject GetNextGameObjectByItemType(ItemType itemType)
        {
        
            for (int i = 0; i < spriteData.itemSpriteData.Length; i++)
            {
                if (spriteData.itemSpriteData[i].itemType == itemType)
                {

                    int nextIndex = i + 1;


                    if (nextIndex < spriteData.itemSpriteData.Length)
                    {
                        return spriteData.itemSpriteData[nextIndex]
                            .itemPrefab; 
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }
        
        public void CheckLoseCondition()
        {
            int x = 0;
            
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].itemType != ItemType.None)
                {
                    x++;
                }
            }
            
            if (x == 7)
            {
                GameManager.Instance.SetGameStateLevelFail();
            }
        }

    }
}
