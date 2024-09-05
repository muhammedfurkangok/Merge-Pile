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
        public List<Item> items = new List<Item>();
        public CD_ItemData itemObjects;

        [SerializeField] private Transform[] slotTransform;
      
        private Item tipItem;
        
        public static UnityEvent ItemInstantiated;
  

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

        public GameObject GetItemByKey(string key)
        {
            foreach (var item in itemObjects.itemData) {
                if (item.key == key)
                    return item.itemPrefab;
            }

            return null;
        }
        
        public GameObject InstantiateBlockByGivenKey(string key, int instantiatePositionIndex)
        {
            var item = GetItemByKey(key);
            if (item == null)
                return null;

            var Rand = Random.Range(1, -1);
            var ObjPosition =Rand > 0 ? new Vector3(0,0.08f,0.3f) : new Vector3(0,0.05f,-0.3f);
            
            var obj = Instantiate(item, slotTransform[instantiatePositionIndex].position + ObjPosition, Quaternion.identity);
            obj.transform.rotation = Quaternion.Euler(-15, -3, 0);
            obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            
            DOTween.Sequence()
                .Append(obj.transform.DOScale(new Vector3(0.7f, 0.55f, 0.7f), 0.15f).SetEase(Ease.OutQuad))  // Squish etkisi
                .Append(obj.transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.3f).SetEase(Ease.OutBack)); // Normal boyuta dönüş
               

            return obj;
        }
    }
}