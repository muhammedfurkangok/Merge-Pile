using DG.Tweening;
using Runtime.Extensions;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private Vector3 baseTransform => new Vector3(0, 4, 0);
    private Tween moveTween;


    public void MovePlayerByGivenPosition(Vector3 position,Item item)
    {
      
        moveTween?.Kill();
        InputManager.Instance.DisableInput();
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(position, 0.25f).SetEase(Ease.Linear));
        sequence.Append(transform.DOMove(baseTransform, 0.25f).SetEase(Ease.Linear));
        
        sequence.OnComplete(() => { 
            item.OnSelected();
            InputManager.Instance.EnableInput();
       
        });
        
        moveTween = sequence;
        
        
    }
}