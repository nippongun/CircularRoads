using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    public List<Vector2> points;
    [SerializeField]
    public int pointCount = 10;
    public float min = 0;
    public float limit = 20;
    public bool autoUpdate;

    public void GeneratePoints()
    {
        points = new List<Vector2>();

        float phi = 2*Mathf.PI/ pointCount;
        float currentPhi = 0;
        float radius;

        float controlRadius;
        for (int i = 0; i < pointCount ; i++)
        {
            radius = Random.Range(min, limit);
            points.Add(new Vector2(Mathf.Cos(currentPhi) * radius, Mathf.Sin(currentPhi) * radius));
            currentPhi += phi;
        }
    }
}
