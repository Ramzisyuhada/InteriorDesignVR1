using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WallPen.Editor
{
    [System.Serializable]
    public class ToolbarPreferences : EditorWindow
    {

        public WallPenEditor editor;
        void OnGUI()
        {
            if (editor.mode == WallPenEditor.BuildMode.Walls)
            {
                EditorGUILayout.LabelField("Wall Settings", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("CONTROLS:\nShift+Left Click to draw\nCtrl+Left Click to erase", MessageType.Info);
            }
            else if (editor.mode == WallPenEditor.BuildMode.Floors)
            {
                EditorGUILayout.LabelField("Floor Settings", EditorStyles.boldLabel);
                editor.floorMaterial = (Material)EditorGUILayout.ObjectField(editor.floorMaterial, typeof(Material), false);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Floor direction");
                string text = "Up";
                if (!editor.floorFaceDirection)
                    text = "Down";
                if (GUILayout.Button(text))
                    editor.floorFaceDirection = !editor.floorFaceDirection;
                GUILayout.EndHorizontal();
                EditorGUILayout.HelpBox("CONTROLS:\nPress Shift+Left Click to draw", MessageType.Info);
            }
        }
    }
}
