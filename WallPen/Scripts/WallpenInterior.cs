using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace WallPen
{
    /// <summary>
    /// Holds all data about a interior. You can remove this from a interior object if you want, but it won't paint properly.
    /// </summary>
    [System.Serializable]
    public class WallpenInterior : MonoBehaviour
    {
        /// <summary>
        /// How big the interior can get.
        /// </summary>
        public int MaxInteriorSize = 20;
        public List<Wall> Walls;
        //Unity doesn't know how to serialize a 2D array
        //So to serialize interiorMap, I'm converting it into a list which unity CAN serialize), and reconstructing it after using MaxInteriorSize
        [HideInInspector]public TileCell[] cells;

        void Reset()
        {
            InitializeInterior();
        }

        public void InitializeInterior()
        {
            #region Initialising the interior
            cells = new TileCell[MaxInteriorSize*MaxInteriorSize];
            Walls = new List<Wall>();

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new TileCell(indexToCoordinate(i), TileCell.CellType.Empty);
            }
            #endregion
        }

        public void Clear()
        {
            foreach (Transform t in transform)
                DestroyImmediate(t.gameObject);
        }

        public Vector2Int WorldToInterior(Vector3 pos)
        {
            return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
        }

        public Vector3 InteriorToWorld(Vector2Int pos)
        {
            return new Vector3(pos.x, 0, pos.y);
        }

        public bool IsCellJunction(int x, int y, List<TileCell.CellType> extraTypes = null)
        {
            Neighbours neighbours = GetNeighbours(x, y);
            Vector3 direction = VectorUtils.GridToWorld(VectorUtils.AverageDirection(neighbours.totalDirections.ToArray()));
            bool isCorner = direction != Vector3.zero;
            return neighbours.totalDirections.Count == 1 || (neighbours.totalDirections.Count == 2 && isCorner) || neighbours.totalDirections.Count == 4 || neighbours.totalDirections.Count == 8 || neighbours.totalDirections.Count == 3 || neighbours.totalDirections.Count == 6 || neighbours.totalDirections.Count == 12 || neighbours.totalDirections.Count == 9 || neighbours.totalDirections.Count == 11 || neighbours.totalDirections.Count == 7 || neighbours.totalDirections.Count == 14 || neighbours.totalDirections.Count == 13 || neighbours.totalDirections.Count == 0;
        }

        public Neighbours GetNeighbours(int xCoord, int yCoord)
        {
            Neighbours neighbours = new Neighbours();

            neighbours.totalDirections = new List<Vector2Int>();
            //Check top
            if (coordinateInRange(xCoord, yCoord + 1))
            {
                if (cells[coordinateToIndex(xCoord, yCoord + 1)].type == TileCell.CellType.Wall)
                {
                    neighbours.hasUpDir = true;
                    neighbours.totalDirections.Add(Vector2Int.up);
                }

            }
            //Check Bottom
            if (coordinateInRange(xCoord, yCoord - 1))
            {
                if (cells[coordinateToIndex(xCoord, yCoord - 1)].type == TileCell.CellType.Wall)
                {
                    neighbours.hasDownDir = true;
                    neighbours.totalDirections.Add(Vector2Int.down);
                }

            }
            //Check left
            if (coordinateInRange(xCoord - 1, yCoord))
            {
                if (cells[coordinateToIndex(xCoord - 1, yCoord)].type == TileCell.CellType.Wall)
                {
                    neighbours.hasRightDir = true;
                    neighbours.totalDirections.Add(Vector2Int.left);
                }

            }
            //Check right
            if (coordinateInRange(xCoord + 1, yCoord))
            {
                if (cells[coordinateToIndex(xCoord + 1, yCoord)].type == TileCell.CellType.Wall)
                {
                    neighbours.hasLeftDir = true;
                    neighbours.totalDirections.Add(Vector2Int.right);
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Checks if a coordinate is in range.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool coordinateInRange(int x, int y)
        {
            return x >= 0 && x < MaxInteriorSize && y >= 0 && y < MaxInteriorSize;
        }

        public int coordinateToIndex(Vector2Int coord)
        {
            return coord.x + (coord.y * MaxInteriorSize);
        }

        public int coordinateToIndex(int x, int y)
        {
            return x + (y * MaxInteriorSize);
        }

        public Vector2Int indexToCoordinate(int index)
        {
            int x = index % MaxInteriorSize;
            int y = index / MaxInteriorSize;
            return new Vector2Int(x, y);
        }
    }

    public class Neighbours
    {
        public bool hasUpDir;
        public bool hasDownDir;
        public bool hasRightDir;
        public bool hasLeftDir;

        public List<Vector2Int> totalDirections;
    }

    public class Wall
    {
        public TileCell one;
        public TileCell two;
        public GameObject wallPrefab;

        public Wall(TileCell from, TileCell to, GameObject prefab)
        {
            one = from;
            two = to;
            wallPrefab = prefab;
        }

        public bool WallHasCells(TileCell cell1, TileCell cell2)
        {
            return (cell1 == one || cell2 == one) && (cell2 == two || cell2 == one);
        }
    }
}
