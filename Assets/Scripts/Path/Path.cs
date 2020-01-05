using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path 
{
    [SerializeField, HideInInspector]
    List<Vector2> points;
    [SerializeField, HideInInspector]
    bool isClosed;
    [SerializeField, HideInInspector]
    bool autoSetControlPoints;
    public Path(Vector2 center)
    {
        points = new List<Vector2>
        {
            center + Vector2.left,
            center + (Vector2.left+Vector2.up)*0.5f,
            center + (Vector2.right+Vector2.down)*0.5f,
            center + Vector2.right
        };
    }

    public int NumberOfPoints{
        get
        {
            return points.Count;
        }
    }

    public int NumberOfSegements {
        get {
            return points.Count / 3;
        }
    }

    public Vector2 this[int i]{
        get
        {
            return points[i];
        }    
    }
    public bool AutoSetControlPoints
    {
        get { return autoSetControlPoints; }
        set { 
            if( autoSetControlPoints != value)
            {
                autoSetControlPoints = value;
                if (autoSetControlPoints)
                {
                    AutoSetAllControlPoints();
                }
            }         
        }
    }

    public bool IsClosed
    {
        get
        {
            return isClosed;
        }
        set
        {
            if (isClosed != value)
            {
                isClosed = value;

                if (isClosed)
                {
                    points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                    points.Add(points[0] * 2 - points[1]);
                    if (autoSetControlPoints)
                    {
                        AutoSetAnchorControlPoints(0);
                        AutoSetAnchorControlPoints(points.Count - 3);
                    }
                }
                else
                {
                    points.RemoveRange(points.Count - 2, 2);
                    if (autoSetControlPoints)
                    {
                        AutoSetControls();
                    }
                }
            }
        }
    }

    public void AddSegement(Vector2 anchor) {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchor)/2);
        points.Add(anchor);

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(points.Count - 1);
        }
    }

    public void RemoveSegement(int anchorIndex)
    {
        if (NumberOfSegements > 2 || !isClosed && NumberOfSegements > 1) {
            if (anchorIndex == 0)
            {
                if (isClosed)
                {
                    points[points.Count - 1] = points[2];
                }
                points.RemoveRange(0, 3);
            } else if (anchorIndex == points.Count - 1 && !isClosed)
            {
                points.RemoveRange(anchorIndex - 2, 3);
            } else
            {
                points.RemoveRange(anchorIndex - 1, 3);
            }
        }
    }

    public void SplitSegment(Vector2 anchorPosition, int segmentIndex)
    {
        points.InsertRange(segmentIndex * 3 + 2, new Vector2[] { Vector2.zero, anchorPosition, Vector2.zero });

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
        }
        else
        {
            AutoSetAnchorControlPoints(segmentIndex*3+3);
        }
    }
    public Vector2[] GetPointsInSegement(int i) {
        return new Vector2[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[LoopIndex(i * 3 + 3)] };
    }

    public void MovePoint(int i,Vector2 position)
    {
        Vector2 deltaMove = position - points[i];
        if (i%3==0 || !autoSetControlPoints)
        {
            points[i] = position;
            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(i);
            }
            else
            {

                if (i % 3 == 0)
                {
                    if (i + 1 < points.Count || isClosed)
                    {
                        points[LoopIndex(i + 1)] += deltaMove;
                    }
                    if (i - 1 >= 0)
                    {
                        points[LoopIndex(i - 1)] += deltaMove;
                    }
                }
                else
                {
                    bool nextAnchor = (i + 1) % 3 == 0;
                    int correspondingIndex = (nextAnchor) ? i + 2 : i - 2;
                    int anchorIndex = (nextAnchor) ? i + 1 : i - 1;

                    if (correspondingIndex >= 0 && correspondingIndex < points.Count || isClosed)
                    {
                        float distance = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingIndex)]).magnitude;
                        Vector2 direction = (points[LoopIndex(anchorIndex)] - position).normalized;
                        points[LoopIndex(correspondingIndex)] = points[LoopIndex(anchorIndex)] + direction * distance;
                    }
                }
            }
        }
    }

    public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector2> evenlySpacedPoints = new List<Vector2>();
        evenlySpacedPoints.Add(points[0]);
        Vector2 previousPoint = points[0];
        float distanceSinceLastPoint = 0;

        for (int i = 0; i < NumberOfSegements; i++)
        {
            Vector2[] p = GetPointsInSegement(i);
            float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
            float curveLength = Vector2.Distance(p[0], p[3]) + controlNetLength / 2;
            int division = Mathf.CeilToInt(curveLength * resolution * 10);
            float t = 0;
            while (t <= 1)
            {
                t += 1/division;
                Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3],t);
                distanceSinceLastPoint += Vector2.Distance(previousPoint, pointOnCurve);

                while(distanceSinceLastPoint >= spacing)
                {
                    float overshoot = distanceSinceLastPoint - spacing;
                    Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshoot;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    distanceSinceLastPoint = overshoot;
                    previousPoint = newEvenlySpacedPoint;
                }
                previousPoint = pointOnCurve;
            }
        }
        return evenlySpacedPoints.ToArray();
    }

    void AutoSetAllAffectedControlPoints(int anchorIndex)
    {
        for (int i = anchorIndex-3; i <= anchorIndex+3; i+=3)
        {
            if (i>=0 && i < points.Count || isClosed)
            {
                AutoSetAnchorControlPoints(LoopIndex(i));
            }
        }
        AutoSetControls();
    }

    void AutoSetAllControlPoints(){
        for (int i = 0; i < points.Count; i+=3)
        {
            AutoSetAnchorControlPoints(i);
        }
        AutoSetControls();
    }

    void AutoSetAnchorControlPoints(int anchorIndex) 
    {
        Vector2 anchorPosition = points[anchorIndex];
        Vector2 direction = Vector2.zero;
        float[] distances = new float[2];

        if (anchorIndex - 3 >= 0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex - 3)] - anchorPosition;
            direction += offset.normalized;
            distances[0] = offset.magnitude;
        }
        if (anchorIndex + 3 >= 0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex + 3)] - anchorPosition;
            direction -= offset.normalized;
            distances[1] = -offset.magnitude;
        }

        direction.Normalize();
        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0&& controlIndex <points.Count || isClosed)
            {
                points[LoopIndex(controlIndex)] = anchorPosition + direction * distances[i] / 2f;
            }
        }
    }

    void AutoSetControls()
    {
        if (!isClosed)
        {
            points[1] = (points[0] + points[2]) / 2f;
            points[points.Count - 2] = (points[points.Count - 1] + points[points.Count - 3]) / 2f;
        }
    }

    int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }
}
