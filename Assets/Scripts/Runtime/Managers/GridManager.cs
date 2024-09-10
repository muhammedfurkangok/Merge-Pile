using System.Collections.Generic;
using Runtime.Entities;
using UnityEngine;

namespace Runtime.Managers
{
    public class GridManager : MonoBehaviour
    {
        public int Width;
        public int Height;
        public float SpaceModifier;

        public List<Item> _itemList;

        private void Awake()
        {
            _itemList = new List<Item>();
        }

        public void Initialize(int width, int height, float spaceModifier)
        {
            Width = width;
            Height = height;
            SpaceModifier = spaceModifier;
        }

        public void AddItem(Item item)
        {
            _itemList.Add(item);
        }

        public void UpdateItemPosition(Item item, Vector2Int newPosition)
        {
            // Eğer pozisyon bilgisi item'da varsa, buradan pozisyonu güncelleyebilirsin
            // Örneğin: item.SetPosition(newPosition);
        }

        public List<Item> GetItems()
        {
            return _itemList;
        }
        public void RemoveItem(Item item)
        {
            if (_itemList.Contains(item))
            {
                _itemList.Remove(item);
            }
        }
        public void ClearItems()
        {
            _itemList.Clear();
        }

        public Vector3 GridSpaceToWorldSpace(Vector2Int gridPosition)
        {
            return new Vector3(gridPosition.x * SpaceModifier, 0, gridPosition.y * SpaceModifier);
        }

        public Vector2Int WorldSpaceToGridSpace(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x / SpaceModifier);
            int y = Mathf.RoundToInt(worldPosition.z / SpaceModifier);
            return new Vector2Int(x, y);
        }

        public void SetDirty()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

       
    }
}