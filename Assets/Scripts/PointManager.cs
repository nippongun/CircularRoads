using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    public List<Vector2> pointList;

    [SerializeField]
    public int points;
    public float min;
    public float limit;

    private void Start()
    {
        // _ = gameObject.AddComponent<PointManager>();
        pointList = new List<Vector2>();
        generatePoints();
    }

    public void generatePoints() {

        for (int i = 0; i < points; i++)
        {
            Vector2 v = new Vector2 {
            x = (float)System.Math.Floor(Random.Range(min, limit)),
            y = (float)System.Math.Floor(Random.Range(min, limit))
            };
            Debug.Log(v.x);
            Debug.Log(v.y);
            pointList.Add(v);
        }
    }
}
