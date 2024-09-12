using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHand : MonoBehaviour
{
    public List<Sprite> fingerSprites;
    public Image handImage;

    private Sequence seq;

    private void OnEnable() {
        handImage = GetComponent<Image>();
    }
    
    
    [Button]
    public void ClickAnimation(Vector3 pos)
    {
        transform.localPosition = pos;
        seq.Kill();
        seq = DOTween.Sequence()
            .AppendInterval(0.2f)
            .AppendCallback(() => {
                SetSprite(fingerSprites[1]);
            })
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                SetSprite(fingerSprites[0]);
            })
            .SetLoops(-1);
    }

    [Button]
    public void DragAnimation(Vector3 from, Vector3 to)
    {
        transform.position = from;
        seq.Kill();
        seq = DOTween.Sequence()
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                SetSprite(fingerSprites[1]);
            })
            .Append(transform.DOMove(to,.5f))
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                SetSprite(fingerSprites[0]);
            })
            .Append(transform.DOMove(to,.25f))
            .SetLoops(-1);
    }
    
    public void SetSprite(Sprite sprite)
    {
        if (handImage) handImage.sprite = sprite;
    }
    
    private void OnDestroy() {
        transform.DOKill();
        seq.Kill();
    }
}
