using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Managers
{
    public class ItemManager : SingletonMonoBehaviour<ItemManager>
    {
        public void SpawnGameObjectGivenPosition(GameObject _gameObject , Vector3 position)
        {
            Instantiate(_gameObject, position, Quaternion.identity);
        }
    }
}