using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_GamePrefab", menuName = "ScriptableObjects/CD_GamePrefab", order = 0)]
    public class CD_GamePrefab : ScriptableObject
    {
        public GamePrefabData gamePrefab;
    }
}