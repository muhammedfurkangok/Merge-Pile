using System;
using DG.Tweening;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.Entities
{
    public class Slot : MonoBehaviour
    {
          public ItemRef active;
          public Transform refTransform;
          public bool Unlocked => unlocked;
          public bool isAvailable => active == null;
          [SerializeField] private bool unlocked = true;
          private bool isAnimating = false;
          
          
      
          public void Place(ItemRef cubeRef , bool scaleAnim = true)
          {
              active = cubeRef;
              if (active == null)
                  return;
      
              isAnimating = true;
              var fakeItem = active.transform;
              fakeItem.SetParent(transform);
              active.LocalMoveTo(refTransform.localPosition);
              fakeItem.DOLocalRotateQuaternion(refTransform.localRotation, 0.3f).SetEase(Ease.InBack);
              DOVirtual.DelayedCall(0.5f, () => {
              isAnimating = false;
              });
          }
      
          public bool IsAvailable()
          {
              return isAvailable && !isAnimating;
          }
      
          public void Clear()
          {
              if (!active) return;
      
              active.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
              {
                  Destroy(active.gameObject);
                  active = null;
              });
          }
          
          public void ScoreAnim(float interval, Vector3 transfrom, Action callback = null)
          {
              if (!active) return;
              
              var temp = active;
              active = null;
              isAnimating = true;

             
              if(SlotManager.Instance.GetEmptySlotCount() <= 0 & ItemManager.Instance.ActiveCubeCount() <= 0)
              {
                  //levelDone;
              }

              DOTween.Sequence()
                  .Append(temp.transform.DOMove(transfrom, 0.25f).SetEase(Ease.InBack))
                  .Append(temp.transform.DOScale(new Vector3(2,2,2), 0.25f).SetEase(Ease.OutBack))
                  .AppendCallback(() => {
                      Destroy(temp.gameObject);
                      isAnimating = false;
                  })
                  .AppendInterval(0.5f)
                  
                  .OnComplete(() => {
                    
                      callback?.Invoke();
                  });
          }
       
    }
}