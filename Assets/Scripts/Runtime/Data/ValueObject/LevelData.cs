using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public class LevelData
    {
        public int levelNumber;
        public int Width;
        public int Height;
        public GridData[] Grids;

        public GridData GetGrid(int x, int y)
        {
            return Grids[x * Width + y];
        }

        public void SetGrid(int x, int y, GridData gridData)
        {
            Grids[x * Width + y] = gridData;
        }
    }
}