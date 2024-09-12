using System.Collections.Generic;
using DG.Tweening;
using Runtime.Entities;
using Runtime.Enums;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Managers
{
    public class UtilityManager : SingletonMonoBehaviour<UtilityManager>
    {

        private Dictionary<UtilityType, bool> utilityStatus = new Dictionary<UtilityType, bool>
        {
            { UtilityType.Bomb, true },
            { UtilityType.Unlock, true },
            { UtilityType.Shuffle, true }
        };


        public void UseUtility(UtilityType utilityType)
        {
            if (utilityStatus[utilityType])
            {
                CursorManager.Instance.SetCursor(utilityType);

                InputManager.Instance.SetUtilityActive(utilityType);

                DeactivateUtility(utilityType);
            }
            else
            {
                Debug.Log($"{utilityType} utility is already used.");
            }
        }

        private void DeactivateUtility(UtilityType utilityType)
        {
            utilityStatus[utilityType] = false;
        }

        public void ApplyUtilityToObject(UtilityType utilityType)
        {
            switch (utilityType)
            {
                case UtilityType.Bomb:
                    Bomb();
                    break;
                case UtilityType.Unlock:
                    Unlock();
                    break;
                case UtilityType.Shuffle:
                    Shuffle();
                    break;
            }
            CursorManager.Instance.ResetCursor();
        }



        public void Bomb()
        {
            Debug.Log("Bomb utility used!");
        }

        public void Unlock()
        {
            Debug.Log("Unlock utility used!");
        }

        public void Shuffle()
        {
            Debug.Log("Shuffle utility used!");
        }
    }
}
