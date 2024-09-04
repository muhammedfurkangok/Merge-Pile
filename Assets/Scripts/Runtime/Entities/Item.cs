using DG.Tweening;
using EPOOutline;
using Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Entities
{
    public class Item : MonoBehaviour
    { 
    public string key = "";
    public GameObject cubeRefPrefab;

    private Vector3 startingPosition;
    private Tween tween;
    private Collider collider;
    public MeshRenderer itemRenderer;

    public bool Tapped => !collider.enabled;

    public Outlinable outlinable;
    public bool success = false;
    private bool isMoving = false;

    private void Awake()
    {
        startingPosition = transform.position;

        collider = GetComponent<Collider>();
    }

    public void OnClick()
    {
        if (isMoving)
            return;

        if (!SlotManager.Instance.HasAvailableSlot())
        {
            return;
        }

        collider.enabled = false;


        var itemRef = Instantiate(cubeRefPrefab, transform.position, Quaternion.identity);
        var itemRefScript = itemRef.GetComponent<ItemRef>();
        itemRefScript.key = key;
        itemRefScript.SetColor(itemRenderer.material);
        itemRefScript.SetScale(transform.localScale);
        itemRefScript.cubeBlock = gameObject;
        SlotManager.Instance.Place(itemRefScript);
        
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);

        // audioManager.PlayAudio(audioManager.clickSound);
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

    public void SetColor(Material material)
    {
        itemRenderer.material = material;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {

    }
#endif
        // public bool isClickable = true; 
        // public float raycastHeight = 5f; 
        // public int raycastCount = 6; 
        // public LayerMask itemLayer;
        //
        // private Renderer objectRenderer;
        // private Color originalColor;
        // private Color selectedColor = Color.white;
        // public ItemType itemType;
        //
        // private void Start()
        // {
        //     objectRenderer = GetComponent<Renderer>();
        //     originalColor = objectRenderer.sharedMaterial.color;
        // }
        //
        // private void FixedUpdate()
        // {
        //     CheckClickable();
        // }
        //
        // public void CheckClickable()
        // {
        //     bool wasClickable = isClickable;  
        //     isClickable = true; 
        //
        //     float raycastInterval = transform.localScale.x / (raycastCount - 1); 
        //     Vector3 startRayPosition = transform.position + Vector3.up * raycastHeight;
        //
        //     for (int i = 0; i < raycastCount; i++)
        //     {
        //         Vector3 rayStart = startRayPosition + Vector3.right * (i * raycastInterval - transform.localScale.x / 2);
        //         RaycastHit hit;
        //
        //         if (Physics.Raycast(rayStart, Vector3.down, out hit, raycastHeight, itemLayer))
        //         {
        //             if (hit.collider.gameObject != gameObject)
        //             {
        //                 isClickable = false;
        //                 break; 
        //             }
        //         }
        //     }
        //
        //
        //     if (isClickable != wasClickable)
        //     {
        //         if (isClickable)
        //         {
        //             UpdateColorSelected(); 
        //         }
        //         else
        //         {
        //             UpdateColorNotSelected();
        //         }
        //     }
        // }
        //
        // public void OnSelected()
        // {
        //     if (isClickable)
        //     {
        //         SlotManager.Instance.SelectAndPlaceItem(gameObject);
        //     }
        //     else
        //     {
        //         Debug.Log("Not Clickable");
        //     }
        // }
        //
        // public void UpdateColorNotSelected()
        // {
        //
        //     foreach (Transform child in transform)
        //     {
        //         Renderer childRenderer = child.GetComponent<Renderer>();
        //         if (childRenderer != null)
        //         {
        //             childRenderer.material.DOColor(selectedColor, 0.1f);
        //         }
        //     }
        // }
        //
        // public void UpdateColorSelected()
        // {
        //
        //     foreach (Transform child in transform)
        //     {
        //         Renderer childRenderer = child.GetComponent<Renderer>();
        //         childRenderer.material.DOColor(originalColor, 0.5f);
        //     }
        // }
        //
        // private void OnDrawGizmosSelected()
        // {
        //     Gizmos.color = Color.red;
        //     float raycastInterval = transform.localScale.x / (raycastCount - 1);
        //     Vector3 startRayPosition = transform.position + Vector3.up * raycastHeight;
        //
        //     for (int i = 0; i < raycastCount; i++)
        //     {
        //         Vector3 rayStart = startRayPosition + Vector3.right * (i * raycastInterval - transform.localScale.x / 2);
        //         Gizmos.DrawRay(rayStart, Vector3.down * raycastHeight);
        //     }
        // }
    }
}
