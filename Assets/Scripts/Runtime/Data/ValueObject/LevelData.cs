using System;
using UnityEngine;
using Runtime.Enums;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class LevelData
    {
        public int levelNumber;
        public int Width;
        public int Height;
        public GridData[] Grids;

        public GridData GetGrid(int x, int y)
        {
           
            return Grids[x * Width + y]; // Corrected grid indexing
        }

        public void SetGrid(int x, int y, GridData gridData)
        {
           
            Grids[x * Width + y] = gridData; // Corrected grid indexing
        }
    }
}