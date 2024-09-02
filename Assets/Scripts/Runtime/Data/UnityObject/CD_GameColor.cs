using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_GameColor", menuName = "ScriptableObjects/CD_GameColor", order = 0)]
    public class CD_GameColor : ScriptableObject
    {
       public GameColorsData[] gameColorsData;
    }
}