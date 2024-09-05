using DG.Tweening;
using Runtime.Entities;
using Runtime.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        private Vector3 baseTransform => new Vector3(0, 4, 0);
        private Tween moveTween;
    

        [SerializeField] private GameObject playerLeftElbow;
        [SerializeField] private GameObject playerRightElbow;
        [SerializeField] private GameObject playerBackElbow;
  
    
        [SerializeField] private GameObject playerLeftArm;
        [SerializeField] private GameObject playerRightArm;
        [SerializeField] private GameObject playerBackArm;
    
        [SerializeField] private GameObject playerBody;
    
        private Transform playerLefElbowtDefaultTransform;
        private Transform playerRightElbowDefaultTransform;
        private Transform playerBackElbowDefaultTransform;
    
        private Transform playerBodyDefaultTransform;


        public Ease holdEase;
        public Ease dropEase;
        public Ease scoreEase;
        public Ease idleEase;


        public void MovePlayerByGivenPosition(Vector3 position,Item item)
        {
      
            moveTween?.Kill();
            InputManager.Instance.DisableInput();
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMove(position, 0.2f).SetEase(Ease.Linear));
            sequence.Join(PlayerHoldAnim());
            sequence.AppendCallback(() => item.transform.SetParent(transform));
            sequence.Append(transform.DOMove(baseTransform, 0.2f).SetEase(Ease.Linear));
        
            sequence.OnComplete(() =>
            {
                PlayerDefaultAnim();
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    item.OnClick();
                });
                InputManager.Instance.EnableInput();
                
       
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

        [Button]
        public Tween PlayerHoldAnim()
        {
            Sequence sequence = DOTween.Sequence();

        
            sequence.Append(playerLeftElbow.transform.DOLocalRotate(new Vector3(0, 0, -20), 0.15f))
                .SetEase(Ease.OutBack);  
            sequence.Join(playerRightElbow.transform.DOLocalRotate(new Vector3(0, 0, 20), 0.15f))
                .SetEase(Ease.OutBack);

     
            sequence.Append(playerLeftElbow.transform.DOLocalRotate(new Vector3(0, 0, -45), 0.3f))
                .SetEase(Ease.OutElastic);  
            sequence.Join(playerRightElbow.transform.DOLocalRotate(new Vector3(0, 0, 45), 0.3f))
                .SetEase(Ease.OutElastic);
            sequence.Join(playerBackElbow.transform.DOLocalRotate(new Vector3(30, 0, 0), 0.3f))
                .SetEase(Ease.OutElastic);

            return sequence;
        }

        [Button]
        public Tween PlayerOnWayAnim()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(playerLeftElbow.transform.DOLocalRotate(new Vector3(0,0,-45),0.25f)).SetEase(holdEase);;
            sequence.Join(playerRightElbow.transform.DOLocalRotate(new Vector3(0,0,45),0.25f)).SetEase(holdEase);;
            sequence.Join(playerBackElbow.transform.DOLocalRotate(new Vector3(30,0,0),0.25f)).SetEase(holdEase);;
       
            return sequence;
        }
        [Button]
        public Tween PlayerDropAnim()
        {
            Sequence sequence = DOTween.Sequence();

            return sequence;
        }
        [Button]
        public Tween PlayerScoreAnim()
        {
            Sequence sequence = DOTween.Sequence();

            return sequence;
        }
    }
}