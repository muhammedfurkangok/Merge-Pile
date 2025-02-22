using System;
using Runtime.Entities;
using Runtime.Enums;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Managers
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float sphereCastRadius;
        
        private UtilityType activeUtility;
        private bool utilityActive = false;

        private bool isInputDisable ;
        
        public bool isInGamePlayState = false;
        

        void Update()
        {
            if(!isInputDisable && !utilityActive && !isInGamePlayState) GetInput();
            
            if(utilityActive) RayForUtilty();
            
        }

        public void EnableInput()
        {
            isInputDisable = false;
        }

        public void DisableInput()
        {
            isInputDisable = true;
        }
        
        public void SetUtilityActive(UtilityType utilityType)
        {
            activeUtility = utilityType;
            utilityActive = true;
        }
        
        public void SetUtilityInactive()
        {
            utilityActive = false;
        }

        public  void RayForUtilty()
        {
            if ( Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Item")))
                {
                  if (!hit.collider.gameObject.GetComponent<Item>().canClickable)
                  {
                      GameObject target = hit.collider.gameObject;
                      UtilityManager.Instance.ApplyUtilityToObject(activeUtility, target.GetComponent<Item>());
                      utilityActive = false;
                  }
                }
            }
            
        }   

        private void GetInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
          
                if (Physics.SphereCast(ray, sphereCastRadius, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.transform.GetComponent<Item>().canClickable)
                    {
                        var childRenderer = hit.transform.GetComponentInChildren<Renderer>();
                        var Item = hit.transform.GetComponent<Item>();
                        Renderer renderer = childRenderer;
                        Vector3 topPosition = renderer.bounds.center + new Vector3(0, renderer.bounds.extents.y, 0);
                        
                        PlayerManager.Instance.MovePlayerByGivenPosition(topPosition,Item);
                    }
                }

            }
        }
        
    }
}
