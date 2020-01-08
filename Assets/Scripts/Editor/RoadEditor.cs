using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadCreator))]
public class RoadEditor : Editor
{

    RoadCreator creator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate new random road"))
        {
            creator.GenerateRoad();
        }
    }

    void OnSceneGUI()
    {
        if (creator.autoUpdate && Event.current.type == EventType.Repaint)
        {
            creator.UpdateRoad();
        }
        Draw();
    }
    private void Draw() {
        for (int i = 0; i < creator.path.NumPoints; i++)
        {
            Handles.color = (i % 3 == 0) ? Color.red:Color.white;

            
            if (i == creator.path.NumPoints-1) Handles.color = Color.black;
            

            
            float handleSize = (i % 3 == 0) ? 0.5f: 0.7f;
            if ( i == 3)
            {
                Handles.color = Color.blue;
                handleSize = 1f;
            }
            Vector2 newPos = Handles.FreeMoveHandle(creator.path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
            if (creator.path[i] != newPos)
            {
                Undo.RecordObject(creator, "Move point");
                creator.path.MovePoint(i, newPos);
            }
        }
    }
    void OnEnable()
    {
        creator = (RoadCreator)target;
    }
}
