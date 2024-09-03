using Runtime.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.Entities
{
    [System.Serializable]
    public class Slot : MonoBehaviour
    {
        public Image handlerSlotImage;
        public ItemType itemType;

        
        public void ChangeSprite(Sprite newSprite)
        {
            handlerSlotImage.sprite = newSprite;
        }
       
    }
}