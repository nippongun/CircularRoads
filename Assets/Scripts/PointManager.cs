using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    public List<Vector2> points;
    public List<Vector2> hull;
    [SerializeField]
    public int pointCount;
    public float min;
    public float limit;

    public void GeneratePoints() {
            points = new List<Vector2>();
            for (int i = 0; i < pointCount; i++)
            {
                Vector2 point = new Vector2(
                    (float)System.Math.Floor(Random.Range(min, limit)),
                    (float)System.Math.Floor(Random.Range(min, limit))
                );
                points.Add(point);
            } 
    }
    public void GenerateHull()
    {
        if(points != null)
        {
           hull = ConvexHull.CreateConvexHull(points);
        } else
        {
            GeneratePoints();
            hull = ConvexHull.CreateConvexHull(points);
        }
    }
}
