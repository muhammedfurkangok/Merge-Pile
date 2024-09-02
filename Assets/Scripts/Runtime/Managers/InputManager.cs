using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float sphereCastRadius;

    private bool isInputBlocked;
    
    void Update()
    {
        CheckInputBlock();
        if(!isInputBlocked) GetInput();
    }

    private void CheckInputBlock()
    {
        if (GameManager.Instance.GameStates != GameStates.Gameplay)
            isInputBlocked = true;
        else 
            isInputBlocked = false;
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
          
            if (Physics.SphereCast(ray,sphereCastRadius, out hit , Mathf.Infinity, layerMask))
            {
                hit.transform.GetComponent<Item>().OnSelected();
            }
        }
    }
}
