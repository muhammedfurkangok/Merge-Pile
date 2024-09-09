using DG.Tweening;
using FIMSpace.FTail;
using Runtime.Entities;
using Runtime.Enums;
using Runtime.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        private Vector3 baseTransform => new Vector3(0, 4, 0);
        private Tween moveTween;

        #region Self Variables
        
        [SerializeField] private TailAnimator2[] playerTails;
        
        public Ease Ease;

        #endregion
     
        private void Start()
        {
            ToggleTail(false);
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
            InputManager.Instance.DisableInput();
            Sequence sequence = DOTween.Sequence();

            float distance = Vector3.Distance(transform.position, position);
            
            float moveDuration = Mathf.Clamp(distance * 0.05f, 0.3f, 0.8f); 

            sequence.AppendCallback(() =>
            {
                ToggleTail(true);
            });

            sequence.Append(transform.DOMoveX(position.x, moveDuration * 0.4f).SetEase(Ease));
            sequence.AppendCallback(() =>
            {
                SoundManager.Instance.PlaySound(GameSoundType.Touch);
            });
            sequence.Append(transform.DOMove(position, moveDuration * 0.6f).SetEase(Ease));
            sequence.AppendCallback(() => item.transform.SetParent(transform));
            sequence.AppendCallback(() => item.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), moveDuration * 0.6f).SetEase(Ease));
            sequence.Append(transform.DOMove(baseTransform, moveDuration * 0.8f).SetEase(Ease.Linear));

            sequence.OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.015f, () =>
                {
                    item.OnClick();
                });
                InputManager.Instance.EnableInput();
                ToggleTail(false);
            });

            moveTween = sequence;
        }

    }
}