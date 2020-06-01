using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ImageProcessor))]
public class ImageProcessorEditor : Editor
{
    SerializedProperty inputImage;

    private void OnEnable()
    {
        /*
        inputImage = serializedObject.FindProperty("inputImage");
        UpdateImage();
        */
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        /*
        serializedObject.Update();
        CheckForImageUpdate();
        serializedObject.ApplyModifiedProperties();
        ImageProcessor ip = (ImageProcessor)target;
        if (GUILayout.Button("Copy image colors"))
        {
            ip.DisplayFirstTile(2);
        }
        if (GUILayout.Button("Reset image colors"))
        {
            ip.ResetTileImage();
        }
        if (GUILayout.Button("List colors"))
        {
            ip.ListColors();
        }
        */
        
    }

    void CheckForImageUpdate()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.ObjectField(inputImage, typeof(Sprite));
        if (EditorGUI.EndChangeCheck())
        {
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        ImageProcessor processor = (ImageProcessor)target;
        processor.UpdateDisplayInputImage((Sprite)inputImage.objectReferenceValue);
    }
}
