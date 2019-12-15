using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    public List<Point> points;
    public List<Line> lines;
    public List<Point> hull;
    public ConvexHull convexHull;
    public Point point;
    [SerializeField]
    public int pointCount;
    public float min;
    public float limit;

    private void Start()
    {
        points = new List<Point>();
        convexHull = new ConvexHull();
        GeneratePoints();
        CreateLines();
        hull = convexHull.CreateConvexHull(points);
    }

    public void GeneratePoints() {

        for (int i = 0; i < pointCount; i++)
        {
            Point point = new Point(
                (float)System.Math.Floor(Random.Range(min, limit)),
                (float)System.Math.Floor(Random.Range(min, limit))
            );
            Debug.Log("X:"+point.X);
            Debug.Log("Y"+point.Y);
            points.Add(point);
        }
    }

    public void CreateLines() {
        //hull = hull.OrderBy(p => p.X).ToList();
        
    }

}
