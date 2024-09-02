using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Enums;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
    public bool isClickable = true; 
    public float raycastHeight = 5f; 
    public int raycastCount = 6; 
    public LayerMask itemLayer;
    
    private Renderer objectRenderer;
    private Color originalColor;
    private Color selectedColor = Color.gray;
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
            Debug.Log("Meyveye tıklandı: " + gameObject.name);
        }
        else
        {
            Debug.Log("Bu meyveye tıklanamaz: " + gameObject.name);
        }
    }
    
    public void UpdateColorNotSelected()
    {
        objectRenderer.material.DOColor(selectedColor, 0.5f); 
    }
    
    public void UpdateColorSelected()
    {
        objectRenderer.material.DOColor(originalColor, 0.5f); 
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
