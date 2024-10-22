using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WallPen;

namespace WallPenEditor
{
    [CustomEditor(typeof(WallpenInterior))]
    public class WallpenInteriorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WallpenInterior interior = (WallpenInterior)target;
            if(GUILayout.Button("Apply!"))
            {
                interior.Clear();
                interior.InitializeInterior();
            }
            EditorGUILayout.HelpBox("Watch out: Changing the size of your interior will completely clear the interior!\nLSHIFT+M1 to draw\nLCTRL+M1 to erase", MessageType.Info);
        }
    }
}
