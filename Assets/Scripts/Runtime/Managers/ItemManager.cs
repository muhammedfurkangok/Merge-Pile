using System.Collections.Generic;
using Runtime.Entities;
using Runtime.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Managers
{
    public class ItemManager : SingletonMonoBehaviour<ItemManager>
    {
        public List<Item> items = new List<Item>();
        private Item tipItem;
  

        private void Start() {
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
    }
}