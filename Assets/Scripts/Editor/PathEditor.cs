using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator pathCreator;
    Path path
    {
        get
        {
            return pathCreator.path;
        }
    }

    const float selectDistance = 0.1f;
    int selectedSegementIndex = -1;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create new"))
        {
            Undo.RecordObject(pathCreator, "Create new");
            pathCreator.CreatePath();           
        }

        bool isClosed = GUILayout.Toggle(path.IsClosed, "Closed");
        if (isClosed != path.IsClosed)
        {
            Undo.RecordObject(pathCreator, "Toggle closed");
            path.IsClosed = isClosed;
        }

        bool autoSetControlPoints = GUILayout.Toggle(path.AutoSetControlPoints, "Auto set control points");
        if (autoSetControlPoints != path.AutoSetControlPoints)
        {
            Undo.RecordObject(pathCreator, "Toggle auto set controls");
            path.AutoSetControlPoints = autoSetControlPoints;
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    private void OnEnable()
    {
        pathCreator = (PathCreator)target;
        if (pathCreator.path == null)
        {
            pathCreator.CreatePath();
        }
    }

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if (selectedSegementIndex != -1)
            {
                Undo.RecordObject(pathCreator, "Split segment");
                path.SplitSegment(mousePosition, selectedSegementIndex);
            }
            else if(!path.IsClosed)
            {
                Undo.RecordObject(pathCreator, "Add segment");
                path.AddSegement(mousePosition);
            }
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDistanceToAnchor = 0.05f;
            int closestAnchorIndex = -1;

            for (int i = 0; i < path.NumberOfPoints; i++)
            {
                float distance = Vector2.Distance(mousePosition, path[i]);
                if (distance < minDistanceToAnchor)
                {
                    minDistanceToAnchor = distance;
                    closestAnchorIndex = i;
                }

                if (closestAnchorIndex != -1)
                {
                    Undo.RecordObject(pathCreator, "Remove segment");
                    path.RemoveSegement(closestAnchorIndex);
                }
            }
        }

        if (guiEvent.type == EventType.MouseMove)
        {
            float minDistanceToSegement = pathCreator.anchorDiameter / 2f;
            int newSelectedSegementIndex = -1;

            for (int i = 0; i < path.NumberOfSegements; i++)
            {
                Vector2[] points = path.GetPointsInSegement(i);
                float distance = HandleUtility.DistancePointBezier(mousePosition, points[0], points[3], points[1], points[2]);
                if (distance < minDistanceToSegement)
                {
                    minDistanceToSegement = distance;
                    newSelectedSegementIndex = i;
                }
            }

            if (newSelectedSegementIndex != selectedSegementIndex)
            {
                selectedSegementIndex = newSelectedSegementIndex;
                HandleUtility.Repaint();
            }
        }
    }

    void Draw()
    {
        for (int i = 0; i < path.NumberOfSegements; i++)
        {
            Vector2[] points = path.GetPointsInSegement(i);
            if (pathCreator.displayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[0], points[1]);
                Handles.DrawLine(points[2], points[3]);
            }
            Color segementColor = (i == selectedSegementIndex && Event.current.shift) ? pathCreator.selectedSegmentCol : pathCreator.segmentCol;
            Handles.DrawBezier(points[0],points[3],points[1],points[2],segementColor,null,1.5f);
        }

        for (int i = 0; i < path.NumberOfPoints; i++)
        {
            if (i % 3 == 0 || pathCreator.displayControlPoints)
            {


                Handles.color = (i % 3 == 0) ? pathCreator.anchorCol : pathCreator.controlCol;
                float handleSize = (i % 3 == 0) ? pathCreator.anchorDiameter : pathCreator.controlDiameter;
                Vector2 newPosition = Handles.FreeMoveHandle(path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
                if (path[i] != newPosition)
                {
                    Undo.RecordObject(pathCreator, "Move point");
                    path.MovePoint(i, newPosition);
                }
            }
        }
    }
}
