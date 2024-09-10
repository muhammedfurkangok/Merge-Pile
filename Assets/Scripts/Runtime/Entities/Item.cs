using DG.Tweening;
using EPOOutline;
using Runtime.Enums;
using Runtime.Extensions;
using Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Entities
{
    public class Item : MonoBehaviour
    {

        #region Self Variables

        #region Public Variables

        public string key = "";
        public GameObject cubeRefPrefab;
        public MeshRenderer itemRenderer;
        public bool Tapped => !collider.enabled;
        public Outlinable outlinable;
        public bool success = false;
        
        public float raycastHeight = 5f; 
        public int raycastCount = 6; 
        
        public LayerMask itemLayer;
        public bool canClickable;
            
        
        #endregion

        #region Private Variables

        private Tween tween;
        private Collider collider;
        private Rigidbody rigidbody;
        private Vector3 startingPosition;   
        private bool isMoving = false;
        private Color originalColor;
        private Color selectedColor = Color.white;
       
        
        #endregion

        #endregion


    private void Awake()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        startingPosition = transform.position;
        originalColor = itemRenderer.sharedMaterial.color;
    }

    void FixedUpdate()
    {
        bool canBeClicked = CheckClickable();
       
        UpdateColorBasedOnClickable(canBeClicked);
    }
    
    public void UpdateColorBasedOnClickable(bool isClickable)
    {
        canClickable = isClickable;
        if (isClickable)
        {
            UpdateColorSelected(); 
        }
        else
        {
            UpdateColorNotSelected(); 
        }
    }
    public bool CheckClickable()
    {
        bool isCurrentlyClickable = true; 

        float raycastInterval = transform.localScale.x / (raycastCount - 1); 
        Vector3 startRayPosition = transform.position; 

        for (int i = 0; i < raycastCount; i++)
        {
            Vector3 rayStart = startRayPosition + Vector3.right * (i * raycastInterval - transform.localScale.x / 2);
            RaycastHit hit;
            
            if (Physics.Raycast(rayStart, Vector3.up, out hit, raycastHeight, itemLayer))
            {
               
                if (hit.collider.gameObject != gameObject)
                {
                    isCurrentlyClickable = false;
                    break; 
                }
            }
        }

        return isCurrentlyClickable;
    }
    
    public void Activate(float delay = 0)
    {
        isMoving = true;
        collider.enabled = true;
        tween?.Kill();
        tween = transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
            isMoving = false;
        }).SetDelay(delay);
    }

    public void SetCollider(bool active)
    {
        collider.enabled = active;
    }

    public void SetRigidBody(bool active)
    {
        rigidbody.isKinematic = !active;
    }

    [Button]
    public void Tip()
    {
        outlinable.enabled = true;
        tween?.Kill();
        tween =
        DOTween.Sequence()
            .Append(DOVirtual.Float(1, .4f, 0.5f, (float value) =>
            {
                outlinable.OutlineParameters.DilateShift = value;
            })
            )
            .Append(DOVirtual.Float(.4f, 1, 0.5f, (float value) =>
            {
                outlinable.OutlineParameters.DilateShift = value;
            })
            ).SetLoops(-1);
    }

        [Button]
       public void UpdateColorNotSelected()
       {
        Renderer childRenderer = transform.GetComponentInChildren<Renderer>();
        childRenderer.material.DOColor( selectedColor, 0.5f);
       }
       [Button]
       public void UpdateColorSelected()
       {
          Renderer childRenderer = transform.GetComponentInChildren<Renderer>();
          childRenderer.material.DOColor( originalColor, 0.5f);
        }
        
        public void OnClick()
        {
            if (isMoving) 
                return;
            if (!SlotManager.Instance.HasAvailableSlot())
            {
                return;
            }
            DOVirtual.DelayedCall(0.1f, () =>
            {
              
            });

           
            var previousItem = ItemManager.Instance.glissandoCounterList.Count > 0 ?  ItemManager.Instance.glissandoCounterList[^1] : null;
            if (previousItem == null || previousItem.key != key)
            {
                ItemManager.Instance.glissandoCounterList.Clear();
                SoundManager.Instance.StopGlissando();
            }
            
            ItemManager.Instance.glissandoCounterList.Add(this);
            SoundManager.Instance.PlaySound(GameSoundType.Slot);
           
                var itemRef = Instantiate(cubeRefPrefab, transform.position, transform.rotation);
                var itemRefScript = itemRef.GetComponent<ItemRef>();
                itemRefScript.key = key;
                itemRefScript.SetColor(ItemManager.Instance.GetMaterialByKey(key));
                itemRefScript.cubeBlock = gameObject;
                SlotManager.Instance.Place(itemRefScript);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            float raycastInterval = transform.localScale.x / (raycastCount - 1);
            Vector3 startRayPosition = transform.position + Vector3.up * raycastHeight;
        
            for (int i = 0; i < raycastCount; i++)
            {
                Vector3 rayStart = startRayPosition + Vector3.right * (i * raycastInterval - transform.localScale.x / 2);
                Gizmos.DrawRay(rayStart, Vector3.down * raycastHeight);
            }
        }
    }
}
