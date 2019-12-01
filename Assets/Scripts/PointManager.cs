using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    public List<Vector2> points;
    public List<Line> lines;
    public List<Hull> hulls;
    [SerializeField]
    public int pointCount;
    public float min;
    public float limit;

    private void Start()
    {
        // _ = gameObject.AddComponent<PointManager>();
        points = new List<Vector2>();
        GeneratePoints();
        CreateLines();
    }

    public void GeneratePoints() {

        for (int i = 0; i < pointCount; i++)
        {
            Vector2 v = new Vector2 {
            x = (float)System.Math.Floor(Random.Range(min, limit)),
            y = (float)System.Math.Floor(Random.Range(min, limit))
            };
            Debug.Log(v.x);
            Debug.Log(v.y);
            points.Add(v);
        }
    }

    public void CreateLines() {
        points = points.OrderBy(p => p.x ).ToList();
        int j = 0;
        for (int i = 0; i < points.Count; i+=2)
        {           
            lines.Add(new Line(points[i], points[i + 1]));
            _ = new Hull(lines[j]);
            Debug.DrawLine(lines[j].p1, lines[j].p2, Color.black, 100000);
            j++;
        }
    }


}
