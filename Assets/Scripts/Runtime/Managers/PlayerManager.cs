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

        [SerializeField] private GameObject playerLeftElbow;
        [SerializeField] private GameObject playerRightElbow;
        [SerializeField] private GameObject playerBackElbow;
        [SerializeField] private GameObject playerLeftArm;
        [SerializeField] private GameObject playerRightArm;
        [SerializeField] private GameObject playerBackArm;
        
        [SerializeField] private GameObject playerBody;
        
        [SerializeField] private TailAnimator2[] playerTails;
        
        
        private Transform playerLefElbowtDefaultTransform;
        private Transform playerRightElbowDefaultTransform;
        private Transform playerBackElbowDefaultTransform;
        private Transform playerBodyDefaultTransform;
        
        public Ease holdEase;
        public Ease dropEase;
        public Ease scoreEase;
        public Ease idleEase;
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

        public void MovePlayerByGivenPosition(Vector3 position,Item item)
        {
      
            moveTween?.Kill();
            InputManager.Instance.DisableInput();
            Sequence sequence = DOTween.Sequence();
            
            sequence.AppendCallback(() =>
            {
                ToggleTail(true);
            });
            
            sequence.Append(transform.DOMoveX(position.x, 0.1f).SetSpeedBased().SetEase(Ease));
            sequence.AppendCallback( () =>
            {
                SoundManager.Instance.PlaySound(GameSoundType.Touch);
            });
            sequence.Append(transform.DOMove(position, 0.25f).SetEase(Ease));
          
            sequence.AppendCallback(() => item.transform.SetParent(transform));
            sequence.AppendCallback(() => item.transform.DOScale(new Vector3(0.4f,0.4f,0.4f), 0.25f).SetEase(Ease));
            
            sequence.Append(transform.DOMove(baseTransform, 0.2f).SetEase(Ease.Linear));
        
            sequence.OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.025f, () =>
                {
                    item.OnClick();
                });
                InputManager.Instance.EnableInput();
                ToggleTail(false);
                
            });
        
            moveTween = sequence;
        
        }

        [Button]
        public Tween PlayerDefaultAnim()
        {
            Sequence sequence = DOTween.Sequence();
        
            sequence.Append(playerLeftElbow.transform.DOLocalRotate(new Vector3(0, 0, -0), 0.3f))
                .SetEase(Ease.OutElastic);  
            sequence.Join(playerRightElbow.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f))
                .SetEase(Ease.OutElastic);
            sequence.Join(playerBackElbow.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f))
                .SetEase(Ease.OutElastic);
            return sequence;
        }

        
        
        public Tween PlayerDropAnim()
        {
            return null;
        }

    }
}