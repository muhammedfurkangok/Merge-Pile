using System.Collections.Generic;
using DG.Tweening;
using Runtime.Entities;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Managers
{
    public class UtilityManager : SingletonMonoBehaviour<UtilityManager>
    {
        public Transform cubes;
        private List<Transform> childTransforms = new List<Transform>();
        private List<Vector3> originalPositions = new List<Vector3>();
        private List<Vector3> replyPositions = new List<Vector3>();
        private List<Item> activeItems = new List<Item>();

        private SlotManager slotManager;

        private void Start() {
            slotManager = SlotManager.Instance;
        }

        public void UseUtility(UtilityType utilityType, int count)
        {
       
            switch (utilityType)
            {
                case UtilityType.Shuffle:
                    Shuffle();
                    break;
                case UtilityType.Reverse:
                    Reverse();
                    break;
                case UtilityType.Tip:
                    Tip();
                    break;
            }
        }


        public void Shuffle()
        {
            childTransforms.Clear();
            originalPositions.Clear();

            foreach (Transform child in cubes)
            {
                if(child.gameObject.activeSelf)
                    childTransforms.Add(child);
            }

            foreach (Transform child in childTransforms)
            {
                originalPositions.Add(child.position);
            }

            int childCount = childTransforms.Count;

            for (int i = 0; i < childCount; i++)
            {
                int randomIndex = Random.Range(0, originalPositions.Count);
                childTransforms[i].position = originalPositions[randomIndex];
                originalPositions.RemoveAt(randomIndex);
            }
        }

        public void Reverse()
        {
            if(SlotManager.Instance.GetEmptySlotCount() <= 0)
                return;

            var lastSlotCube = slotManager.GetLastCube();
            var targetTransform = lastSlotCube.cubeBlock.transform;
        
            var cubeRef = lastSlotCube.transform;
            cubeRef.parent = cubes;

            DOTween.Sequence()
                .Append(cubeRef.DOMove(targetTransform.position, 0.25f).SetEase(Ease.InOutSine))
                .Join(cubeRef.DORotateQuaternion(targetTransform.rotation, 0.2f).SetEase(Ease.InOutSine))
                .Join(cubeRef.DOScale(1, 0.25f).SetEase(Ease.InOutSine))
                .AppendCallback(() => {
                    targetTransform.localScale = Vector3.one;
                    targetTransform.gameObject.SetActive(true);
                    targetTransform.GetComponent<Item>().SetCollider(true);
                    Destroy(cubeRef.gameObject);
                });
        }

        public void Tip()
        {
            var keyList = slotManager.GetCubeKeys();
            var emptySlotCount = slotManager.GetEmptySlotCount();
            var targetKey = "";
            var neededSlotCount = 0;
            foreach (var key in keyList)
            {
                neededSlotCount = 3 - key.Value;
                if(neededSlotCount <= emptySlotCount)
                {
                    targetKey = key.Key;
                    break;
                }
            }

            if(slotManager.GetEmptySlotCount() == 9)
            {
                targetKey = ItemManager.Instance.GetRandomKey();
                neededSlotCount = 3;
            }

            if(targetKey != "")
            {
                for (int i = 0; i < neededSlotCount; i++)
                {
                    FindCubeWithKey(targetKey);
                }
            }
        }

        public void FindCubeWithKey(string key)
        {
            activeItems.Clear();
            foreach (Transform child in cubes)
            {
                if(child.gameObject.activeSelf)
                    activeItems.Add(child.GetComponent<Item>());            
            }

            foreach (Item cube in activeItems)
            {
                if(cube.key == key)
                {
                    cube.OnClick();
                    break;
                }
            }
        }
    }

    public enum UtilityType
    {
        Shuffle,
        Reverse,
        Tip
    }
}