using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PointManager))]
public class HullEditor : Editor
{
    PointManager manager;
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Generate new points"))
        {
            Undo.RecordObject(manager, "Generate new points");
            manager.GeneratePoints();
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    private void OnSceneGUI()
    {
        Draw();
    }

    void Draw()
    {
        Handles.color = Color.white;
        for (int i = 0; i < manager.pointCount-1; i++)
        {
            Handles.DrawLine(manager.points[i], manager.points[i + 1]);
            Handles.FreeMoveHandle(manager.points[i], Quaternion.identity, 0.5f,Vector2.zero,Handles.CylinderHandleCap);
        }
        Handles.DrawLine(manager.points[0], manager.points[manager.points.Count - 1]);
    }

    private void OnEnable()
    {
        manager = (PointManager)target;

        if (manager.points == null)
        {
            manager.GeneratePoints();
        }
    }
}
