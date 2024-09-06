using DG.Tweening;
using UnityEngine;

namespace Runtime.Entities
{
    public class ItemRef : MonoBehaviour
    {
        public string key;
        public MeshRenderer renderer;
        public GameObject cubeBlock;

        public Vector3 scale;
        private Tweener moveTween;

        private void Awake() {
            if (!renderer)
                renderer = GetComponent<MeshRenderer>();
        }

        public void SetColor(Material material)
        {
            renderer.material = new Material(material);
        }

        public void SetScale(Vector3 scale)
        { 
            this.scale = scale;
        }
        
        public void DoScale(Vector3 scale)
        {
            cubeBlock.transform.DOScale(scale, 0.4f).SetEase(Ease.InBack);
        }

        public Tween LocalMoveTo(Vector3 pos)
        {
            moveTween = transform.DOLocalMove(pos, 0.4f).SetEase(Ease.InBack);
            return moveTween;
        }
    }
}