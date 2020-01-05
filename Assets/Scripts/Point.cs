using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : IComparable<Point>
{
    private Vector2 v;

    public Point(float x, float y) {
        v.x = x;
        v.y = y;
    }
    public float Y
    {
        get { return v.y; }
    }
    public float X
    {
        get { return v.x; }
    }
    public int CompareTo(Point point)
    {
        return X.CompareTo(point.X);
    }
}
