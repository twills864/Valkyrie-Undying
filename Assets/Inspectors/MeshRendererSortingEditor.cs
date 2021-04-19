using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

#if UNITY_EDITOR

/// This just exposes the Sorting Layer / Order in MeshRenderer since it's there
/// but not displayed in the inspector. Getting MeshRenderer to render in front
/// of a SpriteRenderer is pretty hard without this.
/// Adapted from https://gist.github.com/sinbad/bd0c49bc462289fa1a018ffd70d806e3
/// With changes from https://forum.unity.com/threads/extending-mesh-renderer-component-with-a-custom-editor.949176/
/// to preserve the Unity MeshRenderer GUI.
[CustomEditor(typeof(MeshRenderer))]
[CanEditMultipleObjects]
public class MeshRendererSortingEditor : Editor
{
    private Editor defaultEditor;
    private MeshRenderer meshRenderer;
    private static bool showSorting = true;
    private string header = "2D Sorting";

    private SerializedProperty sortingLayerIdProperty;
    private SerializedProperty sortingOrderProperty;

    private void OnEnable()
    {
        defaultEditor = CreateEditor(targets, Type.GetType("UnityEditor.MeshRendererEditor, UnityEditor"));
        meshRenderer = target as MeshRenderer;

        sortingLayerIdProperty = serializedObject.FindProperty("m_SortingLayerID");
        sortingOrderProperty = serializedObject.FindProperty("m_SortingOrder");
    }

    private void OnDisable()
    {
        //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
        //Also, make sure to call any required methods like OnDisable
        var disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (disableMethod != null)
            disableMethod.Invoke(defaultEditor, null);
        DestroyImmediate(defaultEditor);
    }

    public override void OnInspectorGUI()
    {
        defaultEditor.OnInspectorGUI();

        serializedObject.Update();

        showSorting = EditorGUILayout.BeginFoldoutHeaderGroup(showSorting, header);
        if (showSorting)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            var newId = DrawSortingLayersPopup(meshRenderer.sortingLayerID);
            if (EditorGUI.EndChangeCheck())
            {
                sortingLayerIdProperty.intValue = newId;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            var order = EditorGUILayout.IntField("Sorting Order", meshRenderer.sortingOrder);
            if (EditorGUI.EndChangeCheck())
            {
                sortingOrderProperty.intValue = order;
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }

    static int DrawSortingLayersPopup(int layerID)
    {
        var layers = SortingLayer.layers;
        var names = layers.Select(l => l.name).ToArray();
        if (!SortingLayer.IsValid(layerID))
        {
            layerID = layers[0].id;
        }
        var layerValue = SortingLayer.GetLayerValueFromID(layerID);
        var newLayerValue = EditorGUILayout.Popup("Sorting Layer", layerValue, names);
        return layers[newLayerValue].id;
    }

}

#endif

// This is free and unencumbered software released into the public domain.
//
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
//
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
// For more information, please refer to <http://unlicense.org/>