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
        GeneratePoints();
        convexHull = new ConvexHull();
        hull = convexHull.CreateConvexHull(points);
        CreateLines();
        
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
        for (int i = 0; i < hull.Count-1; i++)
        {
            Debug.Log("Hull X:"+hull[i].X);
            Debug.DrawLine(new Vector2(hull[i].X,hull[i].Y), new Vector2(hull[i+1].X, hull[i+1].Y), new Color(0,0,0),200000);
        }
        Debug.DrawLine(new Vector2(hull[0].X, hull[0].Y), new Vector2(hull[hull.Count-1].X, hull[hull.Count-1].Y), new Color(0, 0, 0), 200000);
    }

}
