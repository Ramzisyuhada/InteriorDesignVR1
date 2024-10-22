using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallPen
{
    [CreateAssetMenu(fileName = "New Wallset", menuName = "InteriorDraw/Wallset", order = 0)]
    public class WallSet : ScriptableObject
    {
        public GameObject wall;
        public GameObject corner;
        public GameObject stub;
        public GameObject TPoint;
        public GameObject pole;
        public GameObject cross;
    }
}
