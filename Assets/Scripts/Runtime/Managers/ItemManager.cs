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

            var RandomPos = Random.Range(1,-1) > 0 ? 0.3f : -0.3f ;
            
            var obj = Instantiate(item, slotTransform[instantiatePositionIndex].position + new Vector3(0,0,RandomPos), Quaternion.identity);
            obj.transform.localScale = Vector3.zero;
            DOTween.Sequence( )
                .AppendInterval(0.25f)
                .Append(obj.transform.DOScale(new Vector3(0.65f,0.65f,0.65f), 0.5f))
                .Join(obj.transform.DOPunchScale( new Vector3(1.5f,1.5f,1.5f), 0.3f, 1, 0)).SetEase( Ease.OutBack);

            return obj;
        }
    }
}