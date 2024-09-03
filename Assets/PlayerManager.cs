using DG.Tweening;
using Runtime.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

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

        sequence.Append(playerLeftElbow.transform.DOLocalRotate(new Vector3(0,0,-45),0.25f)).SetEase(holdEase);;
        sequence.Join(playerRightElbow.transform.DOLocalRotate(new Vector3(0,0,45),0.25f)).SetEase(holdEase);;
        sequence.Join(playerBackElbow.transform.DOLocalRotate(new Vector3(30,0,0),0.25f)).SetEase(holdEase);;
       
        return sequence;
    }

    public Tween PlayerOnWayAnim()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(playerLeftElbow.transform.DOLocalRotate(new Vector3(0,0,-45),0.25f)).SetEase(holdEase);;
        sequence.Join(playerRightElbow.transform.DOLocalRotate(new Vector3(0,0,45),0.25f)).SetEase(holdEase);;
        sequence.Join(playerBackElbow.transform.DOLocalRotate(new Vector3(30,0,0),0.25f)).SetEase(holdEase);;
       
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