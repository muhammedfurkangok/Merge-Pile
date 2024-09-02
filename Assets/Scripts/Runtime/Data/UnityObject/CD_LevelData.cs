using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_LevelTime", menuName = "ScriptableObjects/CD_LevelTime", order = 0)]
    public class CD_LevelData : ScriptableObject
    {
      public LevelData levelData;
    }
}