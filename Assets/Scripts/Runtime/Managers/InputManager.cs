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
          
            if (Physics.SphereCast(ray, sphereCastRadius, out hit, Mathf.Infinity, layerMask))
            {
                // Nesnenin Renderer'ı varsa, en üst noktasını bulmak için bunu kullanıyoruz
                Renderer renderer = hit.transform.GetComponent<Renderer>();
                if (renderer != null)
                {
                  
                    Vector3 topPosition = renderer.bounds.center + new Vector3(0, renderer.bounds.extents.y, 0);
        
                   
        
                   
                    hit.transform.GetComponent<Item>().OnSelected();
                }
            }

        }
    }
}
