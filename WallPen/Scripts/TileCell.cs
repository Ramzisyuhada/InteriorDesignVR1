using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WallPen
{
    [System.Serializable]
    public class TileCell
    {
        public Vector2Int position;
        public enum CellType
        {
            Wall,
            Empty,
            OutOfBounds
        }
        public CellType type;

        public int roomID = -1; //-1 means unassigned.

        public GameObject prefab;

        public TileCell(Vector2Int position, CellType type)
        {
            this.position = position;
            this.type = type;
        }
    }
}
