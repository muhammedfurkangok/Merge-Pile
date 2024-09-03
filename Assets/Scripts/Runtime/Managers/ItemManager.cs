using UnityEngine;

namespace Runtime.Managers
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private GameObject ;
        
        private void SpawnGameObjectGivenPosition(GameObject gameObject, Vector3 position)
        {
            Instantiate(gameObject, position, Quaternion.identity);
        }
    }
}