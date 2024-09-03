using DG.Tweening;
using Runtime.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private Vector3 baseTransform => new Vector3(0, 4, 0);
    private Tween moveTween;
    

    [SerializeField] private GameObject playerLeft;
    [SerializeField] private GameObject playerRight;
    [SerializeField] private GameObject playerBack;
    
    [SerializeField] private GameObject playerBody;


    public void MovePlayerByGivenPosition(Vector3 position,Item item)
    {
      
        moveTween?.Kill();
        InputManager.Instance.DisableInput();
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(position, 0.25f).SetEase(Ease.Linear));
        sequence.Append(PlayerIdleAnim());
        sequence.Append(transform.DOMove(baseTransform, 0.25f).SetEase(Ease.Linear));
        
        sequence.OnComplete(() => { 
            item.OnSelected();
            InputManager.Instance.EnableInput();
       
        });
        
        moveTween = sequence;
        
    }


    public Tween PlayerIdleAnim()
    {
        Sequence sequence = DOTween.Sequence();
        
        

        return sequence;
    }

    [Button]
    public Tween PlayerHoldAnim()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(playerLeft.transform.DOLocalRotate(new Vector3(0,0,-45),0.25f));
        sequence.Join(playerRight.transform.DOLocalRotate(new Vector3(0,0,45),0.25f));
        sequence.Join(playerBack.transform.DOLocalRotate(new Vector3(30,0,0),0.25f));
        
        return sequence;
    }
    
    public Tween PlayerDropAnim()
    {
        Sequence sequence = DOTween.Sequence();

        return sequence;
    }

    public Tween PlayerScoreAnim()
    {
        Sequence sequence = DOTween.Sequence();

        return sequence;
    }
}