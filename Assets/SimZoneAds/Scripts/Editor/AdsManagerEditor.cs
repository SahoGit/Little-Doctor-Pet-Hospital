using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AdsManager), true)]
public class AdsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //DrawColoredBox(delegate
        //{
        //    EditorGUILayout.LabelField("Version", AdsManager.Version);
        //    EditorGUILayout.LabelField("Release Date", AdsManager.Release_Date);
        //});
        DrawPropertiesExcluding(serializedObject, "m_Script");

        serializedObject.ApplyModifiedProperties();
    }

    //private void DrawLineSeparator(Color color)
    //{
    //    Rect rect = EditorGUILayout.GetControlRect(false, 2f);
    //    EditorGUI.DrawRect(rect, color);
    //}

    //private void DrawColoredBox(System.Action content)
    //{
    //    GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
    //    boxStyle.normal.background = MakeTex(2, 10, new Color(0.175f, 0.175f, 0.175f));

    //    EditorGUILayout.BeginVertical(boxStyle);
    //    content.Invoke();
    //    EditorGUILayout.EndVertical();
    //}

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}