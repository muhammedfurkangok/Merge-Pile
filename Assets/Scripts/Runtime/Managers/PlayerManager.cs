using System;
using BoingKit;
using DG.Tweening;
using FIMSpace.FTail;
using Runtime.Controllers;
using Runtime.Entities;
using Runtime.Enums;
using Runtime.Extensions;
using Runtime.Helpers;
using Sirenix.OdinInspector;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Runtime.Managers
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
   
        #region Self Variables
       
        private Vector3 baseTransform => new Vector3(0.25f, 4, 0);
        private Tween moveTween;
        
        [SerializeField] private IKTargetRaycast[] ikTargetRaycasts;
        [SerializeField] private Rig rig;

        public Transform rayPoint;

        public Ease Ease;

        #endregion

        private void Start()
        {
            transform.DOMove( baseTransform, 0.5f).SetEase(Ease.InBack);
        }
        
        private void SetIKPosition()
        {
            foreach (var ikTargetRaycast in ikTargetRaycasts)
            {
                ikTargetRaycast.SetControllerPosition();
            }
        }

        public void MovePlayerByGivenPosition(Vector3 position, Item item)
        {
            moveTween?.Kill();
            
            DOVirtual.Float( rig.weight, 1, 0.01f, (x) => rig.weight = x);
            InputManager.Instance.DisableInput();
            Sequence sequence = DOTween.Sequence();
            var itemScript = item.GetComponent<Item>();
            
            sequence.Append(transform.DOMoveX(position.x, 0.15f).SetEase(Ease));
            sequence.AppendCallback(() =>
            {
                SoundManager.Instance.PlaySound(GameSoundType.Touch);
            });
            sequence.Append(transform.DOMove(position, 0.15f).SetEase(Ease));
            sequence.AppendCallback(() =>
            {
                item.transform.SetParent(transform);
                item.ColliderTrigger(false);
                item.transform.DOMove(rayPoint.position + new Vector3(0,-.5f,0), 0.15f).SetEase(Ease.Linear);
                item.transform.DOLocalRotateQuaternion(itemScript.cubeRefPrefab.transform.localRotation, 0.3f).SetEase(Ease.Linear);
            });
            sequence.Join( DOVirtual.DelayedCall(0.2f, () =>
            {
                item.SetCollider(false);
                item.SetRigidBody(false);
            }));
            sequence.Append(transform.DOMoveY(baseTransform.y, 0.15f).SetEase(Ease));
            sequence.Join( 
                
                DOVirtual.DelayedCall(0.1f, () =>
                {
                item.gameObject.SetActive(false);
                ItemManager.Instance.DeleteCubeInTheList(itemScript);
                SetIKPosition();
                item.OnClick();
                DOVirtual.Float( rig.weight, 0, 0.01f, (x) => rig.weight = x);
            }));
            sequence.AppendCallback( () =>
            {
                InputManager.Instance.EnableInput();
            });
            sequence.Append(transform.DOMoveX(baseTransform.x, 0.15f).SetEase(Ease));
           
            moveTween = sequence;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayPoint.position, rayPoint.forward);
        }
    }
}
