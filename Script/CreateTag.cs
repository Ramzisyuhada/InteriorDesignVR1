using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateTag : MonoBehaviour
{
   
    void Awake()
    {
        Create();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Create()
    {
        // Open tag manager
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        // Tags Property
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        if (tagsProp.arraySize >= 10000)
        {
            Debug.Log("No more tags can be added to the Tags property. You have " + tagsProp.arraySize + " tags");
        }
        // if not found, add it
        if (!PropertyExists(tagsProp, 0, tagsProp.arraySize, "Walls"))
        {
            int index = tagsProp.arraySize;
            // Insert new array element
            tagsProp.InsertArrayElementAtIndex(index);
            SerializedProperty sp = tagsProp.GetArrayElementAtIndex(index);
            // Set array element to tagName
            sp.stringValue = "Walls";
            Debug.Log("Tag: " + "Walls" + " has been added");
            // Save settings
            tagManager.ApplyModifiedProperties();

        }
        else
        {
            Debug.Log ("Tag: " + "Walls" + " already exists");
        }
    }


    private static bool PropertyExists(SerializedProperty property, int start, int end, string value)
    {
        for (int i = start; i < end; i++)
        {
            SerializedProperty t = property.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(value))
            {
                return true;
            }
        }
        return false;
    }
}
