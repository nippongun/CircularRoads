using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexHull : MonoBehaviour
{
    public List<Point> CreateConvexHull(List<Point> points) {
        points.Sort();
        List<Point> hull = new List<Point>();

        foreach (Point point in points) {
            while (hull.Count >= 2 && !CounterClockWise(hull[hull.Count-2], hull[hull.Count - 2], point)) {
                hull.RemoveAt(hull.Count - 1);
            }
            hull.Add(point);
        }
        int tmp = hull.Count + 1;
        for (int i = points.Count -1 ; i >= 0; i--)
        {
            Point point = points[i];
            while (hull.Count > tmp && !CounterClockWise(hull[hull.Count - 2], hull[hull.Count - 2], point)) {
                hull.RemoveAt(hull.Count - 1);
            }
            hull.Add(point);
        }

        hull.RemoveAt(hull.Count - 1);
        return hull;
    }

    bool CounterClockWise(Point p1, Point p2, Point p3) {
        return (((p2.Y - p1.Y) * (p3.X - p1.X)) < (p2.X - p1.X) * (p3.Y - p1.Y ) );
    }
}
