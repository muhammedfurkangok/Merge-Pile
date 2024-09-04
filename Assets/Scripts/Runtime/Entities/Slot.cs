using System;
using DG.Tweening;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.Entities
{
    [System.Serializable]
    public class Slot : MonoBehaviour
    {
          public ItemRef active;
          public Transform refTransform;
          public bool Unlocked => unlocked;
          public bool isAvailable => active == null;
          [SerializeField] private bool unlocked = true;
          private bool isAnimating = false;
      
          public void Place(ItemRef cubeRef, bool scaleAnim = true)
          {
              active = cubeRef;
      
              if (active == null)
                  return;
      
              isAnimating = true;
              var t = active.transform;
              t.SetParent(transform);
      
              var scale = t.localScale;
              if (scaleAnim)
              {
                  t.localScale = cubeRef.scale;
                  t.DOScale(scale, 0.4f).SetEase(Ease.InBack);
              } 
      
              active.LocalMoveTo(refTransform.localPosition);
              t.DOLocalRotateQuaternion(refTransform.localRotation, 0.3f).SetEase(Ease.InBack);
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
          
          public void ScoreAnim(float interval, Action callback = null)
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
                  .AppendInterval(0.25f)
                  .Append(temp.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack))
                  .AppendCallback(() => {
                      
                      isAnimating = false;
                  })
                  .AppendInterval(0.25f)
                  .OnComplete(() => {
                      callback?.Invoke();
                  });
          }
       
    }
}