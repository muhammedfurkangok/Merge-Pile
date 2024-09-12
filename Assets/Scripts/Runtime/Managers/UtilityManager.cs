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
            // { UtilityType.Shuffle, true }
        };


        public void UseUtility(UtilityType utilityType)
        {
            if (utilityStatus[utilityType])
            {
                UtiltyTip.Instance.SetCursor(utilityType);

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
           Debug.Log("done"); 
        }

        public void ApplyUtilityToObject(UtilityType utilityType, Item item)
        {
            switch (utilityType)
            {
                case UtilityType.Bomb:
                    Bomb(item);
                    break;
                case UtilityType.Unlock:
                    Unlock(item);
                    break;
                // case UtilityType.Shuffle:
                //     Shuffle();
                //     break;
            }
            UtiltyTip.Instance.ResetCursor();
        }



        public void Bomb(Item item )
        {
            item.transform.DOPunchScale(transform.localScale + Vector3.one * .25f, 0.25f).SetEase(Ease.Linear);
        }

        public void Unlock(Item item)
        {
                item.SetCollider(false);
            item.transform.DOJump( item.transform.position + Vector3.up, 0.5f, 1, 0.25f).SetEase(Ease.Linear).OnComplete( () =>
            {
                item.SetRigidBody(false);
                item.gameObject.SetActive(false);
                item.OnClick();
                
            });
        }

        // public void Shuffle()
        // {
        //     Debug.Log("Shuffle utility used!");
        // }
    }
}
