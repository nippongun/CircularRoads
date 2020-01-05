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

        if (GUILayout.Button("Generate new hull"))
        {
            Undo.RecordObject(manager, "Generate new hull");
            manager.GenerateHull();
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
        for (int i = 0; i < manager.hull.Count - 1; i++)
        {
            Handles.DrawLine(manager.hull[i], manager.hull[i + 1]);
        }
        Handles.DrawLine(manager.hull[0], manager.hull[manager.hull.Count-1]);
        for (int i = 0; i < manager.pointCount; i++)
        {
            Handles.FreeMoveHandle(manager.points[i], Quaternion.identity, 0.5f,Vector2.zero,Handles.CylinderHandleCap);
        }
    }

    private void OnEnable()
    {
        manager = (PointManager)target;

        if (manager.points == null)
        {
            manager.GenerateHull();
        }
    }
}
