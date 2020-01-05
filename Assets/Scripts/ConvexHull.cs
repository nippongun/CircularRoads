using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class ConvexHull 
{
    public static List<Vector2> CreateConvexHull(List<Vector2> points) {
        points = points.OrderBy(x => x.x).ToList();
        List<Vector2> hull = new List<Vector2>();

        foreach (Vector2 point in points) {
            while (hull.Count >= 2 && !CounterClockWise(hull[hull.Count-2], hull[hull.Count - 1], point)) {
                hull.RemoveAt(hull.Count - 1);
            }
            hull.Add(point);
        }
        int tmp = hull.Count + 1;
        for (int i = points.Count -1 ; i >= 0; i--)
        {
            Vector2 point = points[i];
            while (hull.Count > tmp && !CounterClockWise(hull[hull.Count - 2], hull[hull.Count - 1], point)) {
                hull.RemoveAt(hull.Count - 1);
            }
            hull.Add(point);
        }

        hull.RemoveAt(hull.Count - 1);
        return hull;
    }

    static bool CounterClockWise(Vector2 p1, Vector2 p2, Vector2 p3) {
        return (((p2.y - p1.y) * (p3.x - p1.x)) < (p2.x - p1.x) * (p3.x - p1.x ) );
    }
}
