using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isClickable = true; 
    public float raycastHeight = 5f; 
    public int raycastCount = 6; 
    public LayerMask itemLayer;
    
    private Renderer objectRenderer;
    private Color originalColor;
    private Color selectedColor = Color.white;
    public ItemType itemType;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.sharedMaterial.color;
    }

    private void FixedUpdate()
    {
        CheckClickable();
    }

    public void CheckClickable()
    {
        bool wasClickable = isClickable;  
        isClickable = true; 

        float raycastInterval = transform.localScale.x / (raycastCount - 1); 
        Vector3 startRayPosition = transform.position + Vector3.up * raycastHeight;

        for (int i = 0; i < raycastCount; i++)
        {
            Vector3 rayStart = startRayPosition + Vector3.right * (i * raycastInterval - transform.localScale.x / 2);
            RaycastHit hit;

            if (Physics.Raycast(rayStart, Vector3.down, out hit, raycastHeight, itemLayer))
            {
                if (hit.collider.gameObject != gameObject)
                {
                    isClickable = false;
                    break; 
                }
            }
        }

       
        if (isClickable != wasClickable)
        {
            if (isClickable)
            {
                UpdateColorSelected(); 
            }
            else
            {
                UpdateColorNotSelected();
            }
        }
    }

    public void OnSelected()
    {
        if (isClickable)
        {
            SlotManager.Instance.SelectAndPlaceItem(gameObject);
        }
        else
        {
            Debug.Log("Not Clickable");
        }
    }
    
    public void UpdateColorNotSelected()
    {

        foreach (Transform child in transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.material.DOColor(selectedColor, 0.1f);
            }
        }
    }

    public void UpdateColorSelected()
    {

        foreach (Transform child in transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            childRenderer.material.DOColor(originalColor, 0.5f);
        }
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
    
          public string key;
        public MeshRenderer renderer;
        public GameObject cubeBlock;
    
        // public Quaternion rotationDifference;
    
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
    
        // public void SetRotationDifference(Quaternion rotationDifference)
        // {
        //     this.rotationDifference = rotationDifference;
        // }
    
        public void SetScale(Vector3 scale)
        { 
            this.scale = scale;
        }
    
        public Tween LocalMoveTo(Vector3 pos)
        {
            moveTween = transform.DOLocalMove(pos, 0.4f).SetEase(Ease.InBack);
            return moveTween;
        }
}
