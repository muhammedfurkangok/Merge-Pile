using System;
using DG.Tweening;
using Runtime.Enums;
using Runtime.Extensions;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.Entities
{
    public class Slot : MonoBehaviour
    {
          [SerializeField] private bool unlocked = true;
          
          public ItemRef active;
          public Transform refTransform;
          public ParticleSystem particle;
          public bool Unlocked => unlocked;
          public bool isAvailable => active == null;
          
          private bool isAnimating = false;
          
          public void Place(ItemRef cubeRef , bool scaleAnim = true)
          {
              active = cubeRef;
              if (active == null)
                  return;
      
              isAnimating = true;
              var fakeItem = active.transform;
              
              fakeItem.SetParent(transform);

              active.transform.DOLocalMove(refTransform.localPosition, 0.2f).SetEase(Ease.InCirc);
              fakeItem.transform.DOScale(Vector3.one * 0.5f, 0.3f).SetEase(Ease.Linear);
              fakeItem.DOLocalRotateQuaternion(refTransform.localRotation, 0.3f).SetEase(Ease.Linear);
              isAnimating = false;
          }
          public void PlayParticle()
          {
              particle.Play();
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
          
          public void ScoreAnim(Vector3 transfrom, Action callback = null)
          {
              if (!active) return;
              
              var temp = active;
              active = null;
            
              callback?.Invoke();
              DOTween.Sequence()
                  .Append(temp.transform.DOJump(transfrom, 1, 1, 0.5f))
                  .AppendCallback( () =>
                  {
                      Destroy(temp.gameObject); 
                  });
                 
          }
    }
}