using Runtime.Entities;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Managers
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float sphereCastRadius;

        private bool isInputDisable ;
    
        void Update()
        {
            if(!isInputDisable) GetInput();
        }

    
        public void EnableInput()
        {
            isInputDisable = false;
        }

        public void DisableInput()
        {
            isInputDisable = true;
        }

        private void GetInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
          
                if (Physics.SphereCast(ray, sphereCastRadius, out hit, Mathf.Infinity, layerMask))
                {
                    var childRenderer = hit.transform.GetComponentInChildren<Renderer>();
                    var Item = hit.transform.GetComponent<Item>();
                    Renderer renderer = childRenderer;
                    Vector3 topPosition = renderer.bounds.center + new Vector3(0, renderer.bounds.extents.y, 0);
                    PlayerManager.Instance.MovePlayerByGivenPosition(topPosition,Item);
                    Item.OnClick();
                 
                }

            }
        }
    
    }
}
