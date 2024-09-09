using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.UnityObject;
using Runtime.Entities;
using Runtime.Extensions;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Runtime.Managers
{
    public class ItemManager : SingletonMonoBehaviour<ItemManager>
    {
        #region Self Variables

        public List<Item> items = new List<Item>();
        public List<Item> glissandoCounterList = new List<Item>();
        public CD_ItemData itemObjects;
        
        [SerializeField] private Transform[] slotTransform;
        [SerializeField] private GameObject frontSideItemsParent;
        [SerializeField] private GameObject backSideItemsParent;
        
        private Item tipItem;
        
        

        #endregion
        

        private void Start()
        {
            items = new List<Item>(FindObjectsOfType<Item>());
            
        }

      
        public int ActiveCubeCount()
        {
            var activeCubeCount = 0;
        
            foreach (var cube in items) {
                if(cube.gameObject.activeSelf)
                    activeCubeCount++;
            }

            return activeCubeCount;
        }

        public string GetRandomKey()
        {
            var key = "";
            var activeCubes = new List<Item>();
            foreach (var cube in items) {
                if(cube.gameObject.activeSelf)
                    activeCubes.Add(cube);
            }
            var random = Random.Range(0, activeCubes.Count);
            key = activeCubes[random].key;
            return key;
        }


        public void ClearTip()
        {
            if(tipItem != null)
                tipItem.outlinable.enabled = false;
        }

        public void DeleteCubeInTheList(Item item)
        {
            items.Remove(item);
        }


        
        public GameObject InstantiateBlockByGivenKey(string key, int instantiatePositionIndex)
        {
            var item = GetItemByKey(key);
            if (item == null)
                return null;

            float Rand = Random.Range(1, -1);
            Vector3 ObjPosition =Rand > 0 ? new Vector3(0,0.08f,0.3f) : new Vector3(0,0.05f,-0.3f);
            GameObject obj = Instantiate(item, slotTransform[instantiatePositionIndex].position + ObjPosition, Quaternion.identity);
            
            if (Rand > 0)
                obj.transform.SetParent(backSideItemsParent.transform);
            else
                obj.transform.SetParent(frontSideItemsParent.transform);
            
            obj.transform.rotation = Quaternion.Euler(-15, -3, 0);
            obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            
            DOTween.Sequence()
                .Append(obj.transform.DOScale(new Vector3(0.7f, 0.45f, 0.7f), 0.15f).SetEase(Ease.OutQuad))
                .Append(obj.transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.3f).SetEase(Ease.OutBack)); 
               

            return obj;
        }
        
        public GameObject GetItemByKey(string key)
        {
            for (int i = 0; i < itemObjects.itemData.Length; i++)
            {
                var item = itemObjects.itemData[i];
                if (item.key == key)
                {
                    int nextIndex = (i + 1) % itemObjects.itemData.Length;
            
                    return itemObjects.itemData[nextIndex].itemPrefab;
                }
            }

            return null;
        }

        public Material GetNextMaterialByKey(string key)
        {
            for (int i = 0; i < itemObjects.itemData.Length; i++)
            {
                var item = itemObjects.itemData[i];
                if (item.key == key)
                {
                    int nextIndex = (i + 1) % itemObjects.itemData.Length;
            
                    return itemObjects.itemData[nextIndex].itemMaterial;
                }
            }

            return null;
        }

        public Material GetMaterialByKey(string key)
        {
            for (int i = 0; i < itemObjects.itemData.Length; i++)
            {
                var item = itemObjects.itemData[i];
                if (item.key == key)
                {
                    return item.itemMaterial;
                }
            }

            return null;
        }
    }
}