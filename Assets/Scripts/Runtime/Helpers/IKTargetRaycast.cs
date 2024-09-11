using System;
using UnityEngine;

namespace Runtime.Helpers
{
    public class IKTargetRaycast : MonoBehaviour
    {
        [Header("Ray Settings")]
        [SerializeField] private Transform handController; 
        [SerializeField] private Transform rayPoint;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Transform centralPoint;  
        [SerializeField] private float raycastDistance = 0.4f; 
        [SerializeField] private LayerMask raycastLayerMask;   

        [SerializeField] private Transform startPosition;
        

        private void FixedUpdate()
        {
          
            Vector3 rayDirection = (centralPoint.position - rayPoint.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(rayPoint.position + offset, rayDirection, out hit, raycastDistance, raycastLayerMask))
            {
                
                handController.position = hit.point;
            }
            else
            {
                handController.position = startPosition.position;
            }
        }

        private void OnDrawGizmos()
        {
            if (rayPoint != null && centralPoint != null)
            {
                
                Gizmos.color = Color.red;
                Vector3 rayDirection = (centralPoint.position - rayPoint.position).normalized;
                Gizmos.DrawLine(rayPoint.position + offset, rayPoint.position + offset + rayDirection * raycastDistance);
            }
        }
    }
}
