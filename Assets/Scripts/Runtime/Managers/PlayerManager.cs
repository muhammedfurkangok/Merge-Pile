using BoingKit;
using DG.Tweening;
using FIMSpace.FTail;
using Runtime.Controllers;
using Runtime.Entities;
using Runtime.Enums;
using Runtime.Extensions;
using Sirenix.OdinInspector;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Runtime.Managers
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        private Vector3 baseTransform => new Vector3(0, 4, 0);
        private Tween moveTween;

        #region Self Variables

        [SerializeField] private TailAnimator2[] playerTails;
        [SerializeField] private BoingBones boingBones;

        public Ease Ease;

        #endregion

        private void Start()
        {
            ToggleTail(false);
            ToggleBoing(false);
        }

        private void ToggleBoing(bool enable)
        {
            boingBones.enabled = enable;
        }

        private void ToggleTail(bool enable)
        {
            foreach (var tail in playerTails)
            {
                tail.enabled = enable;
            }
        }

        public void MovePlayerByGivenPosition(Vector3 position, Item item)
        {
            moveTween?.Kill();
            ToggleBoing(false);

            InputManager.Instance.DisableInput();
            Sequence sequence = DOTween.Sequence();

          
            
            var itemScript = item.GetComponent<Item>();
            var ItemNormalScale = item.transform.localScale;
            var GetAvailableSlot = SlotManager.Instance.GetAvailableSlot();

            sequence.Append(transform.DOMoveX(position.x, 0.15f).SetEase(Ease));
            sequence.AppendCallback(() =>
            {
                SoundManager.Instance.PlaySound(GameSoundType.Touch);
                ToggleTail(true);
            });
            sequence.Append(transform.DOMoveY(position.y, 0.15f).SetEase(Ease));
            sequence.AppendCallback(() =>
            {
                item.transform.SetParent(transform);
                item.transform.DOLocalRotateQuaternion(itemScript.cubeRefPrefab.transform.localRotation, 0.3f).SetEase(Ease.Linear);
                item.SetCollider(false);
                item.SetRigidBody(false);
                // item.transform.SetLayerRecursive(LayerMask.NameToLayer("Slot"));
            });
            sequence.Append(transform.DOMoveY(baseTransform.y ,0.25f).SetEase(Ease).OnComplete( () =>
            {
                ToggleBoing(true);
            }));
            sequence.Join( DOVirtual.DelayedCall(0.125f, () =>
            {
                item.OnClick();
                InputManager.Instance.EnableInput();
            }));
            sequence.Append(transform.DOMoveX(baseTransform.x, 0.15f).SetEase(Ease));
           
            sequence.OnComplete(() =>
            {
                
                ToggleTail(false);
            });

            moveTween = sequence;
        }
    }
}
