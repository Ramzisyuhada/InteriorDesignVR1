using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallPen
{
    public class WallpenInteriorGizmos : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void OnDrawGizmos()
        {
            WallpenInterior i = GetComponent<WallpenInterior>();
            if(i.cells != null)
            {
                foreach (TileCell cell in i.cells)
                {
                    if (cell.type == TileCell.CellType.Wall)
                        Gizmos.color = Color.black;

                    if (i.IsCellJunction(cell.position.x, cell.position.y))
                        Gizmos.color = Color.blue;

                    if (cell.type == TileCell.CellType.Empty)
                        Gizmos.color = Color.white;

                    Color c = Gizmos.color;
                    Gizmos.DrawCube(i.InteriorToWorld(cell.position), Vector3.one);
                }
            }
        }
    }
}
